using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AbilityController : MonoBehaviour
{
    public static AbilityController Instance { get; private set; }

    [Header("References")]
    [SerializeField] SpearAbilityStats SpearAbilityStats;
    [SerializeField] ShieldAbilityStats ShieldAbilityStats;

    #region Spear Settings
    [Header("Spear Settings")]
    [SerializeField] private GameObject SpearPrefab;
    [HideInInspector] public float SpearFireRateMultiplier = 0f;
    [HideInInspector] public float SpearDamageMultiplier = 0f;
    [HideInInspector] public int MaxSpearPiercingCount = 0;
    private SpearAbility SpearAbilityScript;
    #endregion

    #region Shield Settings
    [Header("Shield Settings")]
    [SerializeField] GameObject ShieldPrefab;
    [SerializeField] private int MaxShieldCount;
    [SerializeField] private float DistanceToPlayer = 3f;
    [SerializeField] private float ShieldOrbitSpeed = 45f;
    public float ShieldRotationSpeedMultiplier = 0f;
    [HideInInspector] public float ShieldDamageMultiplier = 0f;
    private ShieldAbility ShieldAbilityScript;
    private int CurrentShieldCount = 0;
    #endregion

    private void Awake()
    {
        Instance = this;
        SpearAbilityScript = new SpearAbility();
        ShieldAbilityScript = new ShieldAbility();

    }

    private void OnEnable()
    {
        Card.OnAbilitySelected += UpdateAbility;
    }

    private void OnDisable()
    {
        Card.OnAbilitySelected -= UpdateAbility;
    }

    public void UpdateAbilities()
    {
        if (SpearAbilityScript.CanThrowSpear())
        {
            GameObject closestEnemy = SpearAbilityScript.GetClosestEnemy(this.transform.position);
            if (closestEnemy != null)
            {
                SpearAbilityScript.InstantiateSpear(SpearPrefab, this.transform.position, Quaternion.identity);
            }
        }

        if (CurrentShieldCount != MaxShieldCount)
        {
            CurrentShieldCount++;
            ShieldAbilityScript.CreateShields(ShieldPrefab, this.transform, DistanceToPlayer, CurrentShieldCount);
        }
    }

    public void LateUpdateAbilities()
    {
        ShieldAbilityScript.RotateShields(this.transform, ShieldOrbitSpeed * (1 + ShieldRotationSpeedMultiplier));
    }

    private void UpdateAbility(AbilityType currentSelectionAbility)
    {
        switch (currentSelectionAbility)
        {
            case AbilityType.SPEAR:
                UpdateSpearAbilityStat();
                break;
            case AbilityType.SHIELD:
                UpdateShieldAbilityStat();
                break;
        }
    }

    private void UpdateSpearAbilityStat()
    {
        // Get the Ability stats from scriptable object.
        float SpearDamageMultiplierPercentage = SpearAbilityStats.SpearInfo[SpearAbilityStats.CurrentAbilityLevel].SpearDamageMultiplierPercentage;
        float SpearFireRateMultiplierPercentage = SpearAbilityStats.SpearInfo[SpearAbilityStats.CurrentAbilityLevel].FireRateMultiplierPercentage;

        // Update the ability values.
        MaxSpearPiercingCount = SpearAbilityStats.SpearInfo[SpearAbilityStats.CurrentAbilityLevel].SpearPiercingCount;
        SpearDamageMultiplier += Utility.GetMultiplierValueFromPercentage(SpearDamageMultiplierPercentage);
        SpearFireRateMultiplier += Utility.GetMultiplierValueFromPercentage(SpearFireRateMultiplierPercentage);


        SpearAbilityStats.CurrentAbilityLevel++;
    }

    private void UpdateShieldAbilityStat()
    {
        // Get the Ability stats from scriptable object.
        float ShieldRotationSpeedMultiplierPercentage = ShieldAbilityStats.ShieldInfo[ShieldAbilityStats.CurrentAbilityLevel].ShieldRotationSpeedMultiplierPercentage;
        float ShieldDamageMultiplierPercentage = ShieldAbilityStats.ShieldInfo[ShieldAbilityStats.CurrentAbilityLevel].ShieldDamageMultiplierPercentage;

        MaxShieldCount += ShieldAbilityStats.ShieldInfo[ShieldAbilityStats.CurrentAbilityLevel].ShieldCount;
        ShieldRotationSpeedMultiplier += Utility.GetMultiplierValueFromPercentage(ShieldRotationSpeedMultiplierPercentage);
        ShieldDamageMultiplier += Utility.GetMultiplierValueFromPercentage(ShieldDamageMultiplierPercentage);
        ShieldAbilityStats.CurrentAbilityLevel++;
    }

}
