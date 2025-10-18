using UnityEngine;
using System;

public class StatsManager : MonoBehaviour
{
    private float currentHP;
    EnergyPartsController energyPartsController;
    UIManager uiManager;
    private float energy = 100;
    private float energyIncrease = 0; // per second
    // private float health = 100;
    private float distance = 100;
    bool lost = false;
    private float loseDistanceIncrease = 0f;
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
    TimeController timeController;
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
        // Debug.Log("Setting base distance increase: " + amount);
        baseDistanceIncrease = amount;
    }

    void Reset()
    {
        energy = AllStatsContainer.Instance.StartEnergy;
        distance = AllStatsContainer.Instance.StartDistance;
        currentHP = AllStatsContainer.Instance.MaxHp;
    }

    void Awake()
    {
        energyPartsController = FindFirstObjectByType<EnergyPartsController>();
        uiManager = FindFirstObjectByType<UIManager>();
        timeController = FindFirstObjectByType<TimeController>();
    }
    void Start()
    {
        Reset();
        OnEnergyChanged?.Invoke();
        OnEnergyIncreaseChanged?.Invoke();
        OnDistanceChanged?.Invoke();

        realDistanceIncrease = CalculateRealDistanceIncrease();
        OnDistanceIncreaseChanged?.Invoke();
    }

    void OnEnable()
    {
        Box.OnPickedUp += HandlePickup;
        GoldenBox.OnGoldenPickedUp += HandleGoldenPickup;
    }
    void OnDisable()
    {
        Box.OnPickedUp -= HandlePickup;
        GoldenBox.OnGoldenPickedUp -= HandleGoldenPickup;
    }

    private float CalculateRealDistanceIncrease()
    {
        return loseDistanceIncrease + baseDistanceIncrease + AllStatsContainer.Instance.GetBlackHoleSpeed(distance, timeController.GetLastReportedGameHours());
    }

    void CalculateDistance()
    {
        // TODO calculate with baseDistanceIncrease, distance and black hole effect
        // realDistanceIncrease = baseDistanceIncrease; // TODO add black hole effect
        float newRealDistanceIncrease = CalculateRealDistanceIncrease();
        if (realDistanceIncrease != newRealDistanceIncrease)
        {
            realDistanceIncrease = newRealDistanceIncrease;
            OnDistanceIncreaseChanged?.Invoke();
        }
        distance += realDistanceIncrease * Time.fixedDeltaTime;
        if (distance < 0 && !lost)
        {
            lost = true;
            distance = 0;
            // loseDistanceIncrease = -10f;
            Debug.LogWarning("You lose!!!!!!!!!!!!!");
            GameStateManager.Instance.LoseBlackHole();
        }
        // Debug.Log("Distance: " + distance);
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

    void HandleGoldenPickup()
    {
        Debug.Log("Picked up golden box!!!");
    }

    public void UpdateUIControls()
    {
        uiManager.UpdateControls();
    }

    public void TryTakeDamage()
    {
        currentHP -= 1;
        Debug.Log("Current HP: " + currentHP);
        if (currentHP == 0)
        {
            Debug.Log("Explode!");
        }
    }

    public void LoseAllEnergy()
    {
        AddEnergy(-10000f);
    }
}
