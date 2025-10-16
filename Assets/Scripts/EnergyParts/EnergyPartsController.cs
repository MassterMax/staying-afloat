using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EnergyPartsController : MonoBehaviour
{
    private float maxEngineEnergyCost = 5f;
    private float engineEnergyCoef = 1.5f;
    private float engineEnergyValue = 0f;
    private float gunConsumptionRate = 10f; // Energy consumed per second when the gun is on
    private float hookConsumptionRate = 5f; // Energy consumed per second when the hook is on
    private float solarPanelRestoreRate = 2f; // Energy restored per second by the solar panel
    StatsManager statsManager;
    Hook hook;
    Gun gun;
    ShipEngine shipEngine;

    void Awake()
    {
        hook = FindFirstObjectByType<Hook>();
        gun = FindFirstObjectByType<Gun>();
        shipEngine = FindFirstObjectByType<ShipEngine>();
        statsManager = FindFirstObjectByType<StatsManager>();
    }
    void Start()
    {
        SetEnergyIncrease();
    }

    void SetEnergyIncrease()
    {
        float totalIncrease = 0f;
        if (hook.IsOn)
            totalIncrease -= hookConsumptionRate;
        if (gun.IsOn)
            totalIncrease -= gunConsumptionRate;
        totalIncrease -= GetShipEngineConsumptionRate();

        totalIncrease += solarPanelRestoreRate;

        Debug.Log("Setting energy increase: " + totalIncrease);
        statsManager.SetEnergyIncrease(totalIncrease);
    }

    public void ChangeHookState()
    {
        if (hook.IsOn)
            hook.TryOff();
        else
            hook.On();
        SetEnergyIncrease();
    }

    public bool GetHookState()
    {
        return hook.IsOn;
    }

    public void ChangeGunState()
    {
        if (gun.IsOn)
            gun.TryOff();
        else
            gun.On();
        SetEnergyIncrease();
    }

    public bool GetGunState()
    {
        return gun.IsOn;
    }

    public void TurnOffAll()
    {
        hook.ForceOff();
        gun.TryOff();
        SetShipEngine(0f);
        SetEnergyIncrease();
    }

    public float GetShipEngineConsumptionRate()
    {
        return Mathf.Pow(maxEngineEnergyCost * engineEnergyValue, engineEnergyCoef);
    }

    public void SetShipEngine(float value)
    {
        if (value <= 0.1f)
        {
            engineEnergyValue = 0f;
            shipEngine.TurnOff();
        }
        else
        {
            engineEnergyValue = value;
            shipEngine.TurnOn();
        }
        SetEnergyIncrease();
    }
}
