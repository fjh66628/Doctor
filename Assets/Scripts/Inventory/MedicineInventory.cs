
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
    [SerializeField]List<Button> buttons;//传入按钮
    [SerializeField]Image detailUI;
    [SerializeField]TextMeshProUGUI medicineName;
    [SerializeField]TextMeshProUGUI medicineDetail;
    [SerializeField]List<TextMeshProUGUI> textMeshProUGUIs;//传入按钮的文本
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
        for(int i = 0 ; i < 3 ; i++ )
        {
            buttons[i].image.sprite = inventoryManager.GetMedicine(i).getMedicineSprite;
            textMeshProUGUIs[i].text = inventoryManager.GetMedicine(i).getMedicineName;
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
        
        // 按X键使用选中的药物
        if(Input.GetKeyDown(KeyCode.X))
        {
            UseMedicineOnPatient();
        }
    }//当Z键按下后或X键按下后
    
    // 使用选中的药物给患者
    private void UseMedicineOnPatient()
    {
        if(selectedIndex == -1 || patient == null)
        {
            Debug.LogWarning("未选择药物或患者不存在");
            return;
        }
        
        Medicine selectedMedicine = inventoryManager.GetMedicine(selectedIndex);
        if(selectedMedicine != null)
        {
            patient.ApplyMedicine(selectedMedicine);
            Debug.Log($"给患者使用了药物: {selectedMedicine.getMedicineName}");
            
            // 使用后隐藏详情面板并取消选择
            detailUI.gameObject.SetActive(false);
            lastIndex = -1;
            selectedIndex = -1;
        }
    }
    public void OnClickButton(int i)
    {
        if(lastIndex != i)
        {
            detailUI.gameObject.SetActive(true);
            medicineName.text = inventoryManager.GetMedicine(i).getMedicineName;
            medicineDetail.text = inventoryManager.GetMedicine(i).getMedicineDetail;
            lastIndex = i;
            selectedIndex = i; //更新当前选中的药物
        }
        else
        {
            detailUI.gameObject.SetActive(false);
            if (inventoryManager.GetMedicine(i).getMedicineName != "空")
                EventManager.CallSeleckedMedicine(i);
            lastIndex = -1;
            selectedIndex = -1; //取消选择
        }

    }//点击，选中的逻辑

}
