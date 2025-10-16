using UnityEngine;

public class EnergyPartsController : MonoBehaviour
{
    Hook hook;
    Gun gun;
    void Start()
    {
        hook = FindFirstObjectByType<Hook>();
        gun = FindFirstObjectByType<Gun>();
    }

    public void ChangeHookState()
    {
        if (hook.IsOn)
            hook.TryOff();
        else
            hook.On();
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
    }

    public bool GetGunState()
    {
        return gun.IsOn;
    }
}
