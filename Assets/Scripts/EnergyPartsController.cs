using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EnergyPartsController : MonoBehaviour
{
    private float gunConsumptionRate = 10f; // Energy consumed per second when the gun is on
    private float hookConsumptionRate = 5f; // Energy consumed per second when the hook is on
    private float solarPanelRestoreRate = 2f; // Energy restored per second by the solar panel
    StatsManager statsManager;
    Hook hook;
    Gun gun;

    void Awake()
    {
        hook = FindFirstObjectByType<Hook>();
        gun = FindFirstObjectByType<Gun>();
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
        SetEnergyIncrease();
    }
}
