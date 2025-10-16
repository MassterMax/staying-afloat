using UnityEngine;

public class ShipIdleMovement : MonoBehaviour
{
    [SerializeField] float swayAmount = 0.5f; // насколько далеко корабль может шататься
    [SerializeField] float swaySpeed = 1f; // скорость колебаний
    [SerializeField] float rotationAmount = 5f; // угол поворота в градусах
    [SerializeField] float rotationSpeed = 1f; // скорость вращения

    private Vector3 startPosition;
    private Quaternion startRotation;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    void Update()
    {
        // Плавное шатание по X и Y
        float swayX = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
        float swayY = Mathf.Cos(Time.time * swaySpeed * 0.5f) * swayAmount * 0.5f;
        Vector3 offset = new Vector3(swayX, swayY, 0);

        transform.position = startPosition + offset;

        // Плавное вращение
        float rotationZ = Mathf.Sin(Time.time * rotationSpeed) * rotationAmount;
        transform.rotation = startRotation * Quaternion.Euler(0, 0, rotationZ);
    }
}
