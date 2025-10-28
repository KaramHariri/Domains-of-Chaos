using JetBrains.Annotations;
using System;
using UnityEngine;

public class Health
{
    private float CurrentHealth = 100f;
    public float MaxHealth = 100f;

    [HideInInspector] public float DamageReductionMultiplier = 0f;
    [HideInInspector] public float MaxHealthMultiplier = 0f;
    [HideInInspector] public float LifeStealMultiplier = 0f;

    public void SubscribeToDamageDealtAction()
    {
        Spear.OnDamageDealt += Heal;
    }

    public void UnSubscribeToDamageDealtAction()
    {
        Spear.OnDamageDealt -= Heal;
    }

    public void TakeDamage(float damageAmount)
    {
        float damageTaken = damageAmount * (1 - DamageReductionMultiplier);
        CurrentHealth -= damageTaken;
    }

    public bool IsDead()
    {
        if (CurrentHealth <= 0)
        {
            return true;
        }
        return false;
    }

    public void IncreaseMaxHealth()
    {
        float healthPercent = CurrentHealth / MaxHealth;
        MaxHealth = MaxHealth * (1 + MaxHealthMultiplier);
        CurrentHealth = healthPercent * MaxHealth;
        healthPercent = CurrentHealth / MaxHealth;
    }

    public void Heal(float damageAmount)
    {
        float healAmount = damageAmount * LifeStealMultiplier;
        CurrentHealth += healAmount;
        if (CurrentHealth >= MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
    }
}
