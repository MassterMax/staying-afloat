using UnityEngine;

public class AllStatsContainer : MonoBehaviour
{
    private float gunConsumptionRate = 1f; // Energy consumed per second when the gun is on
    private float hookConsumptionRate = 1f; // Energy consumed per second when the hook is on
    private float solarPanelRestoreRate = 2f; // Energy restored per second by the solar panel
    private float maxEnergy = 1000;
    private float startEnergy = 100;
    private float startDistance = 100;
    private float maxEngineEnergyCost = 4f;
    private float engineEnergyCoef = 1.5f;
    private float maxEngineSpeed = 5f; // maximum speed of the engine
    private float hookSpeed = 10f;
    float defaultSliderValue = 0.4f;
    private float hookEnergyCost = 5f; // energy cost per hook launch
    private float gunEnergyCost = 5f; // energy cost per gun shot
    private float gunShotDelay = 0.5f; // delay between gun shots
    private float hookExtraDistance = 1f; // extra distance added to hook target point
    private float gunShotSpeed = 15f;
    private float gunShotLifetime = 1.5f;

    // now getters
    public float GunConsumptionRate => gunConsumptionRate;
    public float HookConsumptionRate => hookConsumptionRate;
    public float SolarPanelRestoreRate => solarPanelRestoreRate;
    public float MaxEnergy => maxEnergy;
    public float MaxEngineSpeed => maxEngineSpeed;
    public float HookSpeed => hookSpeed;
    public float DefaultSliderValue => defaultSliderValue;
    public float HookEnergyCost => hookEnergyCost;
    public float GunEnergyCost => gunEnergyCost;
    public float GunShotDelay => gunShotDelay;
    public float HookExtraDistance => hookExtraDistance;
    public float GunShotSpeed => gunShotSpeed;
    public float GunShotLifetime => gunShotLifetime;
    public float StartEnergy => startEnergy;
    public float StartDistance => startDistance;

    public static AllStatsContainer Instance { get; private set; }

    void Awake()
    {
        // Проверяем, есть ли уже экземпляр
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // уничтожаем дубликат
            return;
        }

        Instance = this;
        // DontDestroyOnLoad(gameObject); // если нужно сохранять между сценами
    }

    public float GetBlackHoleSpeed(float distance, int elapsedHours)
    {
        float baseSpeed = -0.1f * (elapsedHours / 24); // increases by -0.1f every 24 hours
        // return -5f;
        if (distance >= 60f)
            return baseSpeed - 2f; // No effect
        if (distance >= 30f)
            return baseSpeed - 3f;
        if (distance >= 10f)
            return baseSpeed - 4.5f;
        return baseSpeed - 5.2f;
    }

    public float GetShipEngineConsumptionRate(float engineEnergyValue)
    {
        float res = Mathf.Pow(maxEngineEnergyCost * engineEnergyValue, engineEnergyCoef);
        // Debug.Log("Calculating ship engine consumption rate: " + res);
        return res;
    }

    // estimate is 12.5 energy
    public float GetBoxEnergy()
    {
        float p = Random.Range(0f, 1f);
        if (p < 0.1f) return 5f;
        if (p < 0.6f) return 10f;
        if (p < 0.95f) return 15f;
        return 30f;
    }
}
