using UnityEngine;
using UnityEngine.EventSystems;

public class Hook : Offable
{
    Vector2 clickPoint;
    public KeyCode launchKey = KeyCode.Mouse0;
    private bool isMovingToClicked = false;
    private bool isReturning = false;
    private Pullable grabbedObject = null;
    private Vector3 originalPosition;

    Rotatable rotatable;
    StatsManager statsManager;

    private Vector3 wireStartPoint;
    [SerializeField] Transform wireTransform;

    protected override void Awake()
    {
        base.Awake();
        rotatable = GetComponent<Rotatable>();
        statsManager = FindAnyObjectByType<StatsManager>();
    }

    protected override void Start()
    {
        base.Start();
        originalPosition = transform.position;
        rotatable.CanRotate = true;
        wireStartPoint = transform.position;
        ResetWire();
    }

    void ResetWire()
    {
        wireTransform.position = wireStartPoint;
        wireTransform.localScale = new Vector3(0, wireTransform.localScale.y, wireTransform.localScale.z);
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;
        if (!IsOn)
            return;

        // Проверка: если курсор над UI, не запускаем хук
        if (Input.GetKeyDown(launchKey) && !isMovingToClicked && !isReturning)
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            if (statsManager.TryConsumeEnergy(AllStatsContainer.Instance.HookEnergyCost))
            {
                GameStateManager.Instance.Play("hitHurt");
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                // increase click point distance as a help to reach further
                Vector2 extra = (mousePos - (Vector2)transform.position).normalized;
                clickPoint = mousePos + extra * AllStatsContainer.Instance.HookExtraDistance;
                isMovingToClicked = true;
                rotatable.CanRotate = false;
            }
        }

        if (isMovingToClicked)
        {
            MoveToClicked();
        }
        else if (isReturning)
        {
            ReturnBack();
        }

        UpdateWire();
    }

    void MoveToClicked()
    {
        transform.position = Vector3.MoveTowards(transform.position, clickPoint, AllStatsContainer.Instance.HookSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, clickPoint) < 0.1f)
        {
            isMovingToClicked = false;
            isReturning = true;
        }
    }

    void ReturnBack()
    {
        transform.position = Vector3.MoveTowards(transform.position, originalPosition, AllStatsContainer.Instance.HookSpeed * Time.deltaTime);
        if (grabbedObject)
        {
            grabbedObject.transform.position = transform.position;
        }
        if (Vector3.Distance(transform.position, originalPosition) < 0.1f)
        {
            transform.position = originalPosition;
            isReturning = false;
            if (grabbedObject)
            {
                // Отпустить объект
                grabbedObject.TryPickup();
                grabbedObject = null;
            }
            rotatable.CanRotate = true;
            ResetWire();
        }
    }

    void UpdateWire()
    {
        if (!isMovingToClicked && !isReturning)
        {
            return;
        }
        Vector3 start = wireStartPoint;
        Vector3 end = transform.position;
        Vector3 dir = end - start;
        dir.z = 0;
        float distance = dir.magnitude;

        // Позиция Wire — середина между стартом и хуком
        wireTransform.position = start + dir * 0.5f;

        // Поворот Wire — в сторону хука
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        wireTransform.rotation = Quaternion.Euler(0, 0, angle);

        Vector3 scale = wireTransform.localScale;
        scale.x = distance * 3.5f;
        wireTransform.localScale = scale;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D: " + other.gameObject.name);

        if (isMovingToClicked && other.CompareTag("Pullable"))
        {
            GameStateManager.Instance.Play("hitHurt");
            Debug.Log("Hooked onto: " + other.gameObject.name);
            grabbedObject = other.GetComponent<Pullable>();
            grabbedObject.Hook();
            isMovingToClicked = false;
            isReturning = true;
        }
    }

    public override bool TryOff()
    {
        if (isMovingToClicked || isReturning)
        {
            return false;
        }
        rotatable.CanRotate = false;
        return base.TryOff();
    }

    public void ForceOff()
    {
        rotatable.CanRotate = false;
        base.TryOff();
    }

    public override void On()
    {
        base.On();
        rotatable.CanRotate = !isMovingToClicked && !isReturning;
    }
}
