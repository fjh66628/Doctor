using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AssistHerbInventory : MonoBehaviour
{
    [Header("引用")]
    public InventoryManager inventoryManager;
    
    [Header("UI组件")]
    [SerializeField] GameObject herbPrefab;
    [SerializeField] List<Button> assistHerbButtons;
    [SerializeField] List<Image> detailUIs;
    [SerializeField] List<TextMeshProUGUI> herbNames;
    [SerializeField] List<TextMeshProUGUI> herbDetails;
    [SerializeField] List<TextMeshProUGUI> assistHerbtextMeshProUGUIs;
    
    int assLastIndex = -1;
    
    void OnEnable()
    {
        EventManager.HerbInventoryUpdateEvent += UpdateUI;
    }
    
    void OnDisable()
    {
        EventManager.HerbInventoryUpdateEvent -= UpdateUI;
    }
    
    // 添加一个专门的方法来卸载所有按钮
    private void UnloadAllButtons()
    {
        // 首先移除所有按钮的事件监听器
        if (assistHerbButtons != null && assistHerbButtons.Count > 0)
        {
            foreach (var button in assistHerbButtons)
            {
                if (button != null)
                {
                    button.onClick.RemoveAllListeners();
                }
            }
        }
        
        // 销毁所有子对象
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            var child = transform.GetChild(i);
            if (child != null && child.gameObject != null)
            {
                if (Application.isPlaying)
                {
                    Destroy(child.gameObject);
                }
                else
                {
                    DestroyImmediate(child.gameObject);
                }
            }
        }
        
        // 清空列表
        if (assistHerbButtons != null) 
        {
            assistHerbButtons.Clear();
        }
        if (assistHerbtextMeshProUGUIs != null) 
        {
            assistHerbtextMeshProUGUIs.Clear();
        }
    }
    
    void UpdateUI()
    {
        // 先卸载所有现有的按钮
        UnloadAllButtons();
        
        // 检查引用
        if (inventoryManager == null)
        {
            Debug.LogError($"[AssistHerbInventory] InventoryManager is null");
            return;
        }
        
        if (herbPrefab == null)
        {
            Debug.LogError($"[AssistHerbInventory] herbPrefab is null");
            return;
        }
        
        // 获取辅助草药数量
        int assistHerbCount = inventoryManager.GetAssistHerbCount();
        
        // 如果没有辅助草药，直接返回
        if (assistHerbCount <= 0)
        {

            return;
        }
        
        // 生成辅助草药按钮
        for (int i = 0; i < assistHerbCount; i++)
        {
            var assistHerb = inventoryManager.GetAssistHerb(i);
            if (assistHerb == null)
            {
                Debug.LogWarning($"[AssistHerbInventory] AssistHerb at index {i} is null");
                continue;
            }
            
            GameObject newherb = Instantiate(herbPrefab, transform, false);
            if (newherb == null)
            {
                Debug.LogError($"[AssistHerbInventory] 实例化herbPrefab失败 (index {i})");
                continue;
            }
            
            newherb.SetActive(true);
            
            // 设置图片
            var img = newherb.GetComponentInChildren<Image>(true);
            if (img != null)
            {
                img.gameObject.SetActive(true);
                img.enabled = true;
                if (assistHerb.getAssistHerbSprite != null)
                {
                    img.sprite = assistHerb.getAssistHerbSprite;
                }
                else
                {
                    Debug.LogWarning($"[AssistHerbInventory] Sprite is null for assist herb {assistHerb.getAssistHerbName} (index {i})");
                }
            }
            else
            {
                Debug.LogWarning($"[AssistHerbInventory] 在按钮预制体中找不到Image组件 (index {i})");
            }
            
            // 设置文本
            var txt = newherb.GetComponentInChildren<TextMeshProUGUI>(true);
            if (txt != null)
            {
                txt.gameObject.SetActive(true);
                txt.enabled = true;
                txt.text = assistHerb.getAssistHerbName;
            }
            else
            {
                Debug.LogWarning($"[AssistHerbInventory] 在按钮预制体中找不到TextMeshProUGUI组件 (index {i})");
            }
            
            // 绑定点击事件
            var btn = newherb.GetComponentInChildren<Button>(true);
            if (btn != null)
            {
                btn.gameObject.SetActive(true);
                btn.enabled = true;
                btn.interactable = true;
                
                int idx = i;
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => OnClickAssistHerbButton(idx));
                
                if (assistHerbButtons != null) 
                {
                    assistHerbButtons.Add(btn);
                }
            }
            else
            {
                Debug.LogWarning($"[AssistHerbInventory] 在按钮预制体中找不到Button组件 (index {i})");
            }
            
            if (assistHerbtextMeshProUGUIs != null && txt != null) 
            {
                assistHerbtextMeshProUGUIs.Add(txt);
            }
            
            // 确保RectTransform正确
            var rt = newherb.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.localScale = Vector3.one;
                rt.anchoredPosition = Vector2.zero;
            }
        }
        
        Debug.Log($"[AssistHerbInventory] 更新完成，生成了 {assistHerbCount} 个按钮");
    }
    
    void Start()
    {
        // 初始时卸载所有可能存在的按钮
        UnloadAllButtons();
        
        if (detailUIs != null && detailUIs.Count > 0)
        {
            foreach (var d in detailUIs)
            {
                if (d != null && d.gameObject != null)
                    d.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogWarning("[AssistHerbInventory] detailUIs list is empty or not assigned in the inspector.");
        }
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            if (detailUIs != null && detailUIs.Count > 0)
            {
                foreach (var d in detailUIs)
                {
                    if (d != null && d.gameObject != null)
                        d.gameObject.SetActive(false);
                }
            }
        }
    }
    
    public void OnClickAssistHerbButton(int i)
    {
        
        if (inventoryManager == null)
        {
            Debug.LogWarning("InventoryManager is null");
            return;
        }
        
        // 检查按钮索引是否在有效范围内
        if (assistHerbButtons != null && (i < 0 || i >= assistHerbButtons.Count))
        {
            Debug.LogWarning($"辅助草药按钮索引 {i} 越界，按钮总数: {assistHerbButtons.Count}");
            return;
        }
        
        if (i < 0 || i >= inventoryManager.GetAssistHerbCount())
        {
            Debug.LogWarning($"辅助草药索引 {i} 越界，库存数量: {inventoryManager.GetAssistHerbCount()}");
            return;
        }
        
        var aHerb = inventoryManager.GetAssistHerb(i);
        if (aHerb == null)
        {
            Debug.LogWarning($"索引 {i} 处的辅助草药为null");
            return;
        }
        
        Image targetDetail = null;
        TextMeshProUGUI targetName = null;
        TextMeshProUGUI targetDetailText = null;
        
        if (detailUIs != null && detailUIs.Count > 0)
            targetDetail = detailUIs[0];
        if (herbNames != null && herbNames.Count > 0)
            targetName = herbNames[0];
        if (herbDetails != null && herbDetails.Count > 0)
            targetDetailText = herbDetails[0];
        
        if (assLastIndex != i)
        {
            if (targetDetail != null) 
            {
                targetDetail.gameObject.SetActive(true);
            }
            if (targetName != null) targetName.text = aHerb.getAssistHerbName;
            if (targetDetailText != null) targetDetailText.text = aHerb.getAssistHerbDetail;
            assLastIndex = i;
        }
        else
        {
            if (targetDetail != null) 
            {
                targetDetail.gameObject.SetActive(false);
            }
            if (aHerb.getAssistHerbName != "空")
                EventManager.CallSeleckedAssistHerb(inventoryManager.GetAssistHerb(i));
            assLastIndex = -1;
        }
        EventManager.CallMedicineInventoryUPdate();
    }
}