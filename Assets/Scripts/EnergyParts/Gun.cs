using UnityEngine;
using UnityEngine.EventSystems;
public class Gun : Offable
{
    Rotatable rotatable;
    StatsManager statsManager;
    [SerializeField] private GameObject bulletPrefab;
    float timeFromLastShot = 0f;

    protected override void Awake()
    {
        base.Awake();
        rotatable = GetComponent<Rotatable>();
        statsManager = FindAnyObjectByType<StatsManager>();
    }

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        if (!IsOn)
            return;
        timeFromLastShot += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            if (timeFromLastShot < AllStatsContainer.Instance.GunShotDelay)
                return;

            if (statsManager.TryConsumeEnergy(AllStatsContainer.Instance.GunEnergyCost))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                // todo spawn bullet towards mousePos
                // spawn bullet towards mousePos
                Vector2 origin = (Vector2)transform.position;
                Vector2 dir = (mousePos - origin).normalized;

                if (bulletPrefab != null)
                {
                    GameObject go = Instantiate(bulletPrefab, origin, Quaternion.identity);
                    go.transform.up = dir;
                    LaserShot shot = go.GetComponent<LaserShot>();
                    if (shot != null)
                    {
                        shot.Init(dir, AllStatsContainer.Instance.GunShotSpeed, AllStatsContainer.Instance.GunShotLifetime);
                    }
                    else
                    {
                        Debug.LogWarning("Bullet prefab does not have a LaserShot component.");
                    }
                }
                timeFromLastShot = 0f;
            }
        }
    }

    public override bool TryOff()
    {
        rotatable.CanRotate = false;
        return base.TryOff();
    }

    public override void On()
    {
        base.On();
        rotatable.CanRotate = true;
    }
}
