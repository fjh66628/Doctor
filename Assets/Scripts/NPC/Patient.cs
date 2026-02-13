using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Patient : MonoBehaviour
{
    [Header("Text Display")]
    public TextMeshProUGUI AC_Text;
    public TextMeshProUGUI BC_Text;
    public TextMeshProUGUI SC_Text;
    public TextMeshProUGUI Name_Text;

    [Header("UI Image")]
    public Image patientImage; // 添加对UI Image的引用

    [Header("Illness Data")]
    [SerializeField] IllnessSQ illnessSQ;

    private Illness currentIllness;

    [Header("Wound Levels")]
    public float insidewoundnum = 0f;
    public float outsidewoundnum = 0f;
    public float spiritwoundnum = 0f;
    
    [Header("Animation Settings")]
    public float fadeDuration = 0.5f; // 淡入淡出持续时间
    
    int currentDay;
    int currentPatient;
    
    private Coroutine fadeCoroutine;
    private bool isFirstTime = true; // 标记是否是第一次调用
    private List<Graphic> uiElements = new List<Graphic>(); // 存储所有需要淡入淡出的UI元素

    void OnEnable()
    {
        EventManager.CureSuccessfullyEvent += GetSick;
        EventManager.UpdateDayEvent += RefreshData;
    }
    
    void OnDisable()
    {
        EventManager.CureSuccessfullyEvent -= GetSick;
        EventManager.UpdateDayEvent -= RefreshData;
    }
    
    private void Start()
    {
        // 收集所有需要淡入淡出的UI元素
        CollectUIElements();
        
        illnessSQ.getIllnessList.Clear();
        RefreshData();
        GetSick();
    }
    
    // 收集所有需要淡入淡出的UI元素
    private void CollectUIElements()
    {
        uiElements.Clear();
        
        // 添加Image组件
        if (patientImage == null)
        {
            patientImage = GetComponent<Image>();
        }
        
        if (patientImage != null)
        {
            uiElements.Add(patientImage);
        }
        
        // 添加所有TextMeshProUGUI组件
        TextMeshProUGUI[] allTexts = GetComponentsInChildren<TextMeshProUGUI>(true);
        foreach (var text in allTexts)
        {
            if (text != null)
            {
                uiElements.Add(text);
            }
        }
    }
    
    public void GetSick()
    {
        if (isFirstTime)
        {
            // 第一次直接设置，不执行淡入淡出
            isFirstTime = false;
            GetSickDirectly();
        }
        else
        {
            // 非第一次，执行淡入淡出效果
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
            fadeCoroutine = StartCoroutine(FadeAndChangeValues());
        }
    }
    
    IEnumerator FadeAndChangeValues()
    {
        if (uiElements.Count == 0)
        {
            CollectUIElements();
        }
        
        if (uiElements.Count == 0)
        {
            // 如果没有UI元素，直接获取疾病数据
            GetSickDirectly();
            yield break;
        }
        
        // 第一阶段：淡出（透明度从当前值到0）
        float timer = 0f;
        
        // 保存所有UI元素的初始颜色
        Color[] startColors = new Color[uiElements.Count];
        for (int i = 0; i < uiElements.Count; i++)
        {
            if (uiElements[i] != null)
            {
                startColors[i] = uiElements[i].color;
            }
        }
        
        // 淡出到完全透明
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;
            
            for (int i = 0; i < uiElements.Count; i++)
            {
                if (uiElements[i] != null)
                {
                    Color transparentColor = new Color(startColors[i].r, startColors[i].g, startColors[i].b, 0f);
                    uiElements[i].color = Color.Lerp(startColors[i], transparentColor, t);
                }
            }
            
            yield return null;
        }
        
        // 确保所有UI元素完全透明
        for (int i = 0; i < uiElements.Count; i++)
        {
            if (uiElements[i] != null)
            {
                Color transparentColor = new Color(uiElements[i].color.r, uiElements[i].color.g, uiElements[i].color.b, 0f);
                uiElements[i].color = transparentColor;
            }
        }
        
        // 第二阶段：透明度为0时更新数值
        UpdateIllnessData();
        
        // 短暂延迟，确保数值更新完成
        yield return null;
        
        // 第三阶段：淡入（透明度从0到1）
        timer = 0f;
        
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;
            
            for (int i = 0; i < uiElements.Count; i++)
            {
                if (uiElements[i] != null)
                {
                    Color fullColor = new Color(startColors[i].r, startColors[i].g, startColors[i].b, 1f);
                    Color currentTransparentColor = new Color(startColors[i].r, startColors[i].g, startColors[i].b, 0f);
                    uiElements[i].color = Color.Lerp(currentTransparentColor, fullColor, t);
                }
            }
            
            yield return null;
        }
        
        // 确保所有UI元素完全不透明
        for (int i = 0; i < uiElements.Count; i++)
        {
            if (uiElements[i] != null)
            {
                Color fullColor = new Color(startColors[i].r, startColors[i].g, startColors[i].b, 1f);
                uiElements[i].color = fullColor;
            }
        }
    }
    
    // 在透明度为0时更新疾病数据
    private void UpdateIllnessData()
    {
        currentDay = GameManager.Instance.GetCurrentDay();
        currentPatient = GameManager.Instance.GetGameStats().curedPatients;
        
        // 确保索引不越界
            currentIllness = illnessSQ.getIllnessList[currentPatient];
            insidewoundnum = currentIllness.getInsideWound;
            outsidewoundnum = currentIllness.getOutsideWound;
            spiritwoundnum = currentIllness.getSpiritWound;
            
            // 更新名称显示
            if (Name_Text != null)
                Name_Text.text = $"{currentIllness.getIllnessName}";

    }
    
    // 没有动画的直接获取疾病数据方法
    private void GetSickDirectly()
    {
        UpdateIllnessData();
    }
    
    private void Update()
    {
        // 更新显示文本，实时反映症状值
        if (AC_Text != null)
            AC_Text.text = $"内伤:{insidewoundnum}";
        if (BC_Text != null)
            BC_Text.text = $"外伤:{outsidewoundnum}";
        if (SC_Text != null)            
            SC_Text.text = $"精神:{spiritwoundnum}";
    }
    
    void RefreshData()
    {
        illnessSQ.RefreshData(GameManager.Instance.GetCurrentDay());
    }
    
public void ApplyMedicine(Medicine medicine)
{
    // 计算治疗后的值
    float afterInside = insidewoundnum - medicine.getInsideWound;
    float afterOutside = outsidewoundnum - medicine.getOutsideWound;
    float afterSpirit = spiritwoundnum - medicine.getMindWound;
    
    // 检查是否有任何值会变为负数（治疗过度）
    if (afterInside < 0f || afterOutside < 0f || afterSpirit < 0f)
    {
        EventManager.CallFailToCure();
        return;
    }
    
    // 检查是否所有值都大于0（治疗不足）
    if (afterInside > 0f && afterOutside > 0f && afterSpirit > 0f)
    {
        EventManager.CallNotCure();
        return;
    }
    
    // 检查是否所有值都等于0（刚好治愈）
    if (Mathf.Approximately(afterInside, 0f) && 
        Mathf.Approximately(afterOutside, 0f) && 
        Mathf.Approximately(afterSpirit, 0f))
    {
        EventManager.CallCureSuccess();
        return;
    }
    
    // 混合情况：部分值为0，部分大于0，且没有任何值小于0
    // 根据您的要求，这种情况调用CallFailToCure()
    EventManager.CallFailToCure();
}

}