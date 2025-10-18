
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    const string FLOAT_VALUE_FORMAT = "F1";
    const string INCREASE_FORMAT = "/ч";
    [SerializeField] GameObject pausePanel;
    [SerializeField] TextMeshProUGUI energyText;
    [SerializeField] TextMeshProUGUI energyIncreaseText;
    [SerializeField] TextMeshProUGUI distanceText;
    [SerializeField] TextMeshProUGUI distanceIncreaseText;
    // bool distanceIncreases = true;
    [SerializeField] TextMeshProUGUI dayAndTimeText;

    [SerializeField] Button hookButton;
    [SerializeField] Sprite hookOnSprite;
    [SerializeField] Sprite hookOffSprite;

    [SerializeField] Slider energySlider;

    [SerializeField] Button gunButton;
    [SerializeField] Sprite gunOnSprite;
    [SerializeField] Sprite gunOffSprite;

    StatsManager statsManager;
    EnergyPartsController energyPartsController;
    void Awake()
    {
        statsManager = FindAnyObjectByType<StatsManager>();
        energyPartsController = FindAnyObjectByType<EnergyPartsController>();
        statsManager.OnEnergyChanged += UpdateEnergyUI;
        statsManager.OnEnergyIncreaseChanged += UpdateEnergyIncreaseUI;
        statsManager.OnDistanceChanged += UpdateDistanceUI;
        statsManager.OnDistanceIncreaseChanged += UpdateDistanceIncreaseUI;

        hookButton.onClick.AddListener(OnHookButtonClicked);
        // UpdateHookImage();

        gunButton.onClick.AddListener(OnGunButtonClicked);
        // UpdateGunImage();

        energySlider.onValueChanged.AddListener(OnSliderValueChanged);

        // UpdateDistanceUI();
        // UpdateDistanceIncreaseUI();
    }

    void Start()
    {
        UpdateHookImage();
        UpdateGunImage();

        energySlider.value = AllStatsContainer.Instance.DefaultSliderValue;
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
        energyText.text = statsManager.Energy.ToString(FLOAT_VALUE_FORMAT);
    }

    void UpdateEnergyIncreaseUI()
    {
        string energyIncreaseSymbol = statsManager.EnergyIncrease > 0 ? "+" : "";
        energyIncreaseText.text = energyIncreaseSymbol + statsManager.EnergyIncrease.ToString(FLOAT_VALUE_FORMAT) + INCREASE_FORMAT;
    }

    void UpdateDistanceUI()
    {
        distanceText.text = statsManager.Distance.ToString(FLOAT_VALUE_FORMAT);
    }

    void UpdateDistanceIncreaseUI()
    {
        Debug.Log("Updating distance increase UI");
        string distanceIncreaseSymbol = statsManager.RealDistanceIncrease > 0 ? "+" : "";
        distanceIncreaseText.text = distanceIncreaseSymbol + statsManager.RealDistanceIncrease.ToString(FLOAT_VALUE_FORMAT) + INCREASE_FORMAT;
        if (Mathf.Approximately(statsManager.RealDistanceIncrease, 0f))
        {
            distanceIncreaseText.color = Color.white;
        }
        else if (statsManager.RealDistanceIncrease > 0)
        {
            distanceIncreaseText.color = Color.green;
        }
        else
        {
            distanceIncreaseText.color = Color.red;
        }
    }


    // totalGameHours: 1 real second == 1 game hour, totalGameHours can be large
    public void UpdateDayAndTime(int totalGameHours)
    {
        int day = Mathf.FloorToInt(totalGameHours / 24f) + 1;
        int hour = Mathf.FloorToInt(totalGameHours % 24f);
        if (dayAndTimeText != null)
            dayAndTimeText.text = $"День {day} — {hour:00}:00";
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
        // Debug.Log("Slider value changed: " + value);
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

    public void HandlePause(bool pause)
    {
        pausePanel.SetActive(pause);
    }

    public void GoToMenu()
    {
        GameStateManager.Instance.LoadStart();
    }
}
