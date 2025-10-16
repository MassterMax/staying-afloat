using UnityEngine;
using System;

public class StatsManager : MonoBehaviour
{
    EnergyPartsController energyPartsController;
    private float maxEnergy = 100;
    private float energy = 100;
    private float energyIncrease = 0; // per second
    private float health = 100;
    private float distance = 100;
    public event Action OnEnergyChanged;
    public event Action OnEnergyIncreaseChanged;

    public float Energy => energy;
    public float EnergyIncrease => energyIncrease;
    public float Health => health;
    public float Distance => distance;
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
            // call systems shutdown
        }
        OnEnergyChanged?.Invoke();
    }

    // public void SubtractEnergy(float amount)
    // {
    //     energy -= amount;
    //     OnEnergyChanged?.Invoke();
    // }

    public void SetEnergyIncrease(float amount)
    {
        energyIncrease = amount;
        OnEnergyIncreaseChanged?.Invoke();
    }

    void Start()
    {
        OnEnergyChanged?.Invoke();
        OnEnergyIncreaseChanged?.Invoke();
        energyPartsController = FindFirstObjectByType<EnergyPartsController>();
    }

    void FixedUpdate()
    {
        if (energyIncrease != 0)
        {
            AddEnergy(energyIncrease * Time.fixedDeltaTime);
        }
    }
}
