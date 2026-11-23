using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInputManager : MonoBehaviour
{
    public static MobileInputManager instance;
    
    [Header("Joystick Settings")]
    public FloatingJoystick joystick;
    
    [Header("Auto Action")]
    private bool actionPressed = false;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Untuk movement
    public Vector2 GetMovementInput()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (joystick != null)
        {
            return new Vector2(joystick.Horizontal, joystick.Vertical);
        }
        return Vector2.zero;
#else
        // PC Input
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
#endif
    }
    
    // Single action button (auto-detect apa yang harus dilakukan)
    public bool GetActionDown()
    {
        bool result = actionPressed;
        actionPressed = false;
        return result;
    }
    
    public void OnActionButton()
    {
        actionPressed = true;
    }
}