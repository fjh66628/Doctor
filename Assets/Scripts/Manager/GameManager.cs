using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
class Level
{
    List<Illness> illnesses;
    List<Herb> herbs;
    List<AssistHerb> assistHerbs;

} ;//关卡类
public class GameManager : MonoBehaviour
{

    [Header("基于天数的列表数据管理器")]
    public HerbSQ herbSQ;
    public AssistHerbSQ assistHerbSQ;
    public IllnessSQ illnessSQ;

    public int currentDay = -1; // 当前天数，初始值为 -1，表示游戏尚未开始

    // 引用 UI 中的 DayCounter（在 UIManager2.cs 中定义）以保持实时同步
    private DayCounter uiDayCounter;
    private InventoryManager inventoryManager;

    private int patients = 2; // 当天总病人数
    private int patientnow = 3;//当天剩余病人数
    // 公开方法，用于更新一天的总病人数量并初始化这一天的剩余病人数目
    public void PatientsToday()
    {
        patients++;
        patientnow = patients;
    }
    //公开方法，用于更新这一天剩余的病人数目
    public void PatientOver()
    {
        patientnow--;
        Debug.Log($"这一天剩下的病人数: {patients}");
    }
    //公开方法

    void Awake()
    {
        // 查找场景中的 DayCounter 实例
        uiDayCounter = FindObjectOfType<DayCounter>();
            inventoryManager = FindObjectOfType<InventoryManager>();
        if (uiDayCounter == null)
        {
            Debug.LogWarning("GameManager: 未找到 DayCounter（UIManager2）实例，currentDay 将不会自动同步。");
        }
        else
        {
            // 初始化同步值
            currentDay = uiDayCounter.GetCurrentDay();
            illnessSQ.RefreshData(currentDay);
            assistHerbSQ.RefreshData(currentDay);
            herbSQ.RefreshData(currentDay);
            Debug.Log($"GameManager: 已与 UI DayCounter 同步，currentDay={currentDay}");
        }

    }

    void Update()
    {
        // 每帧从 UI 的 DayCounter 读取最新值以保持实时同步
        if (uiDayCounter != null)
        {
            int uiDay = uiDayCounter.GetCurrentDay();
            if (currentDay != uiDay)
            {
                currentDay = uiDay;
                // 如需可在此触发其他与天数相关的逻辑
                illnessSQ.RefreshData(currentDay);
                assistHerbSQ.RefreshData(currentDay);
                herbSQ.RefreshData(currentDay);
                // 更新InventoryManager的数据
                if (inventoryManager != null)
                {
                    inventoryManager.ReloadInventory();
                }
                Debug.Log($"GameManager: currentDay 同步为 {currentDay}，已通知 InventoryManager 重新加载");
            }
        }
    }
}
