using UnityEngine;

[RequireComponent(typeof(StatsManager))]
public class BlackHoleController : MonoBehaviour
{
    [SerializeField] GameObject blackHole;
    float minScale = 0.1f; // distance = 100 or more
    float maxScale = 1.0f; // distance = 0 or less
    StatsManager statsManager;
    void Start()
    {
        statsManager = GetComponent<StatsManager>();
    }

    // Update is called once per frame
    void Update()
    {
        ScaleBlackHole();
    }

    void ScaleBlackHole()
    {
        float distance = statsManager.Distance;
        float scale = Mathf.Lerp(maxScale, minScale, distance / 100f);
        blackHole.transform.localScale = new Vector3(scale, 1, 1);
    }
}
