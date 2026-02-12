using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{
    // 单例模式
    public static GameManager Instance { get; private set; }
    public TextMeshProUGUI text;
    [Header("游戏进度数据")]
    public int currentDay = 1;
    public int totalDays = 7;
    public int curedPatientsCount = 0;
    public int untreatedPatientsCount = 0;
    public int failedMedicationCount = 0;
    public UIManager uiManager;
    [Header("游戏状态")]
    public bool isFirstMedicationFailure = true;
    public bool isGameEnded = false;
    
    [Header("事件")]
    public System.Action OnGameEnd;
    
    // 结局类型

    public Scene settlementScene;
    [Header("结局场景名称")]
    public string goodEndingSceneName = "GoodEndingScene";
    public string badEndingSceneName = "BadEndingScene";
    public string neutralEndingSceneName = "NeutralEndingScene";
    public enum EndingType
    {
        GoodEnding,     // 治好多数病人
        BadEnding,      // 放弃治疗过多
        NeutralEnding   // 平局
    }

    void Start()
    {
        currentDay = 1;

        Tips();
    }
    private void Awake()
    {
        // 单例模式实现
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeGame();
        }
        else
        {
            Destroy(gameObject);
        }
        EventManager.CallUpdateDay();
    }
    private void InitializeGame()
    {
        currentDay = 1;
        curedPatientsCount = 0;
        untreatedPatientsCount = 0;
        failedMedicationCount = 0;
        isFirstMedicationFailure = true;
        isGameEnded = false;
    }

    private void OnEnable() 
    {
        EventManager.CureSuccessfullyEvent += OnPatientCuredHandler;
        EventManager.SkipThePatientEvent += OnPatientUntreatedHandler;
        EventManager.FailToCureEvent += OnMedicationFailedHandler;

    }
    private void OnDisable()
    {
        EventManager.CureSuccessfullyEvent -= OnPatientCuredHandler;
        EventManager.SkipThePatientEvent -= OnPatientUntreatedHandler;
        EventManager.FailToCureEvent -= OnMedicationFailedHandler;
    }
    // ========== 1. 外部访问天数信息的方法 ==========
    public int GetCurrentDay()
    {
        return currentDay;
    }

    public int GetRemainingDays()
    {
        return totalDays - currentDay;
    }

    public float GetGameProgress()
    {
        return (float)currentDay / totalDays;
    }

    // 进入下一天的方法
    public void GoToNextDay()
    {
        if (isGameEnded) return;
        
        currentDay++;
        EventManager.CallUpdateDay();

        uiManager.ShowTip($"进入第{currentDay}天");
        Tips();
        
        // 检查是否到达第7天
        if (currentDay >= totalDays)
        {
            CheckEnding();
        }
    }
    
    public void OnPatientCuredHandler()
    {
        curedPatientsCount++;
        if(curedPatientsCount == 2 || curedPatientsCount == 5 || curedPatientsCount == 9 || curedPatientsCount == 14 || curedPatientsCount == 20 || curedPatientsCount == 27)
        {
            GoToNextDay();
        }
    }
    void Tips()
    {
        switch(currentDay)
        {
            case 1:uiManager.ShowTip("进入第一天：双击药物加入合成台制药");return;
            case 2:uiManager.ShowTip("");return;
            case 4:uiManager.ShowTip("");return;
            case 5:uiManager.ShowTip("");return;
            case 6:uiManager.ShowTip("");return;
        }
    }
    // ========== 3. 放弃治疗事件监听 ==========
    public void OnPatientUntreatedHandler()
    {
        untreatedPatientsCount++;
    }

    // ========== 4. 给药失败事件监听 ==========
    public void OnMedicationFailedHandler()
    {
        failedMedicationCount++;
        
        // 如果是第一次失败，显示提示
        if (isFirstMedicationFailure)
        {
            ShowMedicationTip();
            isFirstMedicationFailure = false;
        }
        text.text = $"错误次数 {failedMedicationCount}";
    }

    private void ShowMedicationTip()
    {
        // 显示药品属性与病症匹配的提示
        string tipMessage = "注意：药品的属性需要与病人的病症相匹配！\n";
        // 这里可以调用UI管理器显示提示
        uiManager.ShowTip(tipMessage);
    }

    // ========== 5. 第7天结局判定 ==========
    private void CheckEnding()
    {
        if (isGameEnded) return;
        
        isGameEnded = true;
        EndingType ending = CalculateEnding();
        
        Debug.Log($"游戏结束！结局类型: {ending}");
        
        // 进入结算画面
        StartCoroutine(GoToSettlementScene(ending));
    }

    private EndingType CalculateEnding()
    {
        // 结局判定逻辑
        float cureRate = (float)curedPatientsCount / (curedPatientsCount + untreatedPatientsCount);
        
        if (cureRate >= 0.7f)
        {
            return EndingType.GoodEnding;
        }
        else if (cureRate <= 0.3f)
        {
            return EndingType.BadEnding;
        }
        else
        {
            return EndingType.NeutralEnding;
        }
    }

    private IEnumerator GoToSettlementScene(EndingType ending)
    {
        // 等待一帧确保所有操作完成
        yield return null;
        
        // 传递结局信息到结算场景
        PlayerPrefs.SetString("EndingType", ending.ToString());
        PlayerPrefs.SetInt("CuredPatients", curedPatientsCount);
        PlayerPrefs.SetInt("UntreatedPatients", untreatedPatientsCount);
        
        // 根据结局加载对应的场景
        string sceneToLoad = "Ending-normal";
        if (ending == EndingType.GoodEnding) sceneToLoad = "Ending-good";
        else if (ending == EndingType.BadEnding) sceneToLoad = "Ending-bad";

        SceneManager.LoadScene(sceneToLoad);
    }

    // 重新开始游戏
    public void RestartGame()
    {
        InitializeGame();
        // 重新加载游戏场景或其他初始化操作
        SceneManager.LoadScene("MainGameScene");
    }

    // 获取游戏统计信息（用于UI显示）
    public GameStats GetGameStats()
    {
        return new GameStats
        {
            currentDay = currentDay,
            curedPatients = curedPatientsCount,
            untreatedPatients = untreatedPatientsCount,
            totalDays = totalDays
        };
    }
}

// 游戏统计数据结构
public class GameStats
{
    public int currentDay;
    public int curedPatients;
    public int untreatedPatients;
    public int totalDays;
    
    public int GetTotalPatients()
    {
        return curedPatients + untreatedPatients;
    }
    
    public float GetCureRate()
    {
        int total = GetTotalPatients();
        return total > 0 ? (float)curedPatients / total : 0f;
    }
}