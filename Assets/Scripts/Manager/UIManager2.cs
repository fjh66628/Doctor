using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class DayCounter : MonoBehaviour
{
    [Header("Main UI")]
    [SerializeField] private Image clickableImage;
    [SerializeField] private Image closeButtonImage;
    [SerializeField] private TextMeshProUGUI dayText;

    [Header("Hidden UI - Images")]
    [SerializeField] private Image hiddenImage1; // 隐藏图像1
    [SerializeField] private Image hiddenImage2; // 隐藏图像2
    [SerializeField] private Image hiddenImage3; // 隐藏图像3

    [Header("Hidden UI - Texts")]
    [SerializeField] private TextMeshProUGUI hiddenText1; // 隐藏文本1
    [SerializeField] private TextMeshProUGUI hiddenText2; // 隐藏文本2
    [SerializeField] private TextMeshProUGUI hiddenText3; // 隐藏文本3

    [Header("Settings")]
    [SerializeField] private string targetSceneName = "NextScene";
    [SerializeField] private int maxDays = 6;
    [SerializeField] private bool resetOnEsc = false;

    [Header("Hover Effects")]
    [SerializeField] private float hoverBrightness = 1.3f;
    [SerializeField] private float normalDarkness = 0.8f;
    [SerializeField] private float colorChangeDuration = 0.2f;

    private int currentDay = 1;
    private Button imageButton;
    private Button closeButton;
    private bool areHiddenUIsVisible = false;
    private Dictionary<Graphic, Color> originalColors = new Dictionary<Graphic, Color>();
    private Dictionary<Graphic, Button> uiButtons = new Dictionary<Graphic, Button>();

    void Start()
    {
        Debug.Log("DayCounter Start - 初始化");
        InitializeUI();
        UpdateDayText();
        SetHiddenUIsVisible(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && resetOnEsc)
        {
            Debug.Log("ESC按下，重置计数器");
            ResetCounter();
        }
    }

    private void InitializeUI()
    {
        StoreOriginalColors();
        InitializeMainUI();
        InitializeHiddenUI();
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

    private void InitializeHiddenUI()
    {
        InitializeHiddenImage(hiddenImage1, false, null);
        InitializeHiddenImage(hiddenImage2, true, OnHiddenImage2Clicked);
        InitializeHiddenImage(hiddenImage3, true, OnHiddenImage3Clicked);

        InitializeHiddenText(hiddenText1, false, null);
        InitializeHiddenText(hiddenText2, true, OnHiddenText2Clicked);
        InitializeHiddenText(hiddenText3, true, OnHiddenText3Clicked);
    }

    private void InitializeHiddenImage(Image image, bool interactable, UnityEngine.Events.UnityAction clickAction)
    {
        if (image == null) return;

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
            {
                AddHoverEvents(image);
                uiButtons[image] = button;
            }
        }
    }

    private void InitializeHiddenText(TextMeshProUGUI text, bool interactable, UnityEngine.Events.UnityAction clickAction)
    {
        if (text == null) return;

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
        {
            SetUIBright(uiElement);
        }
    }

    private void OnPointerExitUI(Graphic uiElement)
    {
        if (uiElement != null && originalColors.ContainsKey(uiElement))
        {
            SetUIDark(uiElement);
        }
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

        Debug.Log($"图片点击前 - 当前天数: {currentDay}");
        currentDay++;
        Debug.Log($"图片点击后 - 当前天数: {currentDay}");
        UpdateDayText();

        if (currentDay >= maxDays)
            DisableImageInteraction();
    }

    private void OnCloseButtonClicked()
    {
        Debug.Log($"=== 关闭按钮点击 ===");
        Debug.Log($"点击前 - 当前天数: {currentDay}");
        Debug.Log($"隐藏UI是否可见: {areHiddenUIsVisible}");

        // 记录当前数值，确保不会被修改
        int savedDay = currentDay;

        // 只禁止主UI的交互
        SetMainUIInteractable(false);

        // 显示隐藏UI
        SetHiddenUIsVisible(true);
        areHiddenUIsVisible = true;

        // 保证数值没有被修改
        if (currentDay != savedDay)
        {
            Debug.LogError($"检测到数值被意外修改: {savedDay} -> {currentDay}");
            currentDay = savedDay; // 恢复正确数值
            UpdateDayText();
        }

        Debug.Log($"点击后 - 当前天数: {currentDay}");
    }

    private void OnHiddenImage2Clicked()
    {
        HideHiddenUIsAndRestore();
    }

    private void OnHiddenImage3Clicked()
    {
        LoadTargetScene();
    }

    private void OnHiddenText2Clicked()
    {
        HideHiddenUIsAndRestore();
    }

    private void OnHiddenText3Clicked()
    {
        LoadTargetScene();
    }

    private void HideHiddenUIsAndRestore()
    {
        Debug.Log($"=== 恢复主UI ===");
        Debug.Log($"恢复前 - 当前天数: {currentDay}");

        // 隐藏隐藏UI
        SetHiddenUIsVisible(false);
        areHiddenUIsVisible = false;

        // 恢复主UI交互
        SetMainUIInteractable(true);

        Debug.Log($"恢复后 - 当前天数: {currentDay}");
    }

    private void LoadTargetScene()
    {
        if (!string.IsNullOrEmpty(targetSceneName))
            SceneManager.LoadScene(targetSceneName);
    }

    private void SetHiddenUIsVisible(bool visible)
    {
        Debug.Log($"设置隐藏UI可见性: {visible}");

        SetUIVisible(hiddenImage1, visible);
        SetUIVisible(hiddenImage2, visible);
        SetUIVisible(hiddenImage3, visible);
        SetUIVisible(hiddenText1, visible);
        SetUIVisible(hiddenText2, visible);
        SetUIVisible(hiddenText3, visible);
    }

    private void SetUIVisible(Behaviour uiElement, bool visible)
    {
        if (uiElement != null)
        {
            uiElement.gameObject.SetActive(visible);
        }
    }

    private void SetMainUIInteractable(bool interactable)
    {
        Debug.Log($"设置主UI交互状态: {interactable}");
        Debug.Log($"交互状态改变时 - 当前天数: {currentDay}");

        // ���ð�ť����״̬
        if (closeButton != null)
            closeButton.interactable = interactable;

        if (imageButton != null)
        {
            if (interactable && currentDay < maxDays)
            {
                imageButton.interactable = true;
                Debug.Log("启用图片按钮");
            }
            else if (!interactable)
            {
                imageButton.interactable = false;
                Debug.Log("禁用图片按钮");
            }
        }

        // ���ð�ť͸����
        float alpha = interactable ? 1f : 0.5f;

        if (clickableImage != null)
        {
            Color color = clickableImage.color;
            color.a = alpha;
            clickableImage.color = color;
            Debug.Log($"设置可点击图片透明度: {alpha}");
        }

        if (closeButtonImage != null)
        {
            Color color = closeButtonImage.color;
            color.a = alpha;
            closeButtonImage.color = color;
            Debug.Log($"设置关闭按钮透明度: {alpha}");
        }

        // dayText保持完全透明度，显示当前天数
        if (dayText != null)
        {
            // 确保透明度为1
            Color textColor = dayText.color;
            textColor.a = 1f;
            dayText.color = textColor;

            // 直接设置文本，确保显示正确数值
            dayText.text = "Day " + currentDay;
            Debug.Log($"设置dayText显示: Day {currentDay}");
        }

        Debug.Log($"交互状态改变完毕 - 当前天数: {currentDay}");
    }

    private void DisableImageInteraction()
    {
        if (imageButton != null)
            imageButton.interactable = false;

        Color disabledColor = GetModifiedColor(originalColors[clickableImage], normalDarkness);
        disabledColor.a = 0.5f;
        if (clickableImage != null)
            clickableImage.color = disabledColor;
    }

    private void UpdateDayText()
    {
        if (dayText != null)
        {
            dayText.text = "Day " + currentDay;
            Debug.Log($"更新显示文本: Day {currentDay}");
        }
    }

    public void ResetCounter()
    {
        Debug.Log("=== 重置计数器 ===");
        Debug.Log($"重置前天数: {currentDay}");

        currentDay = 1;
        UpdateDayText();
        SetHiddenUIsVisible(false);
        areHiddenUIsVisible = false;
        SetMainUIInteractable(true);
        SetAllUIsDark();

        Debug.Log($"重置后天数: {currentDay}");
    }

    public void SetResetOnEsc(bool enable)
    {
        resetOnEsc = enable;
        Debug.Log($"设置ESC功能: {enable}");
    }

    public int GetCurrentDay()
    {
        return currentDay;
    }//获取当前天数

}