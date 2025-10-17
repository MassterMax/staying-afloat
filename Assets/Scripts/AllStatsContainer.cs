using UnityEngine;

public class AllStatsContainer : MonoBehaviour
{
    private float gunConsumptionRate = 1f; // Energy consumed per second when the gun is on
    private float hookConsumptionRate = 1f; // Energy consumed per second when the hook is on
    private float solarPanelRestoreRate = 2f; // Energy restored per second by the solar panel
    private float maxEnergy = 100;

    private float maxEngineEnergyCost = 5f;
    private float engineEnergyCoef = 1.5f;
    private float maxEngineSpeed = 10f; // maximum speed of the engine
    private float hookSpeed = 10f;

    // now getters
    public float GunConsumptionRate => gunConsumptionRate;
    public float HookConsumptionRate => hookConsumptionRate;
    public float SolarPanelRestoreRate => solarPanelRestoreRate;
    public float MaxEnergy => maxEnergy;
    public float MaxEngineSpeed => maxEngineSpeed;
    public float HookSpeed => hookSpeed;

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
        DontDestroyOnLoad(gameObject); // если нужно сохранять между сценами
    }

    public float GetBlackHoleSpeed(float distance)
    {
        return -5f;
        // if (distance >= 100f)
        //     return 1f; // No effect
        // if (distance <= 66f)
        //     return 5f;
        // return blackHoleBaseSpeed / (distance * distance);
    }

    public float GetShipEngineConsumptionRate(float engineEnergyValue)
    {
        return Mathf.Pow(maxEngineEnergyCost * engineEnergyValue, engineEnergyCoef);
    }

}
