using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [HideInInspector] public static PlayerController Instance;

    //Input Actions
    public InputActionReference MovementAction;
    public InputActionReference DashAction;

    
    private Dash DashScript;
    private Movement MovementScript;
    
    private Vector2 InputDirection;
    [HideInInspector] public Rigidbody2D Rb;

    int expAmount = 10;
    public bool AddXp = false;

    void Start()
    {
        Instance = this;
        Rb = GetComponent<Rigidbody2D>();
        DashScript = GetComponent<Dash>();
        MovementScript = GetComponent<Movement>();
        AddXp = false;
    }

    
    void Update()
    {
        DashScript.DecreaseDashAbilityResetTimer();
        DashScript.ResetDashCooldowns();
        if (DashScript.IsCurrentlyDashing()) return;
        InputDirection = MovementAction.action.ReadValue<Vector2>();
        
        MovementScript.Move();
        if (AddXp == true)
        {
            ExperienceManager.Instance.AddExperience(expAmount);
            AddXp = false;
        }
    }

    private void DashCallback(InputAction.CallbackContext context)
    {
        DashScript.DashCallback();
    }

    private void OnEnable()
    {
        DashAction.action.started += DashCallback;
    }

    private void OnDisable()
    {
        DashAction.action.started -= DashCallback;
    }

    public Vector2 GetInputDirection()
    {
        return InputDirection.normalized;
    }

}
