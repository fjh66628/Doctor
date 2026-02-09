using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "HerbSQ", menuName = "Game Data/Herbs")]
public class HerbSQ : ScriptableObject
{
    [SerializeField] private List<Herb> herbList = new List<Herb>();
    
    // 通过属性公开列表，以便在其他脚本中访问
    public List<Herb> getHerbList => herbList;

    Herb a=new Herb("金银花","效果：缓解内伤",null,8,1,1);
    Herb b=new Herb("蒲公英","效果：缓解外伤",null,7,2,1);
    Herb c=new Herb("酸枣仁","效果：缓解精神伤害",null,7,1,2);
    Herb a1=new Herb("当归","效果：缓解内伤",null,1,8,1);
    Herb b1=new Herb("紫草","效果：缓解外伤",null,2,7,1);
    Herb c1=new Herb("远志","效果：缓解精神伤害",null,1,7,2);
    Herb a2=new Herb("黄芪","效果：增强免疫力",null,1,1,8);
    Herb b2=new Herb("川芎","效果：活血化瘀",null,1,2,7);
    Herb c2=new Herb("甘草","效果：调和诸药",null,2,1,7);
    Herb a3=new Herb("人参","效果：大补元气",null,5,5,0);
    Herb b3=new Herb("白芍","效果：缓解疼痛",null,5,0,5);
    Herb c3=new Herb("茯苓","效果：利水渗湿",null,0,5,5);

    void Awake()
    {
        // 在游戏开始时初始化药草数据
        RefreshData(1); // 假设从第一天开始
    }
    public void RefreshData(int day)
    {
        if (day == 1)
        {
            if(herbList.Count>0)
                herbList.Clear();

            herbList.Add(a);
            herbList.Add(a1);
            herbList.Add(a2);
        }
        else if (day == 2)
        {
            herbList.Add(b);
            herbList.Add(b1);
            herbList.Add(b2);
        }
        else if (day == 3)
        {
            herbList.Add(c);
            herbList.Add(c1);
        }
        else if (day == 4)
        {
            herbList.Add(c2);
        }
        else if (day == 5)
        {
            herbList.Add(a3);
            herbList.Add(b3);
            herbList.Add(c3);
        }
        // 此方法可用于在游戏开始时初始化或刷新药草数据
        Debug.Log("HerbSQ: 数据已刷新，当前药草数量：" + herbList.Count);
    }
}
