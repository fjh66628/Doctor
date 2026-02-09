using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Patient : MonoBehaviour
{
    [Header("text Display")]
    public TextMeshProUGUI AC_Text;
    public TextMeshProUGUI BC_Text;
    public TextMeshProUGUI SC_Text;

    public TextMeshProUGUI Name_Text;

    [Header("Illness Data")]
    [SerializeField] IllnessSQ illnessSQ;

    private Illness currentIllness;

    [Header("wound Levels")]
    public float insidewoundnum = 0f;
    public float outsidewoundnum = 0f;
    public float spiritwoundnum = 0f;

    private void Start()
    {
        // 从 IllnessSQ 中随机获取一个疾病
        if (illnessSQ != null && illnessSQ.getIllnessList.Count > 0)
        {
            int randomIndex = Random.Range(0, illnessSQ.getIllnessList.Count);
            currentIllness = illnessSQ.getIllnessList[randomIndex];
            
            // 将疾病的症状值赋给患者
            insidewoundnum = currentIllness.getInsideWound;
            outsidewoundnum = currentIllness.getOutsideWound;
            spiritwoundnum = currentIllness.getSpiritWound;
            
            Debug.Log($"患者患上了: {currentIllness.getIllnessName}");
        }
        else
        {
            Debug.LogWarning("illnessSQ 未设置或无疾病数据");
        }
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
        if (Name_Text != null && currentIllness != null)
            Name_Text.text = $"{currentIllness.getIllnessName}";
    }

    // 应用药剂，当某个数值被减到小于零时拒绝给药
    public void ApplyMedicine(Medicine medicine)
    {
        // 检查是否会有任何数值变为负数，如果会就拒绝该次给药
        if (insidewoundnum - medicine.getInsideWound < 0f ||
            outsidewoundnum - medicine.getOutsideWound < 0f ||
            spiritwoundnum - medicine.getMindWound < 0f)
        {
            Debug.LogWarning($"药剂 {medicine.getMedicineName} 超过患者所需，拒绝使用");
            return;
        }

        ReduceInsideWound(medicine.getInsideWound);
        ReduceOutsideWound(medicine.getOutsideWound);
        ReduceSpriteWound(medicine.getMindWound);

        Debug.Log($"应用了药剂: {medicine.getMedicineName}");
    }

    // 各项减弱接口，值不能低于 0
    public void ReduceInsideWound(float amount)
    {
        insidewoundnum = Mathf.Max(0f, insidewoundnum - amount);
    }

    public void ReduceOutsideWound(float amount)
    {
        outsidewoundnum = Mathf.Max(0f, outsidewoundnum - amount);
    }

    public void ReduceSpriteWound(float amount)
    {
        spiritwoundnum = Mathf.Max(0f, spiritwoundnum - amount);
    }

    // 判断是否已痊愈（所有症状为0）
    public bool IsHealed()
    {
        return insidewoundnum <= 0f && outsidewoundnum <= 0f && spiritwoundnum <= 0f;
    }

    // 获取当前患者的疾病
    public Illness GetCurrentIllness()
    {
        return currentIllness;
    }
}
