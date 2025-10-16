using UnityEngine;

public class Hook : MonoBehaviour
{
    private const float ENERGY_COST = 1f;
    Vector2 clickPoint;
    public float speed = 10f;
    public KeyCode launchKey = KeyCode.Mouse0;
    private bool isMovingToClicked = false;
    private bool isReturning = false;
    private GameObject grabbedObject = null;
    private Vector3 originalPosition;

    Rotatable rotatable;
    StatsManager statsManager;

    private Vector3 wireStartPoint;
    [SerializeField] Transform wireTransform;

    void Start()
    {
        originalPosition = transform.position;
        rotatable = GetComponent<Rotatable>();
        rotatable.CanRotate = true;
        statsManager = FindAnyObjectByType<StatsManager>();
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
        if (Input.GetKeyDown(launchKey) && !isMovingToClicked && !isReturning && statsManager.TryConsumeEnergy(ENERGY_COST))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPoint = mousePos;
            isMovingToClicked = true;
            rotatable.CanRotate = false;
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
        transform.position = Vector3.MoveTowards(transform.position, clickPoint, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, clickPoint) < 0.1f)
        {
            isMovingToClicked = false;
            isReturning = true;
        }
    }

    void ReturnBack()
    {
        transform.position = Vector3.MoveTowards(transform.position, originalPosition, speed * Time.deltaTime);
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
            Debug.Log("Hooked onto: " + other.gameObject.name);
            grabbedObject = other.gameObject;
            isMovingToClicked = false;
            isReturning = true;
        }
    }
}
