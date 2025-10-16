
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI energyText;
    [SerializeField] Button hookButton;
    TextMeshProUGUI hookButtonText;

    [SerializeField] Button gunButton;
    TextMeshProUGUI gunButtonText;

    StatsManager statsManager;
    EnergyPartsController energyPartsController;


    void Start()
    {
        statsManager = FindAnyObjectByType<StatsManager>();
        energyPartsController = FindAnyObjectByType<EnergyPartsController>();
        statsManager.OnEnergyChanged += UpdateEnergyUI;
        hookButton.onClick.AddListener(OnHookButtonClicked);
        hookButtonText = hookButton.GetComponentInChildren<TextMeshProUGUI>();
        UpdateHookText();

        gunButton.onClick.AddListener(OnGunButtonClicked);
        gunButtonText = gunButton.GetComponentInChildren<TextMeshProUGUI>();
        UpdateGunText();
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

    void OnHookButtonClicked()
    {
        energyPartsController.ChangeHookState();
        UpdateHookText();
    }

    private void UpdateHookText()
    {
        if (energyPartsController.GetHookState())
            hookButtonText.text = "Hook is On";
        else
            hookButtonText.text = "Hook is Off";
    }

    void OnGunButtonClicked()
    {
        energyPartsController.ChangeGunState();
        UpdateGunText();
    }

    private void UpdateGunText()
    {
        if (energyPartsController.GetGunState())
            gunButtonText.text = "Gun is On";
        else
            gunButtonText.text = "Gun is Off";
    }
}
