using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "HerbSQ", menuName = "Game Data/AssistHerbs")]
public class AssistHerbSQ : ScriptableObject
{
    [SerializeField] private List<AssistHerb> assistHerbList = new List<AssistHerb>();

    public List<AssistHerb> getAssistHerbList => assistHerbList;

    AssistHerb A = new AssistHerb("人参","效果：大补元气",null,1,1,0);
    AssistHerb B = new AssistHerb("白芍","效果：缓解疼痛",null,0,1,1);
    AssistHerb C = new AssistHerb("茯苓","效果：利水渗湿",null,1,0,1);
    AssistHerb D = new AssistHerb("黄连","效果：清热解毒",null,2,0,0);
    AssistHerb E = new AssistHerb("板蓝根","效果：抗病毒",null,0,2,0);
    AssistHerb F = new AssistHerb("银翘散","效果：疏风解表",null,0,0,2);

    void Awake()
    {
        // 在游戏开始时初始化辅助药草数据
        RefreshData(1); // 假设从第一天开始
    }
    public void RefreshData(int day)
    {
        if (day == 1)
        {
            if(assistHerbList.Count>0)
                assistHerbList.Clear();
        }
        else if (day == 3)
        {
            assistHerbList.Add(A);
        }
        else if (day == 4)
        {
            assistHerbList.Add(D);
            assistHerbList.Add(B);
        }
        else if (day == 5)
        {
            assistHerbList.Add(C);
        }
        else if (day == 6)
        {
            assistHerbList.Add(E);
            assistHerbList.Add(F);
        }
    }
}
