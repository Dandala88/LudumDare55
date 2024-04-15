using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IHurt
{
    public float footSpeed;
    public float health;
    public float maxHealth;
    public int attack;
    public bool acquired;
    public AudioClip hurtClip;
    public AudioClip attackClip;
    public int direction;
    public float iFrames;
    [Tooltip("iFlashFrames / iframes = time between flashes")]
    public float iFlashFrames;
    public GameManager.SummonType summonType;

    public delegate void HealthChangeAction(float newHealth, float maxHealth);
    public static event HealthChangeAction OnHealthChange;

    private Vector2 input;
    private Animator animator;
    private bool attackCycle;
    public Vector3 movement;
    private Hitbox[] hitboxes;

    private CharacterController characterController;
    private PlayerSummons playerSummons;
    private AudioSource audioSource;
    private bool invincible;
    private Renderer rend;

    
    public bool attacking;
    private bool preventAutoSwitch;

    private void Awake()
    {
        hitboxes = GetComponentsInChildren<Hitbox>();
        direction = 1;
        health = maxHealth;
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        playerSummons = GetComponentInParent<PlayerSummons>();
        audioSource = GetComponent<AudioSource>();
        rend = GetComponentInChildren<Renderer>();

        if (Input.anyKey)
            preventAutoSwitch = true;
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

    public void Move(Vector2 sentInput)
    {
        input = sentInput;
        movement = new Vector3(input.x, 0f, input.y);
        if (input.x != 0)
        {
            if(!attacking)
                animator.Play("Walk");
            direction = (int)Mathf.Sign(input.x);
        }
        else
        {
            if (!attacking)
                animator.Play("Idle");
        }
    }

    public void Attack()
    {
        if(!attacking)
            audioSource.PlayOneShot(attackClip, GameManager.SfxVolumeScale);
        EnableHitbox(true);
        attacking = true;
        animator.Play("Attack_Primary");
    }

    public void Summon(InputAction.CallbackContext context)
    {
        if (context.started && !attacking && !preventAutoSwitch)
        {
            var value = (int)Mathf.Sign(context.ReadValue<float>());
            Debug.Log(value);
            playerSummons.Summon(value);
        }

        if(context.canceled)
        {
            preventAutoSwitch = false;
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
        if (!invincible)
        {
            audioSource.PlayOneShot(hurtClip, GameManager.SfxVolumeScale);
            health -= amount;
            OnHealthChange.Invoke(health, maxHealth);
            if (health <= 0)
            {
                Death();
            }
            else
                StartCoroutine(IFramesCoroutine());
        }
    }

    public void Death()
    {
        if(playerSummons.HasSummonsLeft())
            playerSummons.Summon(1);
        else
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

    private IEnumerator IFramesCoroutine()
    {
        var baseColor = rend.materials[0].color;
        var colorToggle = false;
        invincible = true;
        var t = 0f;
        while (t < iFrames)
        {
            if (colorToggle)
                rend.materials[0].color = Color.red;
            else
                rend.materials[0].color = baseColor;
            colorToggle = !colorToggle;
            var flashTime = iFlashFrames / iFrames;
            yield return new WaitForSeconds(flashTime);
            t += flashTime;
        }
        rend.enabled = true;
        invincible = false;
    }
}
