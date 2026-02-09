using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class HoverEvent : UnityEvent<int> { }

public class ButtonHoverDetector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("按钮设置")]
    [SerializeField] private Button targetButton;
    [SerializeField] private int buttonIndex = 0;
    
    [Header("悬停事件")]
    public HoverEvent onHoverEnter = new HoverEvent();
    public HoverEvent onHoverExit = new HoverEvent();
    public HoverEvent onHoverStay = new HoverEvent();
    private bool isHovering = false;
    private Image buttonImage;
    
    private void Start()
    {
        if (targetButton == null)
        {
            targetButton = GetComponent<Button>();
        }
        
        if (targetButton != null)
        {
            buttonImage = targetButton.image;
        }
    }
    
    private void Update()
    {
        if (isHovering)
        {
            onHoverStay?.Invoke(buttonIndex);
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        onHoverEnter?.Invoke(buttonIndex);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        onHoverExit?.Invoke(buttonIndex);
    }
    
    // 设置按钮索引
    public void SetButtonIndex(int index)
    {
        buttonIndex = index;
    }
}