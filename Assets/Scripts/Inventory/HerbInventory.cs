using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class HerbInventory : MonoBehaviour
{

    public InventoryManager inventoryManager;
    public HerbSQ herbSQ;
    public AssistHerbSQ assistHerbSQ;
    [SerializeField] GameObject herbPrefab; // 用于实例化的 UI 预制体（应包含 Image、Button 和 TextMeshProUGUI）
    [SerializeField]List<Button> buttons;//传入按钮
    [SerializeField] List<Image> detailUIs;
    [SerializeField] List<TextMeshProUGUI> herbNames;
    [SerializeField] List<TextMeshProUGUI> herbDetails;
    [SerializeField] List<TextMeshProUGUI> textMeshProUGUIs;//传入按钮的文本
    [SerializeField] List<Button> assistHerbButtons;
    [SerializeField] List<TextMeshProUGUI> assistHerbtextMeshProUGUIs;
    int lastIndex = -1,assLastIndex = -1;
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
        Debug.Log("[HerbInventory] UpdateUI called");
        
        // 销毁已有子对象，避免重复生成
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            var child = transform.GetChild(i);
            if (Application.isPlaying)
                Destroy(child.gameObject);
            else
                DestroyImmediate(child.gameObject);
        }

        // 清理列表（容错检查）
        if (buttons != null) buttons.Clear();
        if (textMeshProUGUIs != null) textMeshProUGUIs.Clear();
        if (assistHerbButtons != null) assistHerbButtons.Clear();
        if (assistHerbtextMeshProUGUIs != null) assistHerbtextMeshProUGUIs.Clear();

        if (herbSQ == null || herbPrefab == null)
        {
            Debug.LogError($"[HerbInventory] Missing required fields - herbSQ: {herbSQ != null}, herbPrefab: {herbPrefab != null}");
            return;
        }
        
        var list = herbSQ.getHerbList;
        if (list == null || list.Count == 0) 
        {
            Debug.LogWarning($"[HerbInventory] getHerbList is null or empty. Count: {(list == null ? 0 : list.Count)}");
            return;
        }
        
        var assistList = assistHerbSQ != null ? assistHerbSQ.getAssistHerbList : null;
        
        Debug.Log($"[HerbInventory] Creating {list.Count} herb buttons and {(assistList == null ? 0 : assistList.Count)} assist herb buttons");

        for (int i = 0; i < list.Count; i++)
        {
            var item = list[i];
            // 保持本地坐标（避免世界坐标拉开导致不可见）
            GameObject newherb = Instantiate(herbPrefab, transform, false);

            // 立即激活实例，确保能通过 GetComponentInChildren 找到组件
            newherb.SetActive(true);

            // Image 可能在 prefab 的子对象上，优先使用 GetComponentInChildren（包含未激活子对象）
            var img = newherb.GetComponentInChildren<Image>(true);
            if (img == null)
            {
                Debug.LogWarning($"[HerbInventory] No Image component found on prefab or its children for index {i}");
            }
            else
            {
                // 确保对象和组件处于启用状态
                if (img.gameObject != null) img.gameObject.SetActive(true);
                img.enabled = true;
                if (item != null)
                {
                    img.sprite = item.getHerbSprite;
                    if (img.sprite == null)
                        Debug.LogWarning($"[HerbInventory] Assigned sprite is null for herb {item.getHerbName} (index {i})");
                }
            }

            var txt = newherb.GetComponentInChildren<TextMeshProUGUI>(true);
            if (txt != null)
            {
                if (txt.gameObject != null) txt.gameObject.SetActive(true);
                txt.enabled = true;
                if (item != null) txt.text = item.getHerbName;
            }

            // Button 可能在子对象上，使用 GetComponentInChildren 来确保能找到（包含未激活子对象）
            var btn = newherb.GetComponentInChildren<Button>(true);
            if (btn != null)
            {
                // 确保按钮对象与组件启用
                if (btn.gameObject != null) btn.gameObject.SetActive(true);
                btn.enabled = true;
                btn.interactable = true;

                int idx = i; // 捕获索引
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => OnClickButton(idx, 0));
                if (buttons != null) buttons.Add(btn);
            }

            if (textMeshProUGUIs != null && txt != null) textMeshProUGUIs.Add(txt);

            // 确保 RectTransform 在父级下可见（缩放/位置）
            var rt = newherb.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.localScale = Vector3.one;
                rt.anchoredPosition = Vector2.zero;
            }

        }
        
        // 处理辅助草药列表
        if (assistList != null && assistList.Count > 0)
        {
            for (int i = 0; i < assistList.Count; i++)
            {
                var item = assistList[i];
                GameObject newherb = Instantiate(herbPrefab, transform, false);
                newherb.SetActive(true);

                var img = newherb.GetComponentInChildren<Image>(true);
                if (img != null)
                {
                    if (img.gameObject != null) img.gameObject.SetActive(true);
                    img.enabled = true;
                    if (item != null)
                    {
                        img.sprite = item.getAssistHerbSprite;
                        if (img.sprite == null)
                            Debug.LogWarning($"[HerbInventory] Assigned sprite is null for assist herb {item.getAssistHerbName} (index {i})");
                    }
                }

                var txt = newherb.GetComponentInChildren<TextMeshProUGUI>(true);
                if (txt != null)
                {
                    if (txt.gameObject != null) txt.gameObject.SetActive(true);
                    txt.enabled = true;
                    if (item != null) txt.text = item.getAssistHerbName;
                }

                var btn = newherb.GetComponentInChildren<Button>(true);
                if (btn != null)
                {
                    if (btn.gameObject != null) btn.gameObject.SetActive(true);
                    btn.enabled = true;
                    btn.interactable = true;

                    int idx = i;
                    btn.onClick.RemoveAllListeners();
                    btn.onClick.AddListener(() => OnClickAssistHerbButton(idx, 0));
                    if (assistHerbButtons != null) assistHerbButtons.Add(btn);
                }

                if (assistHerbtextMeshProUGUIs != null && txt != null) assistHerbtextMeshProUGUIs.Add(txt);

                var rt = newherb.GetComponent<RectTransform>();
                if (rt != null)
                {
                    rt.localScale = Vector3.one;
                    rt.anchoredPosition = Vector2.zero;
                }
            }
        }
    }//仓库更新的时候更新UI


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
            Debug.LogWarning("[HerbInventory] detailUIs list is empty or not assigned in the inspector.");
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
    }//当Z键按下后
    public void OnClickButton(int i)
    {
        OnClickButton(i, 0);
    }

    public void OnClickButton(int i, int detailIndex)
    {
        if (inventoryManager == null)
        {
            Debug.LogWarning("InventoryManager is null");
            return;
        }

        if (i < 0 || i >= inventoryManager.GetHerbCount())
        {
            Debug.LogWarning($"Herb index out of range: {i}");
            return;
        }

        var herb = inventoryManager.GetHerb(i);
        if (herb == null)
        {
            Debug.LogWarning($"Herb at index {i} is null");
            return;
        }

        Image targetDetail = null;
        TextMeshProUGUI targetName = null;
        TextMeshProUGUI targetDetailText = null;

        if (detailUIs != null && detailIndex >= 0 && detailIndex < detailUIs.Count)
            targetDetail = detailUIs[detailIndex];
        if (herbNames != null && detailIndex >= 0 && detailIndex < herbNames.Count)
            targetName = herbNames[detailIndex];
        if (herbDetails != null && detailIndex >= 0 && detailIndex < herbDetails.Count)
            targetDetailText = herbDetails[detailIndex];

        if (lastIndex != i)
        {
            if (targetDetail != null) targetDetail.gameObject.SetActive(true);
            if (targetName != null) targetName.text = herb.getHerbName;
            if (targetDetailText != null) targetDetailText.text = herb.getHerbDetail;
            lastIndex = i;
        }
        else
        {
            if (targetDetail != null) targetDetail.gameObject.SetActive(false);
            EventManager.CallSeleckedMainHerb(i);
            lastIndex = -1;
        }
    }//点击，选中的逻辑

    public void OnClickAssistHerbButton(int i)
    {
        OnClickAssistHerbButton(i, 0);
    }

    public void OnClickAssistHerbButton(int i, int detailIndex)
    {
        if (inventoryManager == null)
        {
            Debug.LogWarning("InventoryManager is null");
            return;
        }

        if (i < 0 || i >= inventoryManager.GetAssistHerbCount())
        {
            Debug.LogWarning($"Assist herb index out of range: {i}");
            return;
        }

        var aHerb = inventoryManager.GetAssistHerb(i);
        if (aHerb == null)
        {
            Debug.LogWarning($"Assist herb at index {i} is null");
            return;
        }

        Image targetDetail = null;
        TextMeshProUGUI targetName = null;
        TextMeshProUGUI targetDetailText = null;

        if (detailUIs != null && detailIndex >= 0 && detailIndex < detailUIs.Count)
            targetDetail = detailUIs[detailIndex];
        if (herbNames != null && detailIndex >= 0 && detailIndex < herbNames.Count)
            targetName = herbNames[detailIndex];
        if (herbDetails != null && detailIndex >= 0 && detailIndex < herbDetails.Count)
            targetDetailText = herbDetails[detailIndex];

        if (assLastIndex != i)
        {
            if (targetDetail != null) targetDetail.gameObject.SetActive(true);
            if (targetName != null) targetName.text = aHerb.getAssistHerbName;
            if (targetDetailText != null) targetDetailText.text = aHerb.getAssistHerbDetail;
            assLastIndex = i;

        }
        else
        {
            if (targetDetail != null) targetDetail.gameObject.SetActive(false);
            if (aHerb.getAssistHerbName != "空")
                EventManager.CallSeleckedAssistHerb(i);
            assLastIndex = -1;
        }

    }//点击，选中的逻辑
    
}
