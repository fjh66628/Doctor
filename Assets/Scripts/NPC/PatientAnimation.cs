using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PatientAnimation : MonoBehaviour
{
    [Header("Sprite设置")]
    [SerializeField] Sprite patientSprite; // 可选的Sprite资源，如需更改Sprite可在此赋值
    [SerializeField] List<Sprite> spriteList = new List<Sprite>(); // 存储可能用到的图片列表
    
    [Header("动画设置")]
    public float fadeDuration = 1.0f; // 淡入淡出持续时间，可在Inspector中调整
    public float moveAmplitude = 0.5f; // 上下移动的幅度
    public float moveSpeed = 1.0f; // 上下移动的速度
    public bool enableMovement = true; // 是否启用上下移动效果
    
    [Header("Sprite切换设置")]
    public bool randomSprite = true; // 是否随机切换Sprite
    public int currentSpriteIndex = 0; // 当前Sprite索引（如果按顺序切换）
    
    private SpriteRenderer spriteRenderer;
    private Vector3 originalPosition; // 记录原始位置
    private Coroutine movementCoroutine; // 移动协程引用
    
    void Start()
    {
        // 获取或添加SpriteRenderer组件
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
        
        // 设置初始Sprite
        if (patientSprite != null)
        {
            spriteRenderer.sprite = patientSprite;
        }
        else if (spriteList.Count > 0)
        {
            spriteRenderer.sprite = spriteList[0];
        }
        
        // 记录原始位置
        originalPosition = transform.position;
        
        // 启动上下移动动画
        if (enableMovement)
        {
            StartMovement();
        }
    }
    
    void OnEnable()
    {
        EventManager.CureSuccessfullyEvent += ChangeSprite;
    }
    
    void OnDisable()
    {
        EventManager.CureSuccessfullyEvent -= ChangeSprite;
        
    }
    
    // 开始上下移动动画
    public void StartMovement()
    {
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
        }
        movementCoroutine = StartCoroutine(MoveUpDownCoroutine());
    }
    
    // 停止上下移动动画
    public void StopMovement()
    {
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
            movementCoroutine = null;
        }
        
        // 回到原始位置
        transform.position = originalPosition;
    }
    
    // 上下移动的协程
    IEnumerator MoveUpDownCoroutine()
    {
        float time = 0f;
        
        while (true)
        {
            // 使用正弦函数实现平滑的上下移动
            float yOffset = Mathf.Sin(time * moveSpeed) * moveAmplitude;
            
            // 更新位置：只在Y轴上移动
            transform.position = new Vector3(
                originalPosition.x,
                originalPosition.y + yOffset,
                originalPosition.z
            );
            
            // 更新时间
            time += Time.deltaTime;
            
            yield return null;
        }
    }
    
    // 切换Sprite的方法（不传入参数）
    void ChangeSprite()
    {
        StartCoroutine(ChangeColorAndFade());
    }
    
    // 切换Sprite的协程
    IEnumerator ChangeColorAndFade()
    {
        if (spriteRenderer == null || spriteList.Count == 0) yield break;
        
        // 暂时停止上下移动
        bool wasMoving = enableMovement;
        if (wasMoving)
        {
            StopMovement();
        }
        
        Color originalColor = spriteRenderer.color;
        float timer = 0f;
        
        // 第一阶段：淡出
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
        
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        
        // 选择新的Sprite
        Sprite newSprite = GetNextSprite();
        if (newSprite != null)
        {
            spriteRenderer.sprite = newSprite;
        }
        
        // 随机颜色
        Color randomColor = GenerateRandomColor();
        timer = 0f;
        
        // 第二阶段：淡入
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            spriteRenderer.color = new Color(randomColor.r, randomColor.g, randomColor.b, alpha);
            yield return null;
        }
        
        spriteRenderer.color = new Color(randomColor.r, randomColor.g, randomColor.b, 1f);
        
        // 恢复上下移动
        if (wasMoving && enableMovement)
        {
            StartMovement();
        }
    }
    
    // 获取下一个Sprite
    Sprite GetNextSprite()
    {
        if (spriteList.Count == 0) return null;
        
        if (randomSprite)
        {
            // 随机选择一个Sprite
            int randomIndex = Random.Range(0, spriteList.Count);
            currentSpriteIndex = randomIndex;
            return spriteList[randomIndex];
        }
        else
        {
            // 按顺序选择下一个Sprite
            currentSpriteIndex = (currentSpriteIndex + 1) % spriteList.Count;
            return spriteList[currentSpriteIndex];
        }
    }
    
    // 生成随机颜色
    Color GenerateRandomColor()
    {
        float hue = Random.Range(0f, 1f);
        float saturation = Random.Range(0.3f, 0.7f);
        float value = Random.Range(0.3f, 0.8f);
        return Color.HSVToRGB(hue, saturation, value);
    }
    // 设置移动参数
    public void SetMovementParameters(float amplitude, float speed, bool enable = true)
    {
        moveAmplitude = amplitude;
        moveSpeed = speed;
        enableMovement = enable;
        
        // 重启移动动画
        if (enableMovement)
        {
            StopMovement();
            StartMovement();
        }
        else
        {
            StopMovement();
        }
    }
}