using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;


// level 2 increase movement.
// level 3 cooldown reduction.
// level 4 extra dash.
// level 5 distance.
// level 6 increase movement.
// level 7 cooldown reduction.
// level 8 elemental dash.

public class PlayerController : MonoBehaviour, IDamagable
{
    [HideInInspector] public static PlayerController Instance;

    #region Input Actions
    //Input Actions
    [Header("Input Actions and Animator")]
    public InputActionReference MovementAction;
    public InputActionReference DashAction;
    private Animator PlayerAnimator;
    #endregion

    #region Ability Stats References
    //Ability stats
    [Header("AbilityStats References")]
    [SerializeField] private DashAbilityStats DashAbilityStats;
    [SerializeField] private HealthAbilityStats HealthAbilityStats;
    #endregion

    #region Movement variables
    private float MoveSpeed = 5.0f;
    private float SpeedMultiplier = 0;
    private Vector2 MoveDirection = Vector2.zero;
    #endregion

    #region Scirpts
    private Dash DashScript;
    private Health HealthScript;
    #endregion

    #region Screen Boundaries
    private float Bottom = -89f;
    private float Top = 108f;
    private float Right = 144f;
    private float Left = -154f;
    #endregion

    private Vector2 InputDirection;
    [HideInInspector] public Rigidbody2D Rb;
    private AbilityController AbilityControllerScript;

    void Awake()
    {
        Instance = this;
        Rb = GetComponent<Rigidbody2D>();
        DashScript = GetComponent<Dash>();
        AbilityControllerScript = GetComponent<AbilityController>();
        HealthScript = new Health();
        PlayerAnimator = GetComponent<Animator>();
    }

    public void UpdatePlayer()
    {
        if (DashScript.IsCurrentlyDashing()) return;

        InputDirection = MovementAction.action.ReadValue<Vector2>();
        
        Move();
        UpdateAnimation();
        AbilityControllerScript.UpdateAbilities();
    }

    public void LatePlayerUpdate()
    {
        AbilityControllerScript.LateUpdateAbilities();
        BoundaryCheck();
    }

    private void DashCallback(InputAction.CallbackContext context)
    {
        DashScript.DashCallback();
    }

    private void OnEnable()
    {
        DashAction.action.started += DashCallback;
        Card.OnAbilitySelected += UpdateAbility;
        HealthScript.SubscribeToDamageDealtAction();

        DashAbilityStats.CurrentAbilityLevel = 0;
        HealthAbilityStats.CurrentAbilityLevel = 0;
    }

    private void OnDisable()
    {
        DashAction.action.started -= DashCallback;
        Card.OnAbilitySelected -= UpdateAbility;
        HealthScript.UnSubscribeToDamageDealtAction();

        DashAbilityStats.CurrentAbilityLevel = 0;
        HealthAbilityStats.CurrentAbilityLevel = 0;
    }

    public Vector2 GetInputDirection()
    {
        return InputDirection.normalized;
    }

    private void BoundaryCheck()
    {
        if (transform.position.y < Bottom)
            transform.position = new Vector3(transform.position.x, Bottom, transform.position.z);
        if (transform.position.y > Top)
            transform.position = new Vector3(transform.position.x, Top, transform.position.z);
        if (transform.position.x > Right)
            transform.position = new Vector3(Right, transform.position.y, transform.position.z);
        if (transform.position.x < Left)
            transform.position = new Vector3(Left, transform.position.y, transform.position.z);

    }

    public void Move()
    {
        MoveDirection = GetInputDirection();
        Rb.linearVelocity = new Vector2(MoveDirection.x * MoveSpeed * (1 + SpeedMultiplier), MoveDirection.y * MoveSpeed * (1 + SpeedMultiplier));
    }

    private void UpdateAbility(AbilityType currentSelectionAbility)
    {
        switch (currentSelectionAbility)
        {
            case AbilityType.DASH:
                UpdateDashAbilityStat();
                break;
            case AbilityType.HEALTH:
                UpdateHealthAbilityStat();
                break;
        }
    }

    private void UpdateAnimation()
    {
        if (GetInputDirection().x > 0)
            PlayerAnimator.SetTrigger("Right");
        else if (GetInputDirection().x < 0)
            PlayerAnimator.SetTrigger("Left");
        else if (GetInputDirection().y > 0)
            PlayerAnimator.SetTrigger("Up");
        else if (GetInputDirection().y < 0)
            PlayerAnimator.SetTrigger("Down");
        else
            PlayerAnimator.SetTrigger("Idle");

    }

    private void UpdateDashAbilityStat()
    {
        // Get the Ability stats from scriptable object.
        float DashCooldownReductionMultiplierPercentage = DashAbilityStats.DashInfo[DashAbilityStats.CurrentAbilityLevel].DashCooldownReductionPercentage;
        float DashDistanceMultiplierPercentage = DashAbilityStats.DashInfo[DashAbilityStats.CurrentAbilityLevel].DashDistanceMultiplierPercentage;
        float SpeedMultiplierPercentage = DashAbilityStats.DashInfo[DashAbilityStats.CurrentAbilityLevel].MovementSpeedMultiplierPercentage;

        // Update the ability values.
        DashScript.HasDashAbility = true;
        DashScript.MaxDashAbility = DashAbilityStats.DashInfo[DashAbilityStats.CurrentAbilityLevel].MaxAbility;
        DashScript.DashCooldownReductionMultiplier += Utility.GetMultiplierValueFromPercentage(DashCooldownReductionMultiplierPercentage);
        DashScript.DashDistanceMultiplier += Utility.GetMultiplierValueFromPercentage(DashDistanceMultiplierPercentage);
        SpeedMultiplier += Utility.GetMultiplierValueFromPercentage(SpeedMultiplierPercentage);
        

        DashAbilityStats.CurrentAbilityLevel++;
    }

    private void UpdateHealthAbilityStat()
    {
        // Get the Ability stats from scriptable object.
        float MaxHealthMultiplierPercentage = HealthAbilityStats.HealthInfo[HealthAbilityStats.CurrentAbilityLevel].MaxHealthMultiplierPercentage;
        float LifeStealMultiplierPercentage = HealthAbilityStats.HealthInfo[HealthAbilityStats.CurrentAbilityLevel].LifeStealMultiplierPercentage;
        float DamageReductionMultiplierPercentage = HealthAbilityStats.HealthInfo[(HealthAbilityStats.CurrentAbilityLevel)].DamageReductionMultiplierPercentage;

        HealthScript.MaxHealthMultiplier += Utility.GetMultiplierValueFromPercentage(MaxHealthMultiplierPercentage);
        HealthScript.LifeStealMultiplier += Utility.GetMultiplierValueFromPercentage(LifeStealMultiplierPercentage);
        HealthScript.DamageReductionMultiplier += Utility.GetMultiplierValueFromPercentage(DamageReductionMultiplierPercentage);
        HealthScript.IncreaseMaxHealth();
        HealthAbilityStats.CurrentAbilityLevel++;
    }

    public void TakeDamage(float damageAmount)
    {
        HealthScript.TakeDamage(damageAmount);
    }
}
