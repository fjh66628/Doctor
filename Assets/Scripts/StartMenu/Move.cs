using UnityEngine;

public class FlyingObject : MonoBehaviour
{
    [Header("飞行设置")]
    [SerializeField] private float speed = 5f; // 飞行速度
    [SerializeField] private float screenBuffer = 1f; // 屏幕外缓冲距离
    
    [Header("旋转设置")]
    [SerializeField] private float rotationSpeed = 0f; // 旋转速度（0表示不旋转）
    [SerializeField] private float initialRotation = 0f; // 初始旋转角度
    
    [Header("调试")]
    [SerializeField] private bool showDebugGizmos = true;
    [SerializeField] private Color gizmoColor = Color.green;
    
    private Camera mainCamera;
    private Vector3 currentDirection;
    private bool isActive = true;
    private Quaternion initialRotationQuaternion;
    
    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("未找到主相机！");
            enabled = false;
            return;
        }
        
        // 设置初始旋转
        initialRotationQuaternion = Quaternion.Euler(0f, 0f, initialRotation);
        transform.rotation = initialRotationQuaternion;
        
        // 初始化物体位置和方向
        ResetObjectPositionAndDirection();
    }
    
    void Update()
    {
        if (!isActive || mainCamera == null) return;
        
        // 移动物体
        transform.position += currentDirection * speed * Time.deltaTime;
        
        // 旋转物体（如果启用旋转）
        if (rotationSpeed != 0f)
        {
            float rotationAmount = rotationSpeed * Time.deltaTime;
            transform.Rotate(0f, 0f, rotationAmount);
        }
        else
        {
            // 保持初始旋转
            transform.rotation = initialRotationQuaternion;
        }
        
        // 检查物体是否飞出屏幕
        CheckIfOutOfCameraView();
    }
    
    /// <summary>
    /// 重置物体位置和方向
    /// </summary>
    private void ResetObjectPositionAndDirection()
    {
        // 获取相机视口范围（世界坐标）
        Vector3 cameraPosition = mainCamera.transform.position;
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;
        
        // 确定从哪个屏幕边界进入
        // 0:上, 1:下, 2:左, 3:右
        int entrySide = Random.Range(0, 4);
        
        Vector3 startPosition = Vector3.zero;
        Vector3 targetPosition = Vector3.zero;
        
        switch (entrySide)
        {
            case 0: // 从上边界进入
                startPosition = new Vector3(
                    Random.Range(cameraPosition.x - cameraWidth/2, cameraPosition.x + cameraWidth/2),
                    cameraPosition.y + cameraHeight/2 + screenBuffer,
                    0
                );
                targetPosition = new Vector3(
                    Random.Range(cameraPosition.x - cameraWidth/2, cameraPosition.x + cameraWidth/2),
                    cameraPosition.y - cameraHeight/2 - screenBuffer,
                    0
                );
                break;
                
            case 1: // 从下边界进入
                startPosition = new Vector3(
                    Random.Range(cameraPosition.x - cameraWidth/2, cameraPosition.x + cameraWidth/2),
                    cameraPosition.y - cameraHeight/2 - screenBuffer,
                    0
                );
                targetPosition = new Vector3(
                    Random.Range(cameraPosition.x - cameraWidth/2, cameraPosition.x + cameraWidth/2),
                    cameraPosition.y + cameraHeight/2 + screenBuffer,
                    0
                );
                break;
                
            case 2: // 从左边界进入
                startPosition = new Vector3(
                    cameraPosition.x - cameraWidth/2 - screenBuffer,
                    Random.Range(cameraPosition.y - cameraHeight/2, cameraPosition.y + cameraHeight/2),
                    0
                );
                targetPosition = new Vector3(
                    cameraPosition.x + cameraWidth/2 + screenBuffer,
                    Random.Range(cameraPosition.y - cameraHeight/2, cameraPosition.y + cameraHeight/2),
                    0
                );
                break;
                
            case 3: // 从右边界进入
                startPosition = new Vector3(
                    cameraPosition.x + cameraWidth/2 + screenBuffer,
                    Random.Range(cameraPosition.y - cameraHeight/2, cameraPosition.y + cameraHeight/2),
                    0
                );
                targetPosition = new Vector3(
                    cameraPosition.x - cameraWidth/2 - screenBuffer,
                    Random.Range(cameraPosition.y - cameraHeight/2, cameraPosition.y + cameraHeight/2),
                    0
                );
                break;
        }
        
        // 设置位置
        transform.position = startPosition;
        
        // 计算方向（从起点指向目标点），但保持旋转不变
        currentDirection = (targetPosition - startPosition).normalized;
        
        // 不再根据方向自动设置旋转
        // 物体将保持初始旋转或在速度旋转控制下旋转
        
        // 重置到初始旋转（如果不启用旋转）
        if (rotationSpeed == 0f)
        {
            transform.rotation = initialRotationQuaternion;
        }
    }
    
    /// <summary>
    /// 检查物体是否飞出相机视野
    /// </summary>
    private void CheckIfOutOfCameraView()
    {
        // 将物体位置转换为视口坐标
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);
        
        // 如果物体完全离开视口区域（包括缓冲区域）
        if (viewportPosition.x < -screenBuffer || 
            viewportPosition.x > 1 + screenBuffer ||
            viewportPosition.y < -screenBuffer || 
            viewportPosition.y > 1 + screenBuffer)
        {
            // 重置物体位置和方向，开始新一轮飞行
            ResetObjectPositionAndDirection();
        }
    }
    
    /// <summary>
    /// 设置飞行速度
    /// </summary>
    public void SetSpeed(float newSpeed)
    {
        speed = Mathf.Max(0, newSpeed);
    }
    
    /// <summary>
    /// 设置旋转速度
    /// </summary>
    public void SetRotationSpeed(float newRotationSpeed)
    {
        rotationSpeed = newRotationSpeed;
    }
    
    /// <summary>
    /// 设置初始旋转角度
    /// </summary>
    public void SetInitialRotation(float newInitialRotation)
    {
        initialRotation = newInitialRotation;
        initialRotationQuaternion = Quaternion.Euler(0f, 0f, initialRotation);
        
        // 如果不启用旋转速度，立即应用新的旋转
        if (rotationSpeed == 0f)
        {
            transform.rotation = initialRotationQuaternion;
        }
    }
    
    /// <summary>
    /// 开始/停止飞行
    /// </summary>
    public void SetActive(bool active)
    {
        isActive = active;
    }
    
    /// <summary>
    /// 手动触发重置（用于测试或特殊需求）
    /// </summary>
    public void ManualReset()
    {
        ResetObjectPositionAndDirection();
    }
    
    /// <summary>
    /// 立即停止并重置物体
    /// </summary>
    public void StopAndReset()
    {
        isActive = false;
        ResetObjectPositionAndDirection();
    }
    
    /// <summary>
    /// 获取当前飞行方向
    /// </summary>
    public Vector3 GetCurrentDirection()
    {
        return currentDirection;
    }
    
    /// <summary>
    /// 获取当前速度
    /// </summary>
    public float GetCurrentSpeed()
    {
        return speed;
    }
    
    // 在场景视图中绘制调试信息
    void OnDrawGizmos()
    {
        if (!showDebugGizmos || mainCamera == null) return;
        
        Gizmos.color = gizmoColor;
        
        // 绘制当前运动方向
        if (Application.isPlaying)
        {
            Gizmos.DrawRay(transform.position, currentDirection * 2f);
            
            // 在物体上方显示速度信息
            #if UNITY_EDITOR
            GUIStyle style = new GUIStyle();
            style.normal.textColor = gizmoColor;
            UnityEditor.Handles.Label(
                transform.position + Vector3.up * 1.5f,
                $"速度: {speed:F1}\n方向: {currentDirection:F2}",
                style
            );
            #endif
        }
    }
}