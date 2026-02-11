using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[System.Serializable]
public class InventoryManager : MonoBehaviour
{

    [Header("传入药草数据")]
    [SerializeField]private HerbSQ herbData;
    [Header("传入药物数据")]
    [SerializeField]private MedicineSQ medicineData;
    [SerializeField]private AssistHerbSQ assistHerbData;
    //还要获取当前天数来确定草药仓库中的药物数量
    void OnEnable()
    {
        EventManager.AddHerbEvent += AddHerb;
        EventManager.AddMedicineEvent += AddMedicine;
        EventManager.UpdateDayEvent += UpDateInventory;
    }
    void OnDisable()
    {
        EventManager.AddHerbEvent -= AddHerb;
        EventManager.AddMedicineEvent -= AddMedicine;
        EventManager.UpdateDayEvent -= UpDateInventory;
    }//仓库增加物品事件
    [Header("草药仓库数据")]
    [SerializeField]List<Herb> herbInventory = new List<Herb>();
    [Header("药品仓库数据")]
    [SerializeField]Medicine medicineInventory = new Medicine();//创建两个仓库列表
    [Header("副药仓库数据")]
    [SerializeField]List<AssistHerb> assistHerbInventory;
    [SerializeField]AssistHerb assistHerb;//桌子上的副药
    void Start()
    {
        herbData.getHerbList.Clear();
        assistHerbData.getAssistHerbList.Clear();
        UpDateInventory();
    }
    public void UpDateInventory()
    {
        herbData.RefreshData(GameManager.Instance.GetCurrentDay());
        assistHerbData.RefreshData(GameManager.Instance.GetCurrentDay());
        LoadInventory();

        EventManager.CallMedicineInventoryUPdate();
        EventManager.CallHerbInventoryUpdate();//更新UI
    }
    void LoadInventory()
    {
        
        // 检查数据源是否存在
        if (herbData != null && herbData.getHerbList != null)
        {
            // 从 HerbSQ 读取数据到 herbInventory
            foreach (var herb in herbData.getHerbList)
            {

                herbInventory.Add(herb);
            }
            

        }
        else
        {
            Debug.LogWarning("HerbSQ 数据源为空或未分配");
        }
        
        if (assistHerbData != null && assistHerbData.getAssistHerbList != null)
        {
            // 从 AssistHerbSQ 读取数据到 assistHerbInventory
            foreach (var assistHerb in assistHerbData.getAssistHerbList)
            {
                assistHerbInventory.Add(assistHerb);
            }

        }
        else
        {
            Debug.LogWarning("AssistHerbSQ 数据源为空或未分配");
        }
    }

    public void ReloadInventory()
    {
        // 清空已有仓库数据
        herbInventory.Clear();
        assistHerbInventory.Clear();
        
        // 重新加载草药数据
        int maxHerbNumb = herbData.getHerbList.Count - 1;
        for(int i = 0 ; i < 8 ; i ++)
        {
            if( i < maxHerbNumb)
                AddHerb(herbData.getHerbList[i+1]);
            else
                AddHerb(herbData.getHerbList[0]);
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
        medicineInventory = medicine;
    }
    
    public Medicine GetMedicine()
    {
        return medicineInventory;
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
    public void DeleteMedicine()
    {
        medicineInventory = medicineData.getMedicinesList[0];
        EventManager.CallMedicineInventoryUPdate();
    }
    public void AddComebineMedicine(Medicine medicine)
    {
        
        medicineInventory = medicine;

    }
    public AssistHerb GetAssistHerb()
    {
        return assistHerb;
    }//外部访问桌子上的副药
    public void DeleteAssistHerb()
    {
        assistHerb = null;
    }//删除桌子上的副药
}
