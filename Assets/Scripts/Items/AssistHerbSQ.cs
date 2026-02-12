using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "HerbSQ", menuName = "Game Data/AssistHerbs")]
public class AssistHerbSQ : ScriptableObject
{
    [SerializeField] private List<AssistHerb> assistHerbList = new List<AssistHerb>();

    public List<AssistHerb> getAssistHerbList => assistHerbList;
    [SerializeField] private List<Sprite> sprites;
    AssistHerb A = new AssistHerb("人参","效果：大补元气",null,1,1,0);
    AssistHerb B = new AssistHerb("白芍","效果：缓解疼痛",null,0,1,1);
    AssistHerb C = new AssistHerb("茯苓","效果：利水渗湿",null,1,0,1);
    AssistHerb D = new AssistHerb("黄连","效果：清热解毒",null,2,0,0);
    AssistHerb E = new AssistHerb("板蓝根","效果：抗病毒",null,0,2,0);
    AssistHerb F = new AssistHerb("银翘散","效果：疏风解表",null,0,0,2);
    AssistHerb G = new AssistHerb("红色草药","和其他药一起吃可以增加药效",null,2,1,1);
    AssistHerb H = new AssistHerb("黄色草药","在西班牙的小村庄发现的草药",null,1,2,1);
    AssistHerb I = new AssistHerb("绿色草药","生长在阿克雷山区的草药",null ,2,2,2);
    AssistHerb J = new AssistHerb("蓝色草药","是强力的药剂，可以解毒",null ,1,3,0);
    AssistHerb K = new AssistHerb("大伟哥","听名字都觉得很厉害",null ,3,1,2);
    AssistHerb L = new AssistHerb("“L”","瓦塔西瓦L desu",null ,2,1,3);
    void Awake()
    {
            A.ChangeDetail();
            B.ChangeDetail();
            C.ChangeDetail();
            D.ChangeDetail();
            E.ChangeDetail();
            F.ChangeDetail();
            G.ChangeDetail();
            H.ChangeDetail();
            I.ChangeDetail();
            J.ChangeDetail();
            K.ChangeDetail();
            L.ChangeDetail();
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
            assistHerbList.Clear();
            A.ChangeSprite(sprites[0]);
            assistHerbList.Add(A);
        }
        else if (day == 4)
        {
            assistHerbList.Clear();
            B.ChangeSprite(sprites[1]);
            D.ChangeSprite(sprites[2]);
            assistHerbList.Add(D);
            assistHerbList.Add(B);
        }
        else if (day == 5)
        {
            assistHerbList.Clear();
            C.ChangeSprite(sprites[3]);
            assistHerbList.Add(C);
            assistHerbList.Add(G);
            assistHerbList.Add(H);
        }
        else if (day == 6)
        {
            assistHerbList.Clear();
            E.ChangeSprite(sprites[4]);
            F.ChangeSprite(sprites[5]);
            assistHerbList.Add(E);
            assistHerbList.Add(F);
            assistHerbList.Add(I);
            assistHerbList.Add(J);
            assistHerbList.Add(K);
            assistHerbList.Add(L);
        }
    }
}
