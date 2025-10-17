using UnityEngine;
using UnityEngine.EventSystems;
public class Gun : Offable
{
    Rotatable rotatable;
    StatsManager statsManager;

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

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            if (statsManager.TryConsumeEnergy(AllStatsContainer.Instance.GunEnergyCost))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                // todo spawn bullet towards mousePos
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
