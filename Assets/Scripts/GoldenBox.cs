using UnityEngine;
using System;

public class GoldenBox : Pullable
{
    public static event Action OnGoldenPickedUp;

    [Header("Motion")]
    [SerializeField] private float minSpeed = 1.5f;
    [SerializeField] private float maxSpeed = 2.5f;
    private float speed = 2f;
    private Vector3 target;
    private bool flying = false;

    [Header("Rotation")]
    private float rotationSpeedZ;

    public void Init(Vector3 from, Vector3 to, float? forcedSpeed = null)
    {
        transform.position = from;
        target = to;
        speed = forcedSpeed ?? UnityEngine.Random.Range(minSpeed, maxSpeed);
        rotationSpeedZ = UnityEngine.Random.Range(-180f, 180f);
        flying = true;
    }

    void Update()
    {
        if (flying)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

            transform.Rotate(Vector3.forward, rotationSpeedZ * Time.deltaTime);

            if (Vector3.Distance(transform.position, target) < 0.05f)
            {
                flying = false;
                // можно добавить "плюх" анимацию или начать медленное затухание
                Destroy(gameObject, 0.1f);
            }
        }
    }

    public override void Hook()
    {
        flying = false;
    }

    public override void TryPickup()
    {
        OnGoldenPickedUp?.Invoke();
        Destroy(gameObject);
    }
}
