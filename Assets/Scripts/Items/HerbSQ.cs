using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HerbSQ", menuName = "Game Data/Herbs")]
public class HerbSQ : ScriptableObject
{
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private List<Herb> herbList = new List<Herb>();
    
    public List<Herb> getHerbList => herbList;
    
    // 移除了所有草药实例声明
    // Herb a = new Herb(...); // 删除这些
    
    // 修改RefreshData方法，使用预制数据
    public void RefreshData(int day)
    {
        // 注意：由于ScriptableObject是持久化的，我们需要重新创建列表
        // 避免在运行时修改原始数据
        
        // 临时列表，用于存储当前天数解锁的草药
        var tempList = new List<Herb>();
        
        if (day == 1)
        {
            // 第一天解锁的草药
            tempList.Add(CreateHerb("金银花", "效果：缓解内伤", sprites[0],8, 1, 1));
            tempList.Add(CreateHerb("当归", "效果：缓解内伤",sprites[1] ,1, 8, 1));
            tempList.Add(CreateHerb("黄芪", "效果：增强免疫力",sprites[2] ,1, 1, 8));
        }
        else if (day == 2)
        {
            // 第二天增加的草药
            tempList.Add(CreateHerb("蒲公英", "效果：缓解外伤", sprites[3],7, 2, 1));
            tempList.Add(CreateHerb("紫草", "效果：缓解外伤",sprites[4] ,2, 7, 1));
            tempList.Add(CreateHerb("川芎", "效果：活血化瘀",sprites[5] ,1, 2, 7));
        }
        else if (day == 3)
        {
            tempList.Add(CreateHerb("酸枣仁", "效果：缓解精神伤害",sprites[6] ,7, 1, 2));
            tempList.Add(CreateHerb("远志", "效果：缓解精神伤害", sprites[7],1, 7, 2));
        }
        else if (day == 4)
        {
            tempList.Add(CreateHerb("甘草", "效果：调和诸药",sprites[8] ,2, 1, 7));
        }
        else if (day == 5)
        {
            tempList.Add(CreateHerb("人参", "效果：大补元气", sprites[9],5, 5, 0));
            tempList.Add(CreateHerb("白芍", "效果：缓解疼痛", sprites[10],5, 0, 5));
            tempList.Add(CreateHerb("茯苓", "效果：利水渗湿", sprites[11],0, 5, 5));
        }
        
        // 将结果赋给herbList
        herbList.Clear();
        herbList.AddRange(tempList);
        
        Debug.Log("HerbSQ: 数据已刷新，当前药草数量：" + herbList.Count);
    }
    
    // 辅助方法：创建Herb实例
    private Herb CreateHerb(string name, string detail, Sprite sprite,int inside, int outside, int mental)
    {
        Herb herb =new Herb(name, detail, sprite, inside, outside, mental);
        herb.ChangeDetail();
        return herb;
    }
}