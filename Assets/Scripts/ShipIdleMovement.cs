using UnityEngine;

public class ShipIdleMovement : MonoBehaviour
{
    [SerializeField] float swayAmount = 0.5f; // насколько далеко корабль может шататься
    [SerializeField] float swaySpeed = 1f; // скорость колебаний
    [SerializeField] float rotationAmount = 5f; // угол поворота в градусах
    [SerializeField] float rotationSpeed = 1f; // скорость вращения

    private Vector2 startPosition;
    private float startRotationZ;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        // rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        startPosition = rb.position;
        startRotationZ = rb.rotation;
    }

    void FixedUpdate()
    {
        // Плавное шатание по X и Y (используем Time.time, вызов MovePosition в FixedUpdate)
        float swayX = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
        float swayY = Mathf.Cos(Time.time * swaySpeed * 0.5f) * swayAmount * 0.5f;
        Vector2 offset = new Vector2(swayX, swayY);

        Vector2 targetPos = startPosition + offset;
        rb.MovePosition(targetPos);

        // Плавное вращение через Rigidbody2D
        float rotationZ = Mathf.Sin(Time.time * rotationSpeed) * rotationAmount;
        rb.MoveRotation(startRotationZ + rotationZ);
    }

    // Обработка столкновений — поддерживается и для Trigger и для Collision
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null) return;
        if (other.CompareTag("Asteroid"))
        {
            Debug.Log("Ship hit — apply damage (trigger)");
            other.GetComponent<Asteroid>()?.Explode();
            // TODO: вызвать метод у компонента здоровья, например:
            // GetComponent<Health>()?.ApplyDamage(damageAmount);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider == null) return;
        if (collision.collider.CompareTag("Asteroid"))
        {
            Debug.Log("Ship hit — apply damage (collision)");
            collision.gameObject.GetComponent<Asteroid>()?.Explode();
            // TODO: вызвать метод у компонента здоровья
        }
    }
}
