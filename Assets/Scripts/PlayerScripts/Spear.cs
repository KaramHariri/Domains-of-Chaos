using System;
using UnityEngine;

public class Spear : MonoBehaviour
{
    [SerializeField] private float Speed = 20f;
    private float Damage = 50f;
    private Vector2 ThrowDirection = Vector2.zero;
    private Rigidbody2D Rb;
    public static event Action<float> OnDamageDealt;
    private int PeircingCount = 1;
    private float SpearLifeTimer = 15f;
    GameObject Target;

    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        SetSpearDamage();
        SetPeircingCount();
        Invoke("ReturnSpearToPool", SpearLifeTimer);
    }

    void Update()
    {
        Rb.linearVelocity = ThrowDirection * Speed;
        SetSpearRotation();
    }

    public void SetThrowDirection(Vector2 dir)
    {
        ThrowDirection = dir;
    }

    public void SetTarget(GameObject target)
    {
        Target = target;
    }

    private void SetSpearRotation()
    {
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, ThrowDirection);
        transform.rotation = rotation;
    }

    public void SetSpearDamage()
    {
        Damage =  Damage * (1 + AbilityController.Instance.SpearDamageMultiplier);
    }
   
    public void SetPeircingCount()
    {
        PeircingCount = AbilityController.Instance.MaxSpearPiercingCount;
    }

    private void ReturnSpearToPool()
    {
        ObjectPooler.Instance.ReturnToPool("Spear", this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            PeircingCount--;
            other.GetComponent<IDamagable>().TakeDamage(Damage);
            if (PeircingCount <= 0)
            {
                ObjectPooler.Instance.ReturnToPool("Spear", this.gameObject);
                CancelInvoke("ReturnSpearToPool");
            }
        }
        OnDamageDealt?.Invoke(Damage);

    }
}
