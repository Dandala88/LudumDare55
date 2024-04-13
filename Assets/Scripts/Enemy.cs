using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Enemy : Summon
{
    public float footSpeed;
    public float punchInterval;
    public float health;

    private Animator animator;
    private bool punchArm;

    private Vector3 movement;
    private CharacterController characterController;
    private GameObject target;
    private float punchElapsed;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        movement = Vector3.left;
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

        if (target != null)
        {
            var targetMovement = (target.transform.transform.position - transform.position).normalized;
            movement = new Vector3(targetMovement.x, movement.y, targetMovement.z);
            if (punchElapsed >= punchInterval)
            {
                Punch();
                punchElapsed = 0;
            }
            punchElapsed += Time.deltaTime;
        }
        characterController.Move(movement * footSpeed * Time.deltaTime);
    }

    private void LateUpdate()
    {
        if(target != null)
        {
            transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
        }
    }

    public void Punch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Punch();
        }
    }

    private void Punch()
    {
        if (punchArm)
            animator.Play("Player_Punch_Right");
        else
            animator.Play("Player_Punch_Left");
    }

    public void EndPunch()
    {
        animator.Play("Player_Idle");
        punchArm = !punchArm;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player) && target == null && health > 0)
        {
            target = player.gameObject;
        }
    }

    public void Hurt()
    {
        health--;
        if(health <= 0)
        {
            target = null;
            characterController.height = 0;
            movement = Vector3.down;
            animator.Play("Player_Death");
        }
    }

    public void Death()
    {
        Destroy(gameObject);
    }
}
