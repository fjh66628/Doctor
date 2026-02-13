using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class DayCounter : MonoBehaviour
{
    [Header("Main UI")]
    [SerializeField] private Image clickableImage;
    [SerializeField] private Image closeButtonImage;
    [SerializeField] private TextMeshProUGUI dayText;

    [Header("Hidden UI - Images")]
    [SerializeField] private Image hiddenImage1; // 不可交互
    [SerializeField] private Image hiddenImage2; // 隐藏UI，恢复交互
    [SerializeField] private Image hiddenImage3; // 退出游戏

    [Header("Hidden UI - Texts")]
    [SerializeField] private TextMeshProUGUI hiddenText1; // 不可交互
    [SerializeField] private TextMeshProUGUI hiddenText2; // 隐藏UI，恢复交互
    [SerializeField] private TextMeshProUGUI hiddenText3; // 退出游戏

    [Header("Settings")]
    [SerializeField] private int maxDays = 6;
    [SerializeField] private bool resetOnEsc = false;
    [SerializeField] private bool enableDebugTest = false;

    [Header("Hover Effects")]
    [SerializeField] private float hoverBrightness = 1.3f;
    [SerializeField] private float normalDarkness = 0.8f;
    [SerializeField] private float colorChangeDuration = 0.2f;

    [Header("Enhanced Hover Effects")]
    [SerializeField] private float enhancedHoverBrightness = 1.6f;
    [SerializeField] private float enhancedNormalDarkness = 0.6f;
    [SerializeField] private float enhancedColorChangeDuration = 0.1f;

    private int currentDay = 1;
    private Button imageButton;
    private Button closeButton;
    private bool areHiddenUIsVisible = false;
    private Dictionary<Graphic, Color> originalColors = new Dictionary<Graphic, Color>();
    private Dictionary<Graphic, Button> uiButtons = new Dictionary<Graphic, Button>();

    void Start()
    {
        CheckAllUIRefrences();
        InitializeUI();
        UpdateDayText();

        // 【重要修复】确保隐藏UI在开始时是隐藏的
        StartCoroutine(InitializeHiddenUI());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && resetOnEsc)
        {
            ResetCounter();
        }
    }

    private IEnumerator InitializeHiddenUI()
    {
        yield return new WaitForEndOfFrame(); // 等待一帧确保所有组件初始化完成

        // 【重要修复】确保隐藏UI在开始时是隐藏的
        ForceHideAllHiddenUIs();
        areHiddenUIsVisible = false;
        VerifyHiddenUIState(false);

        if (enableDebugTest)
        {
            yield return StartCoroutine(RunDebugTest());
        }
    }

    private IEnumerator RunDebugTest()
    {
        yield return new WaitForSeconds(1f);
        SetHiddenUIsVisible(true);
        yield return new WaitForSeconds(2f);
        SetHiddenUIsVisible(false);
        yield return new WaitForSeconds(1f);
    }
    
    private void CheckAllUIRefrences()
    {
        // 空方法，用于占位
    }
    
    private void InitializeUI()
    {
        StoreOriginalColors();
        InitializeMainUI();
        InitializeHiddenUIComponents();
        SetAllUIsDark();
    }

    private void InitializeMainUI()
    {
        if (clickableImage != null)
        {
            imageButton = clickableImage.GetComponent<Button>();
            if (imageButton == null)
                imageButton = clickableImage.gameObject.AddComponent<Button>();

            imageButton.onClick.RemoveAllListeners();
            imageButton.onClick.AddListener(OnImageClicked);
            AddHoverEvents(clickableImage);
            uiButtons[clickableImage] = imageButton;
        }

        if (closeButtonImage != null)
        {
            closeButton = closeButtonImage.GetComponent<Button>();
            if (closeButton == null)
                closeButton = closeButtonImage.gameObject.AddComponent<Button>();

            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(OnCloseButtonClicked);
            AddHoverEvents(closeButtonImage);
            uiButtons[closeButtonImage] = closeButton;
        }
    }

    private void InitializeHiddenUIComponents()
    {
        // 【修复】不要在初始化时设置active状态，让SetHiddenUIsVisible统一管理
        InitializeHiddenImage(hiddenImage1, false, null);
        InitializeHiddenImage(hiddenImage2, true, OnHiddenImage2Clicked);
        InitializeHiddenImage(hiddenImage3, true, OnHiddenImage3Clicked);
        InitializeHiddenText(hiddenText1, false, null);
        InitializeHiddenText(hiddenText2, true, OnHiddenText2Clicked);
        InitializeHiddenText(hiddenText3, true, OnHiddenText3Clicked);
        AddHoverEffectsToInteractiveHiddenImages();
    }

    private void AddHoverEffectsToInteractiveHiddenImages()
    {
        if (hiddenImage2 != null) AddEnhancedHoverEventsToImage(hiddenImage2);
        if (hiddenImage3 != null) AddEnhancedHoverEventsToImage(hiddenImage3);
    }

    private void AddEnhancedHoverEventsToImage(Image image)
    {
        if (image == null) return;
        EventTrigger eventTrigger = image.GetComponent<EventTrigger>();
        if (eventTrigger == null) eventTrigger = image.gameObject.AddComponent<EventTrigger>();
        eventTrigger.triggers.Clear();

        EventTrigger.Entry enterEntry = new EventTrigger.Entry();
        enterEntry.eventID = EventTriggerType.PointerEnter;
        enterEntry.callback.AddListener((data) => { OnPointerEnterEnhancedImage(image); });
        eventTrigger.triggers.Add(enterEntry);

        EventTrigger.Entry exitEntry = new EventTrigger.Entry();
        exitEntry.eventID = EventTriggerType.PointerExit;
        exitEntry.callback.AddListener((data) => { OnPointerExitEnhancedImage(image); });
        eventTrigger.triggers.Add(exitEntry);
    }

    private void OnPointerEnterEnhancedImage(Image image)
    {
        if (image != null && originalColors.ContainsKey(image))
            SetEnhancedUIBright(image);
    }

    private void OnPointerExitEnhancedImage(Image image)
    {
        if (image != null && originalColors.ContainsKey(image))
            SetEnhancedUIDark(image);
    }

    private void SetEnhancedUIBright(Graphic uiElement)
    {
        if (uiElement != null && originalColors.ContainsKey(uiElement))
        {
            Color brightColor = GetModifiedColor(originalColors[uiElement], enhancedHoverBrightness);
            uiElement.CrossFadeColor(brightColor, enhancedColorChangeDuration, true, true);
        }
    }

    private void SetEnhancedUIDark(Graphic uiElement)
    {
        if (uiElement != null && originalColors.ContainsKey(uiElement))
        {
            Color darkColor = GetModifiedColor(originalColors[uiElement], enhancedNormalDarkness);
            uiElement.CrossFadeColor(darkColor, enhancedColorChangeDuration, true, true);
        }
    }

    private void InitializeHiddenImage(Image image, bool interactable, UnityEngine.Events.UnityAction clickAction)
    {
        if (image == null)
        {
            return;
        }

        Button button = image.GetComponent<Button>();
        if (button == null && interactable)
            button = image.gameObject.AddComponent<Button>();

        if (button != null)
        {
            button.interactable = interactable;
            button.onClick.RemoveAllListeners();
            if (clickAction != null)
                button.onClick.AddListener(clickAction);
            if (interactable)
                uiButtons[image] = button;
        }
    }

    private void InitializeHiddenText(TextMeshProUGUI text, bool interactable, UnityEngine.Events.UnityAction clickAction)
    {
        if (text == null)
        {
            return;
        }

        Button button = text.GetComponent<Button>();
        if (button == null && interactable)
            button = text.gameObject.AddComponent<Button>();

        if (button != null)
        {
            button.interactable = interactable;
            button.onClick.RemoveAllListeners();
            if (clickAction != null)
                button.onClick.AddListener(clickAction);
            if (interactable)
            {
                AddHoverEvents(text);
                uiButtons[text] = button;
            }
        }
    }

    private void AddHoverEvents(Graphic uiElement)
    {
        if (uiElement == null) return;
        EventTrigger eventTrigger = uiElement.GetComponent<EventTrigger>();
        if (eventTrigger == null)
            eventTrigger = uiElement.gameObject.AddComponent<EventTrigger>();
        eventTrigger.triggers.Clear();

        EventTrigger.Entry enterEntry = new EventTrigger.Entry();
        enterEntry.eventID = EventTriggerType.PointerEnter;
        enterEntry.callback.AddListener((data) => { OnPointerEnterUI(uiElement); });
        eventTrigger.triggers.Add(enterEntry);

        EventTrigger.Entry exitEntry = new EventTrigger.Entry();
        exitEntry.eventID = EventTriggerType.PointerExit;
        exitEntry.callback.AddListener((data) => { OnPointerExitUI(uiElement); });
        eventTrigger.triggers.Add(exitEntry);
    }

    private void OnPointerEnterUI(Graphic uiElement)
    {
        if (uiElement != null && originalColors.ContainsKey(uiElement) && IsUIInteractable(uiElement))
            SetUIBright(uiElement);
    }

    private void OnPointerExitUI(Graphic uiElement)
    {
        if (uiElement != null && originalColors.ContainsKey(uiElement))
            SetUIDark(uiElement);
    }

    private bool IsUIInteractable(Graphic uiElement)
    {
        return uiElement != null && uiButtons.ContainsKey(uiElement) && uiButtons[uiElement].interactable;
    }

    private void StoreOriginalColors()
    {
        StoreColor(clickableImage);
        StoreColor(closeButtonImage);
        StoreColor(dayText);
        StoreColor(hiddenImage1);
        StoreColor(hiddenImage2);
        StoreColor(hiddenImage3);
        StoreColor(hiddenText1);
        StoreColor(hiddenText2);
        StoreColor(hiddenText3);
    }

    private void StoreColor(Graphic uiElement)
    {
        if (uiElement != null)
            originalColors[uiElement] = uiElement.color;
    }

    private void SetUIBright(Graphic uiElement)
    {
        if (uiElement != null && originalColors.ContainsKey(uiElement))
        {
            Color brightColor = GetModifiedColor(originalColors[uiElement], hoverBrightness);
            uiElement.CrossFadeColor(brightColor, colorChangeDuration, true, true);
        }
    }

    private void SetUIDark(Graphic uiElement)
    {
        if (uiElement != null && originalColors.ContainsKey(uiElement))
        {
            Color darkColor = GetModifiedColor(originalColors[uiElement], normalDarkness);
            uiElement.CrossFadeColor(darkColor, colorChangeDuration, true, true);
        }
    }

    private void SetAllUIsDark()
    {
        foreach (var uiElement in originalColors.Keys)
        {
            if (uiElement != null)
                SetUIDark(uiElement);
        }
    }

    private Color GetModifiedColor(Color baseColor, float multiplier)
    {
        return new Color(
            Mathf.Clamp01(baseColor.r * multiplier),
            Mathf.Clamp01(baseColor.g * multiplier),
            Mathf.Clamp01(baseColor.b * multiplier),
            baseColor.a
        );
    }

    private void OnImageClicked()
    {
        if (currentDay >= maxDays || areHiddenUIsVisible) return;
        currentDay++;
        UpdateDayText();
        if (currentDay >= maxDays)
            DisableImageInteraction();
    }

    private void OnCloseButtonClicked()
    {
        if (!areHiddenUIsVisible)
        {
            // 显示隐藏UI
            SetHiddenUIsVisible(true);
            areHiddenUIsVisible = true;
            SetMainUIInteractable(false);
        }
        else
        {
            // 隐藏隐藏UI
            SetHiddenUIsVisible(false);
            areHiddenUIsVisible = false;
            SetMainUIInteractable(true);
        }
    }

    private void OnHiddenImage2Clicked()
    {
        HideHiddenUIsAndRestore();
    }

    private void OnHiddenImage3Clicked()
    {
        QuitGame();
    }

    private void OnHiddenText2Clicked()
    {
        HideHiddenUIsAndRestore();
    }

    private void OnHiddenText3Clicked()
    {
        QuitGame();
    }

    private void HideHiddenUIsAndRestore()
    {
        SetHiddenUIsVisible(false);
        areHiddenUIsVisible = false;
        SetMainUIInteractable(true);
    }

    // 修改为退出游戏
    private void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    // 【核心修复】重新实现SetHiddenUIsVisible方法
    private void SetHiddenUIsVisible(bool visible)
    {
        // 记录设置前的状态
        VerifyHiddenUIState(!visible);

        // 设置每个隐藏UI的可见性
        SetUIActive(hiddenImage1, visible, "hiddenImage1");
        SetUIActive(hiddenImage2, visible, "hiddenImage2");
        SetUIActive(hiddenImage3, visible, "hiddenImage3");
        SetUIActive(hiddenText1, visible, "hiddenText1");
        SetUIActive(hiddenText2, visible, "hiddenText2");
        SetUIActive(hiddenText3, visible, "hiddenText3");

        areHiddenUIsVisible = visible;

        // 验证设置结果
        VerifyHiddenUIState(visible);
    }

    // 【新增】强制隐藏所有隐藏UI的方法
    private void ForceHideAllHiddenUIs()
    {
        if (hiddenImage1 != null) hiddenImage1.gameObject.SetActive(false);
        if (hiddenImage2 != null) hiddenImage2.gameObject.SetActive(false);
        if (hiddenImage3 != null) hiddenImage3.gameObject.SetActive(false);
        if (hiddenText1 != null) hiddenText1.gameObject.SetActive(false);
        if (hiddenText2 != null) hiddenText2.gameObject.SetActive(false);
        if (hiddenText3 != null) hiddenText3.gameObject.SetActive(false);
    }

    private void SetUIActive(Component uiElement, bool active, string elementName)
    {
        if (uiElement != null && uiElement.gameObject != null)
        {
            bool wasActive = uiElement.gameObject.activeInHierarchy;
            uiElement.gameObject.SetActive(active);
            bool isActive = uiElement.gameObject.activeInHierarchy;
            // 如果设置失败，尝试强制设置
            if (isActive != active)
            {
                uiElement.gameObject.SetActive(active);
                isActive = uiElement.gameObject.activeInHierarchy;
            }
        }
    }

    // 【新增】验证隐藏UI状态的方法
    private void VerifyHiddenUIState(bool expectedVisible)
    {
        int activeCount = 0;
        int totalCount = 0;

        activeCount += CheckUIState(hiddenImage1, "hiddenImage1", expectedVisible);
        activeCount += CheckUIState(hiddenImage2, "hiddenImage2", expectedVisible);
        activeCount += CheckUIState(hiddenImage3, "hiddenImage3", expectedVisible);
        activeCount += CheckUIState(hiddenText1, "hiddenText1", expectedVisible);
        activeCount += CheckUIState(hiddenText2, "hiddenText2", expectedVisible);
        activeCount += CheckUIState(hiddenText3, "hiddenText3", expectedVisible);

        totalCount = 6; // 总共有6个隐藏UI

        bool isCorrect = (expectedVisible && activeCount == totalCount) ||
                        (!expectedVisible && activeCount == 0);
    }

    private int CheckUIState(Component uiElement, string elementName, bool expectedVisible)
    {
        if (uiElement != null && uiElement.gameObject != null)
        {
            bool isActive = uiElement.gameObject.activeInHierarchy;
            return isActive ? 1 : 0;
        }
        else
        {
            return 0;
        }
    }

    private void SetMainUIInteractable(bool interactable)
    {
        if (closeButton != null)
        {
            closeButton.interactable = interactable;
        }

        if (imageButton != null)
        {
            if (interactable && currentDay < maxDays)
            {
                imageButton.interactable = true;
            }
            else
            {
                imageButton.interactable = false;
            }
        }

        if (clickableImage != null)
        {
            float alpha = interactable ? 1f : 0.5f;
            Color color = clickableImage.color;
            color.a = alpha;
            clickableImage.color = color;
        }
        UpdateDayText();
    }

    private void DisableImageInteraction()
    {
        if (imageButton != null)
            imageButton.interactable = false;

        if (clickableImage != null)
        {
            Color color = clickableImage.color;
            color.a = 0.5f;
            clickableImage.color = color;
        }
    }

    private void UpdateDayText()
    {
        if (dayText != null)
        {
            dayText.text = "Day " + currentDay;
        }
    }

    public void ResetCounter()
    {
        currentDay = 1;
        UpdateDayText();
        SetHiddenUIsVisible(false);
        areHiddenUIsVisible = false;
        SetMainUIInteractable(true);
        SetAllUIsDark();
    }

    public void SetResetOnEsc(bool enable)
    {
        resetOnEsc = enable;
    }
}