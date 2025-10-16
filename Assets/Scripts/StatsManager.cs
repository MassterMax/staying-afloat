using UnityEngine;
using System;

public class StatsManager : MonoBehaviour
{
    private float energy = 100;
    private float energyIncrease = 0;
    private float health = 100;
    private float distance = 100;
    public event Action OnEnergyChanged;

    public float Energy => energy;
    public float Health => health;
    public float Distance => distance;
    // public int PlayerScore => playerScore;
    // public int AntagonistScore => antagonistScore;
    public void AddEnergy(float amount)
    {
        energy += amount;
        OnEnergyChanged?.Invoke();
    }

    // public void SubtractEnergy(float amount)
    // {
    //     energy -= amount;
    //     OnEnergyChanged?.Invoke();
    // }

    public bool TryConsumeEnergy(float amount)
    {
        if (energy >= amount)
        {
            energy -= amount;
            OnEnergyChanged?.Invoke();
            return true;
        }
        return false;
    }

    public void ResetStats()
    {
        energy = 100;
        health = 100;
        distance = 100;
        OnEnergyChanged?.Invoke();
    }
}
