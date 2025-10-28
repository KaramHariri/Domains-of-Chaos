using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "HealthAbilityStats", menuName = "Scriptable Objects/AbilitiesStats/HealthAbilityStats")]
public class HealthAbilityStats : ScriptableObject
{
    public List<HealthAbilityLevelInfo> HealthInfo;
    public int CurrentAbilityLevel = 0;
    public int MaxAbilityLevel = 8;

    internal void ResetAbilityLevel()
    {
        CurrentAbilityLevel = 0;
    }
}

[Serializable]
public class HealthAbilityLevelInfo
{
    [Header("Ability Name info")]
    public string Level;
    public new string Name;
    public string Description;
    public Sprite Sprite;
    public AbilityType AbilityType;


    [Header("Shield Variables")]
    public float MaxHealthMultiplierPercentage;
    public float LifeStealMultiplierPercentage;
    public float DamageReductionMultiplierPercentage;
    public bool MaxAbility;
}