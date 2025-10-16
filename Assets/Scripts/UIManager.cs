
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI energyText;
    [SerializeField] TextMeshProUGUI energyIncreaseText;
    [SerializeField] Button hookButton;
    TextMeshProUGUI hookButtonText;

    [SerializeField] Slider energySlider;

    [SerializeField] Button gunButton;
    TextMeshProUGUI gunButtonText;

    StatsManager statsManager;
    EnergyPartsController energyPartsController;

    void Awake()
    {
        statsManager = FindAnyObjectByType<StatsManager>();
        energyPartsController = FindAnyObjectByType<EnergyPartsController>();
        statsManager.OnEnergyChanged += UpdateEnergyUI;
        statsManager.OnEnergyIncreaseChanged += UpdateEnergyIncreaseUI;

        hookButton.onClick.AddListener(OnHookButtonClicked);
        hookButtonText = hookButton.GetComponentInChildren<TextMeshProUGUI>();
        UpdateHookText();

        gunButton.onClick.AddListener(OnGunButtonClicked);
        gunButtonText = gunButton.GetComponentInChildren<TextMeshProUGUI>();
        UpdateGunText();

        energySlider.onValueChanged.AddListener(OnSliderValueChanged);
        energySlider.value = 0.2f;
    }

    void OnDestroy()
    {
        if (statsManager != null)
        {
            statsManager.OnEnergyChanged -= UpdateEnergyUI;
            statsManager.OnEnergyIncreaseChanged -= UpdateEnergyIncreaseUI;
        }
    }

    void UpdateEnergyUI()
    {
        // округляем до десятых
        energyText.text = statsManager.Energy.ToString("F1");
    }

    void UpdateEnergyIncreaseUI()
    {
        string energyIncreaseSymbol = statsManager.EnergyIncrease > 0 ? "+" : "";
        energyIncreaseText.text = energyIncreaseSymbol + statsManager.EnergyIncrease.ToString("F1") + "/s";
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

    void OnSliderValueChanged(float value)
    {
        energyPartsController.SetShipEngine(value);
        UpdateEnergyIncreaseUI();
    }
}
