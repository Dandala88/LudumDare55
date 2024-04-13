using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float footSpeed;
    public float maxHealth;

    public delegate void HealthChangeAction(float newHealth, float maxHealth);
    public static event HealthChangeAction OnHealthChange;

    private Vector2 input;
    private Animator animator;
    private bool punchArm;
    private float health;
    private Vector3 movement;

    private CharacterController characterController;

    private void Awake()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (characterController.isGrounded)
        {
            movement.y = 0f;
        }
        else
        {
            movement.y += Physics.gravity.y * Time.deltaTime;
        }
        characterController.Move(movement * footSpeed * Time.deltaTime);
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            input = context.ReadValue<Vector2>();
            movement = new Vector3(input.x, 0f, input.y);
        }
    }

    public void Punch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (punchArm)
                animator.Play("Player_Punch_Right");
            else
                animator.Play("Player_Punch_Left");
        }
    }

    public void EndPunch()
    {
        animator.Play("Player_Idle");
        punchArm = !punchArm;
    }

    public void Hurt()
    {
        health--;
        OnHealthChange.Invoke(health, maxHealth);
        Debug.Log(health / maxHealth);
    }

    public void Death()
    {
        Destroy(gameObject);
    }
}
