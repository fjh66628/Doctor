using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[System.Serializable]
public class InventoryManager : MonoBehaviour
{
    public GameManager gameManager;//传入gamemanager
    [Header("传入药草数据")]
    [SerializeField]private HerbSQ herbData;
    [Header("传入药物数据")]
    [SerializeField]private MedicineSQ medicineData;
    [SerializeField]private AssistHerbSQ assistHerbData;
    //还要获取当前天数来确定草药仓库中的药物数量
    int medicineIndex = 0;
    void OnEnable()
    {
        EventManager.AddHerbEvent += AddHerb;
        EventManager.AddMedicineEvent += AddMedicine;
    }
    void OnDisable()
    {
        EventManager.AddHerbEvent -= AddHerb;
        EventManager.AddMedicineEvent -= AddMedicine;
    }//仓库增加物品事件
    [Header("草药仓库数据")]
    [SerializeField]List<Herb> herbInventory = new List<Herb>();
    [Header("药品仓库数据")]
    [SerializeField]List<Medicine> medicineInventory = new List<Medicine>();//创建两个仓库列表
    [Header("副药仓库数据")]
    [SerializeField]List<AssistHerb> assistHerbInventory;
    void Start()
    {
        UpDateInventory();
    }
    public void UpDateInventory()
    {
        if(assistHerbInventory == null)
            Debug.LogWarning("未完成仓库更新逻辑");




        EventManager.CallMedicineInventoryUPdate();
        EventManager.CallHerbInventoryUpdate();//更新UI
    }
    public void ReloadInventory()
    {
        // 清空已有仓库数据
        herbInventory.Clear();
        assistHerbInventory.Clear();
        medicineInventory.Clear();
        
        // 重新加载草药数据
        int maxHerbNumb = herbData.getHerbList.Count - 1;
        for(int i = 0 ; i < 8 ; i ++)
        {
            if( i < maxHerbNumb)
                AddHerb(herbData.getHerbList[i+1]);
            else
                AddHerb(herbData.getHerbList[0]);
        }
        
        // 重新加载药品数据
        int maxMedicineNumb = 3;
        for(int i = 0 ; i < maxMedicineNumb ; i ++)
        {
            AddMedicine(medicineData.getMedicinesList[0]);
        }
        
        // 重新加载辅助草药数据
        int maxAssistHerbNum = 6;
        for(int i = 0 ; i < maxAssistHerbNum  ; i++)
        {
            if (assistHerbData.getAssistHerbList.Count - 1 > i)
                AddAssistHerb(assistHerbData.getAssistHerbList[i + 1]);
            else
                AddAssistHerb(assistHerbData.getAssistHerbList[0]);
        }
        
        // 触发UI更新事件
        EventManager.CallHerbInventoryUpdate();
        EventManager.CallMedicineInventoryUPdate();
        Debug.Log("[InventoryManager] 仓库数据已重新加载");
    }

    void AddHerb(Herb herb)
    {
        herbInventory.Add(herb);
    }
    void AddAssistHerb(AssistHerb herb)
    {
        assistHerbInventory.Add(herb);
    }
    void AddMedicine(Medicine medicine)
    {
        medicineInventory.Add(medicine);
    }
    
    public Medicine GetMedicine(int index)
    {
        return medicineInventory[index];
    }
    public Herb GetHerb(int index)
    {
        return herbInventory[index];
    }
    public AssistHerb GetAssistHerb(int index)
    {
        return assistHerbInventory[index];
    }
    public int GetHerbCount()
    {
        return herbInventory == null ? 0 : herbInventory.Count;
    }
    public int GetAssistHerbCount()
    {
        return assistHerbInventory == null ? 0 : assistHerbInventory.Count;
    }
    public void DeleteMedicine(Medicine medicine , int i)
    {
        medicineInventory[i] = medicine;
        EventManager.CallMedicineInventoryUPdate();
    }
    public void AddComebineMedicine(Medicine medicine)
    {
        for (int i = 0; i < medicineInventory.Count; i++)
        {
            if (medicineInventory[i].getMedicineName == "空")
            {
                medicineInventory [i] = medicine;
                return;
            }
        }
        medicineInventory[medicineIndex] = medicine;
        medicineIndex++;
    }
}
