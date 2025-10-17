using UnityEngine;
using System;

public class StatsManager : MonoBehaviour
{
    EnergyPartsController energyPartsController;
    UIManager uiManager;
    private float energy = 100;
    private float energyIncrease = 0; // per second
    // private float health = 100;
    private float distance = 100;
    private float baseDistanceIncrease = 0; // per second
    private float realDistanceIncrease = 0; // per second, with black hole effect
    public event Action OnEnergyChanged;
    public event Action OnEnergyIncreaseChanged;
    public event Action OnDistanceChanged;
    public event Action OnDistanceIncreaseChanged;

    public float Energy => energy;
    public float EnergyIncrease => energyIncrease;
    public float Distance => distance;
    public float RealDistanceIncrease => realDistanceIncrease;
    // public float Health => health;
    // public int PlayerScore => playerScore;
    // public int AntagonistScore => antagonistScore;
    void AddEnergy(float amount)
    {
        energy += amount;
        if (energy > AllStatsContainer.Instance.MaxEnergy)
            energy = AllStatsContainer.Instance.MaxEnergy;
        if (energy < 0)
        {
            energy = 0;
            Debug.LogWarning("Shutdown systems");
            energyPartsController.TurnOffAll();
            uiManager.ForceTurnOffControls();
        }
        OnEnergyChanged?.Invoke();
    }

    public bool TryConsumeEnergy(float amount)
    {
        if (energy > amount)
        {
            AddEnergy(-amount);
            return true;
        }
        else
        {
            // not enough energy
            Debug.LogWarning("Not enough energy to consume: " + amount);
            return false;
        }
    }



    public void SetEnergyIncrease(float amount)
    {
        energyIncrease = amount;
        OnEnergyIncreaseChanged?.Invoke();
    }

    public void SetBaseDistanceIncrease(float amount)
    {
        Debug.Log("Setting base distance increase: " + amount);
        baseDistanceIncrease = amount;
    }

    void Reset()
    {
        energy = AllStatsContainer.Instance.MaxEnergy;
        distance = 100;
        // health = 100;
    }

    void Start()
    {
        Reset();
        OnEnergyChanged?.Invoke();
        OnEnergyIncreaseChanged?.Invoke();
        OnDistanceChanged?.Invoke();

        realDistanceIncrease = baseDistanceIncrease + AllStatsContainer.Instance.GetBlackHoleSpeed(distance);
        OnDistanceIncreaseChanged?.Invoke();

        energyPartsController = FindFirstObjectByType<EnergyPartsController>();
        uiManager = FindFirstObjectByType<UIManager>();
    }

    void OnEnable() => Box.OnPickedUp += HandlePickup;
    void OnDisable() => Box.OnPickedUp -= HandlePickup;

    void CalculateDistance()
    {
        // TODO calculate with baseDistanceIncrease, distance and black hole effect
        // realDistanceIncrease = baseDistanceIncrease; // TODO add black hole effect
        float newRealDistanceIncrease = baseDistanceIncrease + AllStatsContainer.Instance.GetBlackHoleSpeed(distance);
        if (realDistanceIncrease != newRealDistanceIncrease)
        {
            realDistanceIncrease = newRealDistanceIncrease;
            OnDistanceIncreaseChanged?.Invoke();
        }
        distance += realDistanceIncrease * Time.fixedDeltaTime;
        if (distance < 0)
        {
            distance = 0;
            Debug.LogWarning("You lose!!!!!!!!!!!!!");
            // handle lose
        }
        OnDistanceChanged?.Invoke();
    }

    void FixedUpdate()
    {
        if (energyIncrease != 0)
        {
            AddEnergy(energyIncrease * Time.fixedDeltaTime);
        }
        CalculateDistance();
    }

    void HandlePickup(float energy)
    {
        Debug.Log("Picked up energy: " + energy);
        AddEnergy(energy);
    }

    public void UpdateUIControls()
    {
        uiManager.UpdateControls();
    }
}
