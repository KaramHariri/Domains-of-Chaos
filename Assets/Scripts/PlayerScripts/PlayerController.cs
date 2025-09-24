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

    [Header("ThrowingSpearSettings")]
    [SerializeField] private float FireRate = 1.0f;
    [SerializeField] private GameObject SpearPrefab;
    private Vector2 DirectionToClosestEnemy = Vector2.zero;
    private float CurrentSpearCooldown = 0f;
    private float NextThrowingTime = 0f;

    [Header("ShieldSettings")]
    public int CurrentShieldCount = 0;      // Active orbiters (driven by ability level)
    public int MaxShieldCount = 0;
    public float Radius = 3f;
    public GameObject ShieldPrefab;
    public List<Shield> Shields = new List<Shield>();

    float CurrentAngle = 0f;
    public float OrbitSpeedDegPerSec = 90f;

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
        CurrentAngle = OrbitSpeedDegPerSec * Time.deltaTime;
        FindClosestEnemy();
        InputDirection = MovementAction.action.ReadValue<Vector2>();

        if(CanThrowSpear())
        {
            InstantiateSpear();
        }

        if (CurrentShieldCount != MaxShieldCount)
        {
            CreateShields();
            CurrentShieldCount = MaxShieldCount;
        }

        if (CurrentShieldCount != 0)
        {
            RotateShields();
        }

        if (AddXp == true)
        {
            ExperienceManager.Instance.AddExperience(expAmount);
            AddXp = false;
        }
    }

    private void FixedUpdate()
    {
        if (DashScript.IsCurrentlyDashing()) return;

        DashScript.DecreaseDashAbilityResetTimer();
        DashScript.ResetDashCooldowns();
        MovementScript.Move();
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

    private bool CanThrowSpear()
    {
        CurrentSpearCooldown += Time.deltaTime;
        NextThrowingTime = 1f / FireRate;
        if (CurrentSpearCooldown >= NextThrowingTime)
        {
            CurrentSpearCooldown = 0;
            return true;
        }
        return false;
    }

    private void InstantiateSpear()
    {
        GameObject spear = Instantiate(SpearPrefab, transform.position, Quaternion.identity);
        spear.GetComponent<Spear>().SetThrowDirection(DirectionToClosestEnemy);
    }

    public Vector2 GetInputDirection()
    {
        return InputDirection;
    }

    private void FindClosestEnemy()
    {
        float distanceToClosestEnemy = Mathf.Infinity;
        Enemy closestEnemy = null;
        Enemy[] allEnemies = GameObject.FindObjectsOfType<Enemy>();

        foreach(Enemy currentEnemy in allEnemies)
        {
            float distanceToCurrentEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;
            if (distanceToCurrentEnemy < distanceToClosestEnemy)
            {
                distanceToClosestEnemy = distanceToCurrentEnemy;
                closestEnemy = currentEnemy;
            }
        }
        DirectionToClosestEnemy = closestEnemy.transform.position - this.transform.position;
        DirectionToClosestEnemy.Normalize();
    }

    private void ClearShieldList()
    {
        foreach (var shield in Shields)
        {
            if (shield != null)
            {
                Destroy(shield.gameObject);
            }
        }
        Shields.Clear();
    }

    private void CreateShields()
    {
        ClearShieldList();
        float step = 360f / MaxShieldCount;

        for (int i = 0; i < MaxShieldCount; i++)
        {
            float angleDeg = i * step;
            float rad = angleDeg * Mathf.Deg2Rad;


            Vector3 offsetBetweenShield = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * Radius;
            Vector3 pos = transform.position + offsetBetweenShield;

            GameObject shieldObject = Instantiate(ShieldPrefab, pos, Quaternion.identity);
            Shield shieldScripts = shieldObject.GetComponent<Shield>();
            Shields.Add(shieldScripts);
        }
    }

    private void RotateShields()
    {
        foreach(var shield in Shields)
        {
            shield.RotateAroundPlayer(this.transform,CurrentAngle);
        }
    }
}
