using System;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float Speed = 20f;
    private float Damage = 25f;
    private Vector2 MoveDirection = Vector2.zero;
    private Rigidbody2D Rb;

    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        Invoke("ReturnToPool", 10f);
    }

    void Update()
    {
        Rb.linearVelocity = MoveDirection * Speed;
    }

    public void SetThrowDirection(Vector2 dir)
    {
        MoveDirection = dir;
    }

    public void SetProjectileDamage(float damage)
    {
        Damage = damage;
    }

    private void ReturnToPool()
    {
        ObjectPooler.Instance.ReturnToPool(gameObject.name, this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<IDamagable>().TakeDamage(Damage);
            ReturnToPool();
        }
    }
}
