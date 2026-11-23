using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FloatingJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("References")]
    public RectTransform background;
    public RectTransform handle;
    
    [Header("Settings")]
    [Range(50f, 150f)]
    public float handleRange = 100f; // LEBIH BESAR = LEBIH STABIL
    
    [Range(0.1f, 2f)]
    public float sensitivity = 1f;
    
    [Range(0f, 0.3f)]
    public float deadZone = 0.1f;
    
    public bool hideOnRelease = true;
    
    [Header("Position Override")]
    public bool useFixedPosition = true; // TOGGLE UNTUK FIXED POSITION
    public Vector2 fixedOffset = new Vector2(150f, 200f); // OFFSET DARI BOTTOM LEFT
    public bool useStartPositionAsFixed = true; // PAKE POSISI DI EDITOR
    
    [Header("Smoothing")]
    [Range(1f, 20f)]
    public float smoothSpeed = 10f;
    
    private Vector2 inputVector;
    private Vector2 smoothedInput;
    private Canvas canvas;
    private Camera cam;
    private RectTransform canvasRect;
    private Vector2 joystickStartPos;
    private bool isDragging = false;
    
    public float Horizontal => smoothedInput.x;
    public float Vertical => smoothedInput.y;
    public Vector2 Direction => smoothedInput;
    
    void Start()
    {
        // GET CANVAS
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("FloatingJoystick harus di dalam Canvas!");
            enabled = false;
            return;
        }
        
        canvasRect = canvas.transform as RectTransform;
        
        // SETUP CAMERA - FIX UNTUK MOBILE
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera || canvas.renderMode == RenderMode.WorldSpace)
        {
            cam = canvas.worldCamera;
            if (cam == null)
            {
                cam = Camera.main;
            }
        }
        else
        {
            // Screen Space Overlay = null camera
            cam = null;
        }
        
        // SAVE START POSITION
        joystickStartPos = background.anchoredPosition;
        
        // JIKA PAKE FIXED POSITION DARI EDITOR
        if (useFixedPosition && useStartPositionAsFixed)
        {
            fixedOffset = new Vector2(
                background.anchoredPosition.x + canvasRect.sizeDelta.x / 2f,
                background.anchoredPosition.y + canvasRect.sizeDelta.y / 2f
            );
        }
        
        // HIDE IF FLOATING MODE
        if (hideOnRelease)
        {
            background.gameObject.SetActive(false);
        }
        
        // RESET
        handle.anchoredPosition = Vector2.zero;
        
        // DEBUG INFO
        Debug.Log($"Joystick Setup - Canvas Mode: {canvas.renderMode}, Camera: {(cam != null ? cam.name : "null")}");
    }
    
    void Update()
    {
        // SMOOTH INPUT DENGAN INTERPOLASI
        if (isDragging)
        {
            smoothedInput = Vector2.Lerp(smoothedInput, inputVector, Time.deltaTime * smoothSpeed);
        }
        else
        {
            // RESET LEBIH CEPAT SAAT RELEASE
            smoothedInput = Vector2.Lerp(smoothedInput, Vector2.zero, Time.deltaTime * smoothSpeed * 2f);
        }
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        
        if (hideOnRelease)
        {
            background.gameObject.SetActive(true);
            
            // CONVERT SCREEN POSITION KE LOCAL POSITION
            Vector2 localPoint;
            
            if (useFixedPosition)
            {
                // GUNAKAN FIXED POSITION (DARI BOTTOM LEFT)
                localPoint = new Vector2(
                    -canvasRect.sizeDelta.x / 2f + fixedOffset.x,
                    -canvasRect.sizeDelta.y / 2f + fixedOffset.y
                );
                background.anchoredPosition = localPoint;
            }
            else if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                eventData.position,
                cam,
                out localPoint))
            {
                // FOLLOW FINGER (DEFAULT)
                background.anchoredPosition = localPoint;
            }
        }
        
        OnDrag(eventData);
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;
        
        // CALCULATE POSITION RELATIVE TO BACKGROUND
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background,
            eventData.position,
            cam,
            out localPoint))
        {
            // DEBUG - CEK NILAI
            // Debug.Log($"LocalPoint: {localPoint}");
            
            // CLAMP WITHIN HANDLE RANGE
            localPoint = Vector2.ClampMagnitude(localPoint, handleRange);
            handle.anchoredPosition = localPoint;
            
            // CALCULATE INPUT VECTOR (FIX: pastikan normalize bener)
            Vector2 rawInput = localPoint / handleRange;
            float magnitude = rawInput.magnitude;
            
            // APPLY DEAD ZONE
            if (magnitude < deadZone)
            {
                inputVector = Vector2.zero;
            }
            else
            {
                // REMAP DARI DEADZONE KE 1
                float adjustedMagnitude = Mathf.InverseLerp(deadZone, 1f, magnitude);
                
                // FIX: Normalize dulu baru kali magnitude
                Vector2 direction = rawInput.normalized;
                inputVector = direction * adjustedMagnitude * sensitivity;
                
                // CLAMP UNTUK SAFETY
                inputVector = Vector2.ClampMagnitude(inputVector, 1f);
                
                // DEBUG
                // Debug.Log($"Input: {inputVector}, Dir: {direction}, Mag: {adjustedMagnitude}");
            }
        }
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        
        // RESET HANDLE
        handle.anchoredPosition = Vector2.zero;
        inputVector = Vector2.zero;
        
        // HIDE OR RETURN TO START
        if (hideOnRelease)
        {
            background.gameObject.SetActive(false);
            smoothedInput = Vector2.zero; // INSTANT RESET SAAT HIDE
        }
        else
        {
            background.anchoredPosition = joystickStartPos;
        }
    }
    
    // UTILITY METHODS
    public void ResetJoystick()
    {
        isDragging = false;
        handle.anchoredPosition = Vector2.zero;
        inputVector = Vector2.zero;
        smoothedInput = Vector2.zero;
        
        if (hideOnRelease)
        {
            background.gameObject.SetActive(false);
        }
    }
    
    public float GetMagnitude()
    {
        return smoothedInput.magnitude;
    }
    
    void OnDisable()
    {
        ResetJoystick();
    }
}