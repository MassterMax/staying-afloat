using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EnergyPartsController : MonoBehaviour
{

    StatsManager statsManager;
    Hook hook;
    Gun gun;
    private float engineEnergyValue = 0f; // from 0 to 1
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
        // hook.ForceOff();
        // gun.TryOff();
        SetEnergyIncrease();
    }

    void Update()
    {
        // debug 
        if (Input.GetKeyDown(KeyCode.R))
        {
            statsManager.IncreaseEnergyDebug();
        }

        bool input = false;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            input = true;
            ChangeHookState();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            input = true;
            ChangeGunState();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            input = true;
            // todo try to press fly away
        }
        if (input)
        {
            statsManager.UpdateUIControls();
        }
    }

    void SetEnergyIncrease()
    {
        float totalIncrease = 0f;
        if (hook.IsOn)
            totalIncrease -= AllStatsContainer.Instance.HookConsumptionRate;
        if (gun.IsOn)
            totalIncrease -= AllStatsContainer.Instance.GunConsumptionRate;
        totalIncrease -= AllStatsContainer.Instance.GetShipEngineConsumptionRate(engineEnergyValue);

        totalIncrease += AllStatsContainer.Instance.SolarPanelRestoreRate;

        Debug.Log("Setting energy increase: " + totalIncrease);
        statsManager.SetEnergyIncrease(totalIncrease);
    }

    public void ChangeHookState()
    {
        if (hook.IsOn)
            // hook.TryOff();
            hook.ForceOff();
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
        // statsManager.
    }

    void SetDistanceIncrease()
    {
        statsManager.SetBaseDistanceIncrease(engineEnergyValue * AllStatsContainer.Instance.MaxEngineSpeed);
    }

    public void SetShipEngine(float value)
    {
        if (value <= 0.05f)  // Dead zone to turn off the engine
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
        SetDistanceIncrease();
    }
}
