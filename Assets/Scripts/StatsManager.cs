using UnityEngine;
using System;

public class StatsManager : MonoBehaviour
{
    EnergyPartsController energyPartsController;
    UIManager uiManager;
    private float maxEnergy = 100;
    private float energy = 100;
    private float energyIncrease = 0; // per second
    private float health = 100;
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
        if (energy > maxEnergy)
            energy = maxEnergy;
        if (energy < 0)
        {
            energy = 0;
            Debug.LogWarning("Shutdown systems");
            energyPartsController.TurnOffAll();
            uiManager.ForceTurnOffControls();
        }
        OnEnergyChanged?.Invoke();
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
        energy = maxEnergy;
        distance = 100;
        health = 100;
    }

    void Start()
    {
        Reset();
        OnEnergyChanged?.Invoke();
        OnEnergyIncreaseChanged?.Invoke();
        OnDistanceChanged?.Invoke();
        energyPartsController = FindFirstObjectByType<EnergyPartsController>();
        uiManager = FindFirstObjectByType<UIManager>();
    }

    void CalculateDistance()
    {
        // TODO calculate with baseDistanceIncrease, distance and black hole effect
        // realDistanceIncrease = baseDistanceIncrease; // TODO add black hole effect
        if (realDistanceIncrease != baseDistanceIncrease)
        {
            realDistanceIncrease = baseDistanceIncrease;
            OnDistanceIncreaseChanged?.Invoke();
        }
        distance += baseDistanceIncrease * Time.fixedDeltaTime;
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
}
