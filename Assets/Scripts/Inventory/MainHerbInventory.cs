using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MainHerbInventory : MonoBehaviour
{
    [Header("引用")]
    public InventoryManager inventoryManager;
    
    [Header("UI组件")]
    [SerializeField] GameObject herbPrefab;
    [SerializeField] List<Button> buttons;
    [SerializeField] List<Image> detailUIs;
    [SerializeField] List<TextMeshProUGUI> herbNames;
    [SerializeField] List<TextMeshProUGUI> herbDetails;
    [SerializeField] List<TextMeshProUGUI> textMeshProUGUIs;
    
    int lastIndex = -1;
    
    void OnEnable()
    {
        EventManager.HerbInventoryUpdateEvent += UpdateUI;
    }
    
    void OnDisable()
    {
        EventManager.HerbInventoryUpdateEvent -= UpdateUI;
    }
    
    void UpdateUI()
    {
        // 销毁已有子对象
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            var child = transform.GetChild(i);
            if (Application.isPlaying)
                Destroy(child.gameObject);
            else
                DestroyImmediate(child.gameObject);
        }
        
        // 清理列表
        if (buttons != null) buttons.Clear();
        if (textMeshProUGUIs != null) textMeshProUGUIs.Clear();
        
        // 检查引用
        if (inventoryManager == null)
        {
            Debug.LogError($"[MainHerbInventory] InventoryManager is null");
            return;
        }
        
        if (herbPrefab == null)
        {
            Debug.LogError($"[MainHerbInventory] herbPrefab is null");
            return;
        }
        
        // 获取主草药数量
        int herbCount = inventoryManager.GetHerbCount();

        
        // 生成主草药按钮
        for (int i = 0; i < herbCount; i++)
        {
            var herb = inventoryManager.GetHerb(i);
            if (herb == null)
            {
                Debug.LogWarning($"[MainHerbInventory] Herb at index {i} is null");
                continue;
            }
            
            GameObject newherb = Instantiate(herbPrefab, transform, false);
            newherb.SetActive(true);
            
            // 设置图片
            var img = newherb.GetComponentInChildren<Image>(true);
            if (img != null)
            {
                img.gameObject.SetActive(true);
                img.enabled = true;
                if (herb.getHerbSprite != null)
                {
                    img.sprite = herb.getHerbSprite;
                }
                else
                {
                    Debug.LogWarning($"[MainHerbInventory] Sprite is null for herb {herb.getHerbName} (index {i})");
                }
            }
            
            // 设置文本
            var txt = newherb.GetComponentInChildren<TextMeshProUGUI>(true);
            if (txt != null)
            {
                txt.gameObject.SetActive(true);
                txt.enabled = true;
                txt.text = herb.getHerbName;
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
                btn.onClick.AddListener(() => OnClickButton(idx));
                if (buttons != null) buttons.Add(btn);
            }
            
            if (textMeshProUGUIs != null && txt != null) 
                textMeshProUGUIs.Add(txt);
            
            // 确保RectTransform正确
            var rt = newherb.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.localScale = Vector3.one;
                rt.anchoredPosition = Vector2.zero;
            }
        }
    }
    
    void Start()
    {
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
            Debug.LogWarning("[MainHerbInventory] detailUIs list is empty or not assigned in the inspector.");
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
    
    public void OnClickButton(int i)
    {

        
        // 安全检查
        if (inventoryManager == null)
        {
            Debug.LogWarning("InventoryManager is null");
            return;
        }
        
        // 检查按钮索引是否在有效范围内
        if (buttons != null && (i < 0 || i >= buttons.Count))
        {
            Debug.LogWarning($"按钮索引 {i} 越界，按钮总数: {buttons.Count}");
            return;
        }
        
        // 检查库存索引是否在有效范围内
        if (i < 0 || i >= inventoryManager.GetHerbCount())
        {
            Debug.LogWarning($"草药索引 {i} 越界，库存数量: {inventoryManager.GetHerbCount()}");
            return;
        }
        
        var herb = inventoryManager.GetHerb(i);
        if (herb == null)
        {
            return;
        }
        
        // 获取UI组件
        Image targetDetail = null;
        TextMeshProUGUI targetName = null;
        TextMeshProUGUI targetDetailText = null;
        
        if (detailUIs != null && detailUIs.Count > 0)
            targetDetail = detailUIs[0];
        if (herbNames != null && herbNames.Count > 0)
            targetName = herbNames[0];
        if (herbDetails != null && herbDetails.Count > 0)
            targetDetailText = herbDetails[0];
        
        // 切换逻辑
        if (lastIndex != i)
        {
            // 显示选中草药的详情
            if (targetDetail != null) 
            {
                targetDetail.gameObject.SetActive(true);
            }
            if (targetName != null) targetName.text = herb.getHerbName;
            if (targetDetailText != null) targetDetailText.text = herb.getHerbDetail;
            lastIndex = i;
        }
        else
        {
            // 隐藏详情并触发选中事件
            if (targetDetail != null) 
            {
                targetDetail.gameObject.SetActive(false);
            }
            EventManager.CallSeleckedMainHerb(inventoryManager.GetHerb(i));
            lastIndex = -1;
        }
    }
}