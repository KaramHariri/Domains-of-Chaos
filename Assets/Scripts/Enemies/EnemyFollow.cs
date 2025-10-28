using UnityEngine;

public class EnemyFollow : MonoBehaviour, IDamagable
{
    #region Enemy Properties
    private float Health = 100f;
    private float MovementSpeed = 3.5f;
    private float Damage = 10f;
    private float Experience = 15f;
    #endregion

    #region References
    private Rigidbody2D Rigidbody;
    private SpriteRenderer SpriteRenderer;
    private GameObject Player;
    [SerializeField] GameObject ExperiencePrefab;
    private ObjectPooler ObjectPoolerInstance;
    #endregion

    private Vector2 DirectionToPlayer;

    private void Start()
    {
        ObjectPoolerInstance = ObjectPooler.Instance;
        Rigidbody = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Player = GameObject.FindGameObjectWithTag("Player");

    }

    private void OnEnable()
    {
        Health = Health * WaveSpawner.WaveDifficulty;
        Damage = Damage * WaveSpawner.WaveDifficulty;
        MovementSpeed = MovementSpeed * WaveSpawner.WaveDifficulty;
    }

    private void Update()
    {
        if (IsDead())
        {
            InstantiateExperienceDrop();
            ObjectPoolerInstance.ReturnToPool(gameObject.name, this.gameObject);
        }
        FlipSprite();
        if (GetVetorToPlayer().magnitude >= 1f)
        {
            MoveTowardsPlayer();
        }
    }

    private void FlipSprite()
    {
        if (GetVetorToPlayer().normalized.x > -0.1f)
        {
            SpriteRenderer.flipX = false;
        }
        else
        {
            SpriteRenderer.flipX = true;
        }
    }

    private void InstantiateExperienceDrop()
    {
        GameObject experience = ObjectPoolerInstance.SpawnFromPool("Experience", transform.position, Quaternion.identity);
        Experience experienceScript = ExperiencePrefab.GetComponent<Experience>();
        experienceScript.SetExperienceAmount(Experience);
    }

    private bool IsDead()
    {
        if (Health <= 0)
        {
            return true;
        }
        return false;
    }

    private void MoveTowardsPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, MovementSpeed * Time.deltaTime);
    }

    private Vector2 GetVetorToPlayer()
    {
        return Player.transform.position - transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<IDamagable>().TakeDamage(Damage);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        Health -= damageAmount;
    }
}
