using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InGameUI : MonoBehaviour
{

    // 单例模式
    public static InGameUI Instance { get; private set; }

    // 各种UI
    public GameObject inventoryPanel;   // 仓库UI
    public GameObject deskPanel;        // 桌面UI
    public GameObject craftPanel;       // 合成台UI
    public GameObject numpanel;         // 数值UI

    // 仓库UI组件
    public Transform inventoryContent;
    public GameObject herbItemPrefab;

    //引用其他脚本

    public HerbSQ herbSQ;

    private void Awake()
    {
        // 设置单例
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 显示仓库面板
    public void ShowInventory()
    {
        inventoryPanel.SetActive(true);
        // 清空现有内容
        foreach (Transform child in inventoryContent)
        {
            Destroy(child.gameObject);
        }
        // 添加每个herb
        foreach (var herb in herbSQ.getHerbList)
        {
            GameObject item = Instantiate(herbItemPrefab, inventoryContent);
            // 设置文本和图像
            Text text = item.GetComponentInChildren<Text>();
            if (text != null)
            {
                text.text = herb.getHerbName;
            }
            /*
            Image img = item.GetComponentInChildren<Image>();
            if (img != null)
            {
                img.sprite = herb.getHerbSprite;
            }
            */
        }
    }

    public void showCraft()
    {
        craftPanel.SetActive(true);
    }
}
