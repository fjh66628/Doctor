using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "IllnessSQ", menuName = "Game Data/Illnesses")]
public class IllnessSQ : ScriptableObject
{
    [SerializeField] private List<Illness> illnessList = new List<Illness>();
    
    // 通过属性公开列表，以便在其他脚本中访问
    public List<Illness> getIllnessList => illnessList;
    
    // 定义所有疾病类型
    Illness A = new Illness("擦伤", "症状：轻微流血", null, 8, 1, 1);
    Illness B = new Illness("炎症", "症状：腹痛", null, 1, 8, 1);
    Illness C = new Illness("失眠", "症状：精神疲惫", null, 1, 1, 8);
    Illness D = new Illness("骨折", "症状：剧烈疼痛", null, 7, 2, 1);
    Illness E = new Illness("中毒", "症状：呕吐腹泻", null, 5, 4, 1);
    Illness F = new Illness("抑郁", "症状：情绪低落", null, 5, 6, 1);
    Illness G = new Illness("重伤", "症状：生命垂危", null, 7, 4, 1);
    Illness H = new Illness("感染", "症状：高烧不退", null, 5, 2, 5);
    Illness I = new Illness("久病", "症状：虚弱无力", null, 2, 5, 5);
    Illness J = new Illness("精神病", "症状：幻觉妄想", null, 6, 5, 2);
    Illness K = new Illness("多重伤害", "症状：多处受伤", null, 5, 5, 4);
    Illness L = new Illness("绝症", "症状：无药可治", null, 3, 5, 6);
    Illness M = new Illness("未知疾病", "症状：症状复杂", null, 6, 3, 5);
    Illness N = new Illness("慢性病", "症状：长期折磨", null, 6, 3, 7);
    Illness O = new Illness("急性病", "症状：突发严重", null, 3, 6, 5);
    Illness P = new Illness("传染病", "症状：易传播", null, 8, 6, 3);
    Illness Q = new Illness("职业病", "症状：特定症状", null, 6, 5, 7);
    
    // 打乱列表的方法
    private List<Illness> ShuffleList(List<Illness> list)
    {
        return list.OrderBy(x => Random.Range(0, 10000)).ToList();
    }
    
    // 根据天数获取当天可用的疾病类型
    private List<Illness> GetAvailableIllnessesForDay(int day)
    {
        if (day == 1)
        {
            return new List<Illness> { A, B };
        }
        else if (day == 2)
        {
            return new List<Illness> { C, D };
        }
        else if (day == 3)
        {
            return new List<Illness> { E, F };
        }
        else if (day == 4)
        {
            return new List<Illness> { G, H, I };
        }
        else if (day == 5)
        {
            return new List<Illness> {J, K, L, M };
        }
        else
        {
            return new List<Illness> { N, O, P, Q };
        }
    }
    
    public void RefreshData(int day)
    {
        // 获取当天可用的疾病类型
        List<Illness> availableIllnesses = GetAvailableIllnessesForDay(day);
        
        // 打乱顺序
        availableIllnesses = ShuffleList(availableIllnesses);
        
        // 清空当前列表
        illnessList.Clear();
        
        // 计算需要多少疾病：从GameManager获取当前治愈的病人数量
        int curedPatients = 0;
        if (GameManager.Instance != null && GameManager.Instance.GetGameStats() != null)
        {
            curedPatients = GameManager.Instance.GetGameStats().curedPatients;
        }
        
        // 生成一个足够长的列表，避免频繁扩展
        // 设定一个初始长度，比如100，足够应对大部分情况
        int initialListSize = 100;
        
        // 如果治愈的病人数接近初始长度，就增加一些余量
        int neededCount = Mathf.Max(curedPatients + 10, initialListSize);
        
        // 使用当天的疾病类型来填充列表
        for (int i = 0; i < neededCount; i++)
        {
            int index = i % availableIllnesses.Count;
            illnessList.Add(availableIllnesses[index]);
        }
        

    }
}