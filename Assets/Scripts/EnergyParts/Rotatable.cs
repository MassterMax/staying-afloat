using UnityEngine;

public class Rotatable : MonoBehaviour
{
    [SerializeField] float startAngle = 90f;
    private bool canRotate = true;
    bool paused = false;
    public bool CanRotate
    {
        get { return canRotate; }
        set { canRotate = value; }
    }

    void Start()
    {
        GameStateManager.Instance.OnPauseStateChanged += HandlePauseStateChanged;
    }

    void HandlePauseStateChanged()
    {
        paused = GameStateManager.Instance.Paused;
    }

    void OnDisable()
    {
        GameStateManager.Instance.OnPauseStateChanged -= HandlePauseStateChanged;
    }

    void Update()
    {
        HandleRotation();
    }

    void HandleRotation()
    {
        if (canRotate && !paused)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = (Vector3)mousePos - transform.position;
            direction.z = 0;
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + startAngle;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
}
