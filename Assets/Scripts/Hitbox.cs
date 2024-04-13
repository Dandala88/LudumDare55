using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private SphereCollider coll;

    private void Awake()
    {
        coll = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<IHurt>() != null)
        {
            var hurtable = other.gameObject.GetComponent<IHurt>();
            hurtable.Hurt(hurtable.Damage());
        }
    }

    public void EnableHitBox(bool enable)
    {
        coll.enabled = enable;
    }
}
