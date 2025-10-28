using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShieldAbilityStats", menuName = "Scriptable Objects/AbilitiesStats/ShieldAbilityStats")]
public class ShieldAbilityStats : ScriptableObject
{
    public List<ShieldAbilityLevelInfo> ShieldInfo;
    public int CurrentAbilityLevel = 0;
    public int MaxAbilityLevel = 8;

    internal void ResetAbilityLevel()
    {
        CurrentAbilityLevel = 0;
    }
}

[Serializable]
public class ShieldAbilityLevelInfo
{
    [Header("Ability Name info")]
    public string Level;
    public new string Name;
    public string Description;
    public Sprite Sprite;
    public AbilityType AbilityType;


    [Header("Shield Variables")]
    public int ShieldCount;
    public float ShieldRotationSpeedMultiplierPercentage;
    public float ShieldDamageMultiplierPercentage;
    public bool MaxAbility;
}