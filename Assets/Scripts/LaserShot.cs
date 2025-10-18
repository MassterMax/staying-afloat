using UnityEngine;

public class LaserShot : MonoBehaviour
{
    Rigidbody2D rb;
    private float lifetime;

    // call after Instantiate
    public void Init(Vector2 direction, float speed, float life)
    {
        lifetime = life;
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // set velocity — prefer kinematic body with velocity or dynamic as needed
            rb.linearVelocity = direction.normalized * speed;
        }
        else
        {
            // fallback: move by transform if no rigidbody
            StartCoroutine(MoveByTransform(direction.normalized * speed));
        }
        Destroy(gameObject, lifetime);
    }

    System.Collections.IEnumerator MoveByTransform(Vector2 vel)
    {
        float t = 0f;
        while (t < lifetime)
        {
            transform.position += (Vector3)(vel * Time.deltaTime);
            t += Time.deltaTime;
            yield return null;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Asteroid"))
        {
            Debug.Log("LaserShot hit meteor: " + other.gameObject.name);
            other.GetComponent<Asteroid>()?.Explode();
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("LaserShot hit non-Asteroid: " + other.gameObject.name);
            // optional: destroy bullet on hitting any obstacle
            // Destroy(gameObject);
        }
    }
}
