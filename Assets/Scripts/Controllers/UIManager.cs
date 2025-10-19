
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    const string FLOAT_VALUE_FORMAT = "F1";
    const string HARD_FLOAT_VALUE_FORMAT = "F0";
    const string INCREASE_FORMAT = "/ч";
    [SerializeField] GameObject shipUIPanel;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject losePanel;
    [SerializeField] GameObject winPanel;
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

    // fly
    bool flyButtonActive = false;
    [SerializeField] Button flyButton;
    [SerializeField] TextMeshProUGUI flyText;
    [SerializeField] Sprite flyButtonPassiveSprite;
    [SerializeField] Sprite flyButtonActiveSprite;
    [SerializeField] Sprite flyButtonPressedSprite;

    // hp
    [SerializeField] List<GameObject> hpBar;

    StatsManager statsManager;
    EnergyPartsController energyPartsController;

    // pages
    [SerializeField] SimplePage simplePage;
    [SerializeField] ChoosePage choosePage;

    // upgrades 
    [SerializeField] Image regenUpgradeImage;
    [SerializeField] TextMeshProUGUI regenUpgradeText;
    [SerializeField] Image hookUpgradeImage;
    [SerializeField] TextMeshProUGUI hookUpgradeText;
    [SerializeField] Image gunUpgradeImage;
    [SerializeField] TextMeshProUGUI gunUpgradeText;
    [SerializeField] Image engineUpgradeImage;
    [SerializeField] TextMeshProUGUI engineUpgradeText;
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
        losePanel.SetActive(false);
        winPanel.SetActive(false);
        pausePanel.SetActive(false);

        energySlider.value = AllStatsContainer.Instance.DefaultSliderValue;
        flyButton.interactable = false;
        // ShowHP(3)
        // HideSimplePage();
        choosePage.gameObject.SetActive(false);
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
        float percent = Math.Min(100f, (statsManager.Energy * 100f / AllStatsContainer.Instance.EnergyToFly));
        flyText.text = percent.ToString(HARD_FLOAT_VALUE_FORMAT) + "%";
        bool flyButtonShouldBeActive = statsManager.Energy >= AllStatsContainer.Instance.EnergyToFly;

        if (flyButtonShouldBeActive != flyButtonActive)
        {
            flyButtonActive = flyButtonShouldBeActive;
            flyButton.interactable = flyButtonShouldBeActive;

            if (flyButtonActive)
                flyButton.image.sprite = flyButtonActiveSprite;
            else
                flyButton.image.sprite = flyButtonPassiveSprite;
        }
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
        // Debug.Log("Updating distance increase UI");
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

    public void ShowLosePanel()
    {
        Debug.Log("ShowLosePanel");
        losePanel.SetActive(true);
    }

    public void GoToMenu()
    {
        GameStateManager.Instance.LoadStart();
    }

    public void HideShipUIPanel()
    {
        shipUIPanel.SetActive(false);
    }

    public void FlyAwayButton()
    {
        GameStateManager.Instance.FlyAway(ShowWinScreen);
    }

    void ShowWinScreen()
    {
        winPanel.SetActive(true);
    }

    public void ShowHP(int value)
    {
        for (int i = 0; i < hpBar.Count; ++i)
        {
            if (i < value)
                hpBar[i].SetActive(true);
            else
                hpBar[i].SetActive(false);
        }
    }

    public void ShowSimplePage(string text, string nextButtonText = null)
    {
        simplePage.gameObject.SetActive(true);
        simplePage.SetText(text, nextButtonText);
    }

    public void ShowOptionPage(string text, string option1, string option2)
    {
        choosePage.gameObject.SetActive(true);
        choosePage.SetText(text, option1, option2);
    }

    public void HideFistOptionPage()
    {
        choosePage.gameObject.SetActive(false);
        GameStateManager.Instance.ResumeGameAfterOptionPage(true);
    }

    public void HideSecondOptionPage()
    {
        choosePage.gameObject.SetActive(false);
        GameStateManager.Instance.ResumeGameAfterOptionPage(false);
    }

    public void HideSimplePage()
    {
        simplePage.gameObject.SetActive(false);
        GameStateManager.Instance.ResumeGameAfterPage();
    }

    public void ShowUpgradeStats(int regenUpgrade, int hookUprgade, int gunUprgade, int engineUpgrade)
    {
        regenUpgradeImage.gameObject.SetActive(regenUpgrade > 0);
        regenUpgradeText.text = regenUpgrade.ToString();

        hookUpgradeImage.gameObject.SetActive(hookUprgade > 0);
        hookUpgradeText.text = hookUprgade.ToString();

        gunUpgradeImage.gameObject.SetActive(gunUprgade > 0);
        gunUpgradeText.text = gunUprgade.ToString();

        engineUpgradeImage.gameObject.SetActive(engineUpgrade > 0);
        engineUpgradeText.text = engineUpgrade.ToString();
    }
}
