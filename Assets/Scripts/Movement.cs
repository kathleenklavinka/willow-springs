using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 3;
    public Animator animator;
    public Vector3 direction;

    public void Update() 
    {
        // Support both PC and Mobile input
        Vector2 input = Vector2.zero;
        
        // Try mobile input first
        if (MobileInputManager.instance != null)
        {
            input = MobileInputManager.instance.GetMovementInput();
        }
        
        if (input == Vector2.zero)
        {
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        
        direction = new Vector3(input.x, input.y, 0);
        AnimateMovement(direction);
    }

    private void FixedUpdate() 
    {
        transform.position += direction * speed * Time.deltaTime;
    }
    
    void AnimateMovement(Vector3 direction) 
    {
        if (animator != null) 
        {
            if (direction.magnitude > 0) 
            {
                animator.SetBool("isMoving", true);
                animator.SetFloat("horizontal", direction.x);
                animator.SetFloat("vertical", direction.y);
            } 
            else 
            {
                animator.SetBool("isMoving", false);
            }
        }
    }
}