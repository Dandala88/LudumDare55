using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : MonoBehaviour, IHurt
{
    public float footSpeed;
    public float health;
    public int attack;
    public float attackInterval;
    public bool hasSecondary;
    public float deathSeconds;
    public AudioClip hurtClip;
    public AudioClip attackClip;

    private Animator animator;
    private bool attackCycle;

    private Vector3 movement;
    private CharacterController characterController;
    private GameObject target;
    private float attackElapsed;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        PlayerSummons.OnSummonChange += ChangeTarget;
    }

    private void Start()
    {
        movement = Vector3.left;
        animator.Play("Idle");
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
            if (attackElapsed >= attackInterval)
            {
                Attack();
                attackElapsed = 0;
            }
            attackElapsed += Time.deltaTime;
        }
        characterController.Move(movement * footSpeed * Time.deltaTime);
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            var diffVec = target.transform.position - transform.position;

            transform.forward = Vector3.right * Mathf.Sign(diffVec.x);
        }
    }

    private void Attack()
    {
        audioSource.PlayOneShot(attackClip, 3);
        if (attackCycle || !hasSecondary)
            animator.Play("Attack_Primary");
        else
            animator.Play("Attack_Secondary");
    }

    public void EndAttack()
    {
        animator.Play("Idle");
        attackCycle = !attackCycle;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player) && target == null && health > 0)
        {
            target = player.gameObject;
        }
    }

    public void Hurt(int amount)
    {
        audioSource.PlayOneShot(hurtClip, 3);
        health -= amount;
        if (health <= 0)
        {
            target = null;
            characterController.height = 0;
            movement = Vector3.down;
            animator.Play("Death");
            StartCoroutine(DeathCoroutine());
        }
    }

    public void ChangeTarget(Player player)
    {
        target = player.gameObject;
    }

    public int Damage()
    {
        return attack;
    }

    private IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(deathSeconds);
        Death();
    }

    public void Death()
    {
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        PlayerSummons.OnSummonChange -= ChangeTarget;
    }

    private void OnDestroy()
    {
        WaveManager.currentWaveSummons.Remove(this);
    }
}
