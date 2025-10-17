using UnityEngine;

[RequireComponent(typeof(StatsManager))]
public class BlackHoleController : MonoBehaviour
{
    [SerializeField] GameObject blackHole;
    float minScale = 0.1f; // distance = 100 or more
    float maxScale = 1.0f; // distance = 0 or less
    StatsManager statsManager;
    ScrollingBackground scrollingBackground;

    void Awake()
    {
        statsManager = GetComponent<StatsManager>();
        scrollingBackground = FindFirstObjectByType<ScrollingBackground>();
        statsManager.OnDistanceIncreaseChanged += UpdateDistanceIncreaseBG;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ScaleBlackHole();
    }

    void ScaleBlackHole()
    {
        float distance = statsManager.Distance;
        // float distance = distanceDebug;
        float scale = Mathf.Lerp(maxScale, minScale, distance / 100f);
        blackHole.transform.localScale = new Vector3(scale, 1, 1);
    }

    void UpdateDistanceIncreaseBG()
    {
        float val = statsManager.RealDistanceIncrease;
        Debug.Log("Updating background speed based on distance increase: " + val);
        if (Mathf.Abs(val) < 0.1f)
            scrollingBackground.SetSpeed(0);
        else if (val < 0)
            scrollingBackground.SetSpeed(+0.005f);
        else
            scrollingBackground.SetSpeed(-0.005f);
    }

    void OnDestroy()
    {
        statsManager.OnDistanceIncreaseChanged -= UpdateDistanceIncreaseBG;
    }
}
