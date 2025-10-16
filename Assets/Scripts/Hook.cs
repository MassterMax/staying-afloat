using UnityEngine;

public class Hook : MonoBehaviour
{
    Vector2 clickPoint;
    public float speed = 10f;
    public KeyCode launchKey = KeyCode.Mouse0;
    private bool isMovingToClicked = false;
    private bool isReturning = false;
    private GameObject grabbedObject = null;
    private Vector3 originalPosition;

    Rotatable rotatable;

    void Start()
    {
        originalPosition = transform.position;
        rotatable = GetComponent<Rotatable>();
        rotatable.CanRotate = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(launchKey) && !isMovingToClicked && !isReturning)
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
        }
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
