
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI energyText;
    [SerializeField] TextMeshProUGUI energyIncreaseText;
    [SerializeField] TextMeshProUGUI distanceText;
    [SerializeField] TextMeshProUGUI distanceIncreaseText;

    [SerializeField] Button hookButton;
    TextMeshProUGUI hookButtonText;

    [SerializeField] Slider energySlider;

    [SerializeField] Button gunButton;
    TextMeshProUGUI gunButtonText;

    StatsManager statsManager;
    EnergyPartsController energyPartsController;

    float defaultSliderValue = 0.15f;

    void Awake()
    {
        statsManager = FindAnyObjectByType<StatsManager>();
        energyPartsController = FindAnyObjectByType<EnergyPartsController>();
        statsManager.OnEnergyChanged += UpdateEnergyUI;
        statsManager.OnEnergyIncreaseChanged += UpdateEnergyIncreaseUI;
        statsManager.OnDistanceChanged += UpdateDistanceUI;
        statsManager.OnDistanceIncreaseChanged += UpdateDistanceIncreaseUI;

        hookButton.onClick.AddListener(OnHookButtonClicked);
        hookButtonText = hookButton.GetComponentInChildren<TextMeshProUGUI>();
        UpdateHookText();

        gunButton.onClick.AddListener(OnGunButtonClicked);
        gunButtonText = gunButton.GetComponentInChildren<TextMeshProUGUI>();
        UpdateGunText();

        energySlider.onValueChanged.AddListener(OnSliderValueChanged);

        // UpdateDistanceUI();
        // UpdateDistanceIncreaseUI();
    }

    void Start()
    {
        energySlider.value = defaultSliderValue;
    }

    void OnDestroy()
    {
        if (statsManager != null)
        {
            statsManager.OnEnergyChanged -= UpdateEnergyUI;
            statsManager.OnEnergyIncreaseChanged -= UpdateEnergyIncreaseUI;
            statsManager.OnDistanceChanged -= UpdateDistanceUI;
            statsManager.OnDistanceIncreaseChanged -= UpdateDistanceIncreaseUI;
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

    void UpdateDistanceUI()
    {
        distanceText.text = statsManager.Distance.ToString("F1");
    }

    void UpdateDistanceIncreaseUI()
    {
        Debug.Log("Updating distance increase UI");
        string distanceIncreaseSymbol = statsManager.RealDistanceIncrease > 0 ? "+" : "";
        distanceIncreaseText.text = distanceIncreaseSymbol + statsManager.RealDistanceIncrease.ToString("F1") + "/s";
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
        Debug.Log("Slider value changed: " + value);
        energyPartsController.SetShipEngine(value);
        UpdateEnergyIncreaseUI();
    }

    public void ForceTurnOffControls()
    {
        UpdateHookText();
        UpdateGunText();
        energySlider.value = 0f;
    }
}
