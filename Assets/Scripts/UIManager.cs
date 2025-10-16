
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI energyText;
    // [SerializeField] TextMeshProUGUI hpText;
    // [SerializeField] TextMeshProUGUI distanceText;
    StatsManager statsManager;


    void Start()
    {
        statsManager = FindAnyObjectByType<StatsManager>();
        statsManager.OnEnergyChanged += UpdateEnergyUI;
    }

    void OnDestroy()
    {
        if (statsManager != null)
            statsManager.OnEnergyChanged -= UpdateEnergyUI;
    }

    void UpdateEnergyUI()
    {
        energyText.text = statsManager.Energy.ToString() + "%";
    }
}
