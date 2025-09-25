using UnityEngine;

public class Spear : MonoBehaviour
{
    [Header("ThrowingSpearSettings")]
    [SerializeField] private float FireRate = 1.0f;
    [SerializeField] private float Speed = 20f;
    private Vector2 ThrowDirection = Vector2.zero;
    private Rigidbody2D Rb;


    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Rb.linearVelocity = ThrowDirection * Speed;
    }

    public void SetThrowDirection(Vector2 dir)
    {
        ThrowDirection = dir;
    }

   

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            //Debug.Log("Hit");
            Destroy(gameObject);
        }
        
    }
}
