using UnityEngine;
using System;

public class Box : Pullable
{
    public static event Action<int> OnPickedUp;

    [Header("Loot")]
    [SerializeField] private int energyAmount = 10;

    [Header("Motion")]
    [SerializeField] private float minSpeed = 1f;
    [SerializeField] private float maxSpeed = 4f;
    private float speed = 2f;
    private Vector3 target;
    private bool flying = false;

    [Header("Rotation")]
    private float rotationSpeedZ;

    public void Init(Vector3 from, Vector3 to, float? forcedSpeed = null, int? lootEnergy = null)
    {
        transform.position = from;
        target = to;
        speed = forcedSpeed ?? UnityEngine.Random.Range(minSpeed, maxSpeed);
        rotationSpeedZ = UnityEngine.Random.Range(-180f, 180f);
        if (lootEnergy.HasValue) energyAmount = lootEnergy.Value;
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
            }
        }
    }

    public override void Hook()
    {
        flying = false;
    }

    public override void TryPickup()
    {
        OnPickedUp?.Invoke(energyAmount);
        Destroy(gameObject);
    }
}
