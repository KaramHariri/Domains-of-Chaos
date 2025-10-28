using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

// level 2 shield damage
// level 3 rotation speed
// level 4 increase number of shields.
// level 5 shield damage
// level 6 rotation speed
// level 7 increase number.
// level 8 stun.


public class Shield : MonoBehaviour
{
    public GameObject Prefab;         // Orbiting object prefab

    [Header("Settings")]
    public float Radius = 3f;         // Distance from player
    private float BaseDamage = 5f;


    public void RotateAroundPlayer(Transform playerTransform, float angle)
    {
        transform.RotateAround(playerTransform.position, Vector3.forward, angle);
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
        {
            float damage = BaseDamage * (1 + AbilityController.Instance.ShieldDamageMultiplier);
            other.GetComponent<IDamagable>().TakeDamage(damage);
        }
    }


}
