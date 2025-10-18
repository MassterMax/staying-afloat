using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    private float speed = -0.01f;
    [SerializeField] private Renderer rend;
    void Update()
    {
        rend.material.mainTextureOffset += new Vector2(speed * Time.deltaTime, 0);
    }

    public void SetSpeed(float newSpeed)
    {
        Debug.Log("Setting background speed to: " + newSpeed);
        speed = newSpeed;
    }
}
