using UnityEngine;

/// <summary>
/// Handles player movement using WASD keys and tracks facing direction.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float MoveSpeed = 5f;

    [Header("Health")]
    public int MaxHealth = 500;
    public int CurrentHealth;

    [Header("Shield")]
    public float ShieldHealth = 0f;
    public float ShieldDecayRate = 5f;

    public Vector3 FacingDirection { get; private set; } = Vector3.right;
    public bool IsAlive => CurrentHealth > 0;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        CurrentHealth = MaxHealth;
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentState != GameManager.GameState.Playing) return;
        HandleMovementInput();
        DecayShield();
    }

    private void FixedUpdate() { ApplyMovement(); }

    private void HandleMovementInput()
    {
        moveInput = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) moveInput.y += 1f;
        if (Input.GetKey(KeyCode.S)) moveInput.y -= 1f;
        if (Input.GetKey(KeyCode.A)) moveInput.x -= 1f;
        if (Input.GetKey(KeyCode.D)) moveInput.x += 1f;
        moveInput = moveInput.normalized;
        if (moveInput.sqrMagnitude > 0.01f)
            FacingDirection = new Vector3(moveInput.x, moveInput.y, 0f).normalized;
    }

    private void ApplyMovement()
    {
        if (rb != null) rb.linearVelocity = moveInput * MoveSpeed;
        else transform.position += (Vector3)(moveInput * MoveSpeed * Time.fixedDeltaTime);
    }

    private void DecayShield()
    {
        if (ShieldHealth > 0f)
        {
            ShieldHealth -= ShieldDecayRate * Time.deltaTime;
            ShieldHealth = Mathf.Max(0f, ShieldHealth);
        }
    }

    public void AddShield(float amount) { ShieldHealth += amount; }

    public void TakeDamage(int damage)
    {
        if (!IsAlive) return;
        float remainingDamage = damage;
        if (ShieldHealth > 0f)
        {
            float absorbed = Mathf.Min(ShieldHealth, remainingDamage);
            ShieldHealth -= absorbed;
            remainingDamage -= absorbed;
        }
        CurrentHealth -= Mathf.RoundToInt(remainingDamage);
        CurrentHealth = Mathf.Max(0, CurrentHealth);
        if (!IsAlive) GameManager.Instance.OnPlayerDeath();
    }

    public Vector3 GetPosition() { return transform.position; }
}
