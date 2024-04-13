using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float footSpeed;
    public float health;

    private Vector2 input;
    private Animator animator;
    private bool punchArm;

    private CharacterController characterController;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    public Vector3 Movement
    {
        get { return new Vector3(input.x, 0f, input.y); }
    }

    void Update()
    {
        characterController.Move(Movement * footSpeed * Time.deltaTime);
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            input = context.ReadValue<Vector2>();
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
    }

    public void Death()
    {
        Destroy(gameObject);
    }
}
