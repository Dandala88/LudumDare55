using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IHurt
{
    public float footSpeed;
    public float maxHealth;
    public int attack;
    public bool acquired;
    public AudioClip hurtClip;
    public AudioClip attackClip;
    public int direction;

    public delegate void HealthChangeAction(float newHealth, float maxHealth);
    public static event HealthChangeAction OnHealthChange;

    private Vector2 input;
    private Animator animator;
    private bool attackCycle;
    private float health;
    public Vector3 movement;
    private Hitbox[] hitboxes;

    private CharacterController characterController;
    private PlayerSummons playerSummons;
    private AudioSource audioSource;


    private bool attacking;

    private void Awake()
    {
        hitboxes = GetComponentsInChildren<Hitbox>();
        direction = 1;
        health = maxHealth;
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        playerSummons = GetComponentInParent<PlayerSummons>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        EnableHitbox(false);
    }

    private void OnEnable()
    {
        if(OnHealthChange != null)
            OnHealthChange.Invoke(health, maxHealth);
        animator.Play("Idle");
    }

    void Update()
    {
        transform.forward = Vector3.right * direction;
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
            if (input.x != 0)
                direction = (int)Mathf.Sign(input.x);
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if(!attacking)
                audioSource.PlayOneShot(attackClip, 3);
            EnableHitbox(true);
            attacking = true;
            animator.Play("Attack_Primary");

            //if (attackCycle)
            //    animator.Play("Attack_Primary");
            //else
            //    animator.Play("Attack_Secondary");
        }
    }

    public void Summon(InputAction.CallbackContext context)
    {
        if (context.started && !attacking)
        {
            var value = (int)Mathf.Sign(context.ReadValue<float>());
            playerSummons.Summon(value);
        }
    }

    public void EndAttack()
    {
        attacking = false;
        EnableHitbox(false);
        animator.Play("Idle");
        attackCycle = !attackCycle;
    }

    public void Hurt(int amount)
    {
        audioSource.PlayOneShot(hurtClip, 3f);
        health-=amount;
        OnHealthChange.Invoke(health, maxHealth);
        if(health <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        SceneManager.LoadSceneAsync(0);
    }

    private void EnableHitbox(bool enable)
    {
        foreach (var fist in hitboxes)
            fist.EnableHitBox(enable);
    }

    public int Damage()
    {
        return attack;
    }
}
