using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFist : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Enemy>() != null)
        {
            var enemy = other.gameObject.GetComponent<Enemy>();
            enemy.Hurt();
        }
    }
}
