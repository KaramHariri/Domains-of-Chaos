using UnityEngine;
using UnityEngine.EventSystems;

public class Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;
    private float speedMultiplierPercentage = 1f;
    private float speedMultiplier = 1f;
    private Vector2 MoveDirection = Vector2.zero;

    public void Move()
    {
        MoveDirection = PlayerController.Instance.GetInputDirection();
        PlayerController.Instance.Rb.linearVelocity = new Vector2(MoveDirection.x * moveSpeed, MoveDirection.y * moveSpeed);
    }
}
