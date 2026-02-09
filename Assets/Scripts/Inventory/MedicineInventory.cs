
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class MedicineInventory : MonoBehaviour
{

    public InventoryManager inventoryManager;
    [SerializeField]Button assistButton;
    [SerializeField]Button medicineButton;//传入按钮
    [SerializeField]Image detailUI;
    [SerializeField]TextMeshProUGUI medicineName;
    [SerializeField]TextMeshProUGUI medicineDetail;
    [SerializeField]TextMeshProUGUI assistHerbText;
    [SerializeField]TextMeshProUGUI medicineText;//传入按钮的文本
    public Patient patient;//对应的病人实例
    int lastIndex = -1;
    int selectedIndex = -1; //当前被选中的药物索引
    void OnEnable()
    {
        EventManager.MedicineInventoryUpdateEvent += UpdateUI;
    }
    void OnDisable()
    {
        EventManager.MedicineInventoryUpdateEvent -= UpdateUI;
    }

    void UpdateUI()
    {
        medicineButton.image.sprite = inventoryManager.GetMedicine().getMedicineSprite;
        medicineText.text = inventoryManager.GetMedicine().getMedicineName;
        if(inventoryManager.GetAssistHerb() != null)
        {
            assistButton.image.sprite = inventoryManager.GetAssistHerb().getAssistHerbSprite;
            assistHerbText.text = inventoryManager.GetAssistHerb().getAssistHerbName;
        }
    }//仓库更新的时候更新UI


    void Start()
    {
        detailUI.gameObject.SetActive(false);
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            detailUI.gameObject.SetActive(false);
            selectedIndex = -1; //取消选择
        }
    }//当Z键按下后或X键按下后
    

    // 使用选中的药物给患者
    public void UseMedicineOnPatient()
    { 
        if(selectedIndex == -1 || patient == null)
        {
            Debug.LogWarning("未选择药物或患者不存在");
            return;
        }
        
        Medicine selectedMedicine = inventoryManager.GetMedicine();
        if(selectedMedicine != null)
        {
            if(inventoryManager.GetAssistHerb() != null)
            {
                selectedMedicine.ChangeInternalWound(selectedMedicine.getInsideWound + inventoryManager.GetAssistHerb().getInternalWound);
                selectedMedicine.ChangeOutsideWound(selectedMedicine.getOutsideWound + inventoryManager.GetAssistHerb().getOutsideWound);
                selectedMedicine.ChangeMindWound(selectedMedicine.getMindWound + inventoryManager.GetAssistHerb().getMindWound);
            }
            patient.ApplyMedicine(selectedMedicine);
            Debug.Log($"给患者使用了药物: {selectedMedicine.getMedicineName}");
            inventoryManager.DeleteMedicine();
            inventoryManager.DeleteAssistHerb();
            // 使用后隐藏详情面板并取消选择
            detailUI.gameObject.SetActive(false);
            selectedIndex = -1;
        }
    }
    public void OnClickButton(int i)
    {
        if(lastIndex != i)
        {
            detailUI.gameObject.SetActive(true);
            medicineName.text = inventoryManager.GetMedicine().getMedicineName;
            medicineDetail.text = inventoryManager.GetMedicine().getMedicineDetail;
            lastIndex = i;
            selectedIndex = i; //更新当前选中的药物
        }
        else
        {
            detailUI.gameObject.SetActive(false);
            lastIndex = -1;
            selectedIndex = -1; //取消选择
        }

    }//点击，选中的逻辑
    
    public void ClearDesk()
    {
        inventoryManager.DeleteAssistHerb();
        inventoryManager.DeleteMedicine();
        UpdateUI();
    }

    public void CheckDetail()
    {
        if(inventoryManager.GetMedicine() != null)
        {
            detailUI.gameObject.SetActive(true);
            medicineName.text = "复方药剂";
            int mindCure = inventoryManager.GetAssistHerb() == null? 0:inventoryManager.GetAssistHerb().getMindWound + inventoryManager.GetMedicine().getMindWound;
            int outCure = inventoryManager.GetAssistHerb() == null? 0:inventoryManager.GetAssistHerb().getOutsideWound + inventoryManager.GetMedicine().getOutsideWound;
            int insideCure = inventoryManager.GetAssistHerb() == null? 0:inventoryManager.GetAssistHerb().getInternalWound + inventoryManager.GetMedicine().getInsideWound;
            medicineDetail.text = $"内伤-{insideCure}\n外伤-{outCure}\n精神伤-{mindCure}"; 
        }
    }
}
