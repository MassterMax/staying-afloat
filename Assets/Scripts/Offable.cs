using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Offable : MonoBehaviour
{
    Color originalColor;
    Color turnedOffColor = new(0.2f, 0.2f, 0.2f);
    bool turnedOn = true;
    public bool IsOn => turnedOn;
    SpriteRenderer spriteRenderer;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        Debug.Log("SpriteRenderer found: " + (spriteRenderer != null));
    }

    public virtual bool TryOff()
    {
        Debug.Log("SpriteRenderer in TryOff: " + (spriteRenderer != null));

        turnedOn = false;
        spriteRenderer.color = turnedOffColor;
        return true;
    }

    public virtual void On()
    {
        turnedOn = true;
        spriteRenderer.color = originalColor;
    }
}
