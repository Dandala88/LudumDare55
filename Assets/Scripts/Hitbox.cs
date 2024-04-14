using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public AudioClip hit;
    private SphereCollider coll;
    private AudioSource audioSource;

    private void Awake()
    {
        coll = GetComponent<SphereCollider>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<IHurt>() != null)
        {
            var hurtable = other.gameObject.GetComponent<IHurt>();
            audioSource.PlayOneShot(hit,GameManager.SfxVolumeScale);
            hurtable.Hurt(hurtable.Damage());
        }
    }

    public void EnableHitBox(bool enable)
    {
        coll.enabled = enable;
    }
}
