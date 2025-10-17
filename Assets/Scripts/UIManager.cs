
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
    [SerializeField] Sprite hookOnSprite;
    [SerializeField] Sprite hookOffSprite;

    [SerializeField] Slider energySlider;

    [SerializeField] Button gunButton;
    [SerializeField] Sprite gunOnSprite;
    [SerializeField] Sprite gunOffSprite;

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
        UpdateHookImage();

        gunButton.onClick.AddListener(OnGunButtonClicked);
        UpdateGunImage();

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
        UpdateHookImage();
    }

    private void UpdateHookImage()
    {
        if (energyPartsController.GetHookState())
            hookButton.image.sprite = hookOnSprite;
        else
            hookButton.image.sprite = hookOffSprite;
    }

    void OnGunButtonClicked()
    {
        energyPartsController.ChangeGunState();
        UpdateGunImage();
    }

    private void UpdateGunImage()
    {
        if (energyPartsController.GetGunState())
            gunButton.image.sprite = gunOnSprite;
        else
            gunButton.image.sprite = gunOffSprite;
    }

    void OnSliderValueChanged(float value)
    {
        Debug.Log("Slider value changed: " + value);
        energyPartsController.SetShipEngine(value);
        UpdateEnergyIncreaseUI();
    }

    public void ForceTurnOffControls()
    {
        UpdateControls();
        energySlider.value = 0f;
    }

    public void UpdateControls()
    {
        UpdateHookImage();
        UpdateGunImage();
    }
}
