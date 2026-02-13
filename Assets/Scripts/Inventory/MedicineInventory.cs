
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    bool isClicked = false;
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
        if(inventoryManager.GetMedicine().getMedicineSprite != null)
            medicineButton.image.sprite = inventoryManager.GetMedicine().getMedicineSprite;
        medicineText.text = inventoryManager.GetMedicine().getMedicineName;
        if(inventoryManager.GetAssistHerb() != null)
        {
            assistButton.image.sprite = inventoryManager.GetAssistHerb().getAssistHerbSprite;
            assistHerbText.text = inventoryManager.GetAssistHerb().getAssistHerbName;
        }
        else
        {
            assistButton.image.sprite = null;
            assistHerbText.text = null;
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

        }
    }//当Z键按下后或X键按下后
    

    // 使用选中的药物给患者
    public void UseMedicineOnPatient()
    { 
        if(patient == null)
        {
            Debug.LogWarning("患者不存在");
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
            inventoryManager.DeleteMedicine();
            inventoryManager.DeleteAssistHerb();
            EventManager.CallMedicineInventoryUPdate();
        }
        detailUI.gameObject.SetActive(false);
        isClicked = false;
    }

    
    public void ClearDesk()
    {
        detailUI.gameObject.SetActive(false);
        isClicked = false;
        inventoryManager.DeleteAssistHerb();
        inventoryManager.DeleteMedicine();
        UpdateUI();
    }

    public void CheckDetail()
    {
        if(inventoryManager.GetMedicine() != null)
        {
            if(!isClicked)
            {   
                detailUI.gameObject.SetActive(true);
                medicineName.text = "复方药剂";
                int mindCure = (inventoryManager.GetAssistHerb() == null? 0:inventoryManager.GetAssistHerb().getMindWound) + inventoryManager.GetMedicine().getMindWound;
                int outCure = (inventoryManager.GetAssistHerb() == null? 0:inventoryManager.GetAssistHerb().getOutsideWound) + inventoryManager.GetMedicine().getOutsideWound;
                int insideCure = (inventoryManager.GetAssistHerb() == null? 0:inventoryManager.GetAssistHerb().getInternalWound) + inventoryManager.GetMedicine().getInsideWound;
                medicineDetail.text = $"内伤-{insideCure}\n外伤-{outCure}\n精神伤-{mindCure}";
                isClicked = !isClicked;
            }
            else
            {
                detailUI.gameObject.SetActive(false);
                isClicked = !isClicked;
            }
        }
    }
}
