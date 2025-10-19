using UnityEngine;

public class BlackHoleController : MonoBehaviour
{
    [SerializeField] GameObject blackHole;
    float minScale = 0.05f; // distance = 100 or more
    float maxScale = 1.0f; // distance = -100 or less
    StatsManager statsManager;
    ScrollingBackground scrollingBackground;
    BlackHoleWarpEffect blackHoleWarpEffect;
    // int  lastDistanceEffect = 1000;

    void Awake()
    {
        statsManager = GetComponent<StatsManager>();
        // scrollingBackground = FindFirstObjectByType<ScrollingBackground>();
        statsManager.OnDistanceIncreaseChanged += UpdateDistanceIncreaseBG;
        blackHoleWarpEffect = FindFirstObjectByType<BlackHoleWarpEffect>();
    }

    void Start()
    {
        scrollingBackground = FindFirstObjectByType<ScrollingBackground>();
        UpdateDistanceIncreaseBG();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ScaleBlackHole();
    }

    public float GetScale()
    {
        return blackHole.transform.localScale.x;
    }

    void ScaleBlackHole()
    {
        float distance = statsManager.Distance;
        float scale;
        // float distance = distanceDebug;
        if (distance >= 0)
        {
            scale = Mathf.Lerp(maxScale, minScale, distance / 100f);
            int newDistanceEffect = (int)(distance / 1);
            // if (newDistanceEffect != lastDistanceEffect)
            // {
            //     lastDistanceEffect = newDistanceEffect;
            blackHoleWarpEffect.UpdateDistance(distance);
            // }
        }
        else
        {
            float t = Mathf.InverseLerp(0f, -100f, distance); // 0 при 0, 1 при -100
            scale = Mathf.Lerp(1f, 2.2f, t);
            blackHoleWarpEffect.UpdateDistance(100f);
        }
        blackHole.transform.localScale = new Vector3(scale, 1, 1);
    }

    void UpdateDistanceIncreaseBG()
    {
        float val = statsManager.RealDistanceIncrease;
        Debug.Log("Updating background speed based on distance increase: " + val);
        if (scrollingBackground == null)
        {
            scrollingBackground = FindFirstObjectByType<ScrollingBackground>();
            if (scrollingBackground == null)
            {
                Debug.LogWarning("ScrollingBackground not found - can't update background speed yet.");
                return;
            }
        }

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
