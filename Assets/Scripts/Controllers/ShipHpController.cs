using UnityEngine;

public class ShipHpController : MonoBehaviour
{
    StatsManager statsManager;
    void Awake()
    {
        statsManager = FindAnyObjectByType<StatsManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null) return;
        if (other.CompareTag("Asteroid"))
        {
            Debug.Log("Ship hit — apply damage (trigger)");
            other.GetComponent<Asteroid>()?.Explode();
            // TODO: вызвать метод у компонента здоровья, например:
            // GetComponent<Health>()?.ApplyDamage(damageAmount);
            statsManager.TryTakeDamage();
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
            statsManager.TryTakeDamage();
        }
    }
}
