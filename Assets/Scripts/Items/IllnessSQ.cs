using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IllnessSQ", menuName = "Game Data/Illnesses")]
public class IllnessSQ : ScriptableObject
{
    [SerializeField] private List<Illness> illnessList = new List<Illness>();
    
    // 通过属性公开列表，以便在其他脚本中访问
    public List<Illness> getIllnessList => illnessList;
    Illness A=new Illness("擦伤","症状：轻微流血",null,8,1,1);
    Illness B=new Illness("炎症","症状：腹痛",null,1,8,1);
    Illness C=new Illness("失眠","症状：精神疲惫",null,1,1,8);

    Illness D=new Illness("骨折","症状：剧烈疼痛",null,7,2,1);
    Illness E=new Illness("中毒","症状：呕吐腹泻",null,5,4,1);
    Illness F=new Illness("抑郁","症状：情绪低落",null,5,6,1);

    Illness G=new Illness("重伤","症状：生命垂危",null,7,4,1);
    Illness H=new Illness("感染","症状：高烧不退",null,5,2,5);
    Illness I=new Illness("久病","症状：虚弱无力",null,2,5,5);

    Illness J=new Illness("精神病","症状：幻觉妄想",null,6,5,2);
    Illness K=new Illness("多重伤害","症状：多处受伤",null,5,5,4);
    Illness L=new Illness("绝症","症状：无药可治",null,3,5,6);

    Illness M=new Illness("未知疾病","症状：症状复杂",null,6,3,5);
    Illness N=new Illness("慢性病","症状：长期折磨",null,6,3,7);
    Illness O=new Illness("急性病","症状：突发严重",null,3,6,5);
    Illness P=new Illness("传染病","症状：易传播",null,8,6,3);
    Illness Q=new Illness("职业病","症状：特定症状",null,6,5,7);
    
    void Awake()
    {
        // 在游戏开始时初始化疾病数据
        RefreshData(1); // 假设从第一天开始
    }

    public void RefreshData(int day)
    {
        if (day == 1)
        {
            if(illnessList.Count>0)
                illnessList.Clear();
            illnessList.Add(A);
            illnessList.Add(B);
        }
        else if (day == 2)
        {
            illnessList.Add(B);
            illnessList.Add(C);
            illnessList.Add(D);
        }
        else if (day == 3)
        {
            illnessList.Add(C);
            illnessList.Add(D);
            illnessList.Add(E);
            illnessList.Add(F);
        }
        else if (day == 4)
        {
            illnessList.Add(E);
            illnessList.Add(F);
            illnessList.Add(G);
            illnessList.Add(H);
            illnessList.Add(I);
        }
        else if (day == 5)
        {
            illnessList.Add(H);
            illnessList.Add(I);
            illnessList.Add(J);
            illnessList.Add(K);
            illnessList.Add(L);
            illnessList.Add(M);
        }
        else
        {
            illnessList.Add(K);
            illnessList.Add(L);
            illnessList.Add(M);
            illnessList.Add(N);
            illnessList.Add(O);
            illnessList.Add(P);
            illnessList.Add(Q);
        }
        // 此方法可用于在游戏开始时初始化或刷新疾病数据
        Debug.Log("IllnessSQ: 数据已刷新，当前疾病数量：" + illnessList.Count);
    }
}
