using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 15f;
    [SerializeField] private BoolVariable atDesk;

    [Header("Animation (assign sprites)")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    // Run / Idle frames: only Right, Up, Down are unique
    [SerializeField] private Sprite[] runRight = new Sprite[4];
    [SerializeField] private Sprite[] runUp = new Sprite[4];
    [SerializeField] private Sprite[] runDown = new Sprite[4];

    [SerializeField] private Sprite[] idleRight = new Sprite[2];
    [SerializeField] private Sprite[] idleUp = new Sprite[2];
    [SerializeField] private Sprite[] idleDown = new Sprite[2];

    [SerializeField] private float runFps = 8f;
    [SerializeField] private float idleFps = 2f;
    private int lastDirIndex = 3; // 0=Right,1=Up,2=Left,3=Down (default Down or whatever you prefer)

    private Rigidbody2D rb;
    private Vector2 target;
    private Vector2 velocity;

    private float animTimer;
    private int animFrame;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void MoveTo(Vector2 vec) => target = vec;

    private void FixedUpdate()
    {
        Vector2 toTarget = target - rb.position;
        float distance = toTarget.magnitude;

        const float epsilon = 0.01f;
        Vector2 dir = distance > epsilon ? (toTarget / distance) : Vector2.zero;

        // Arrival speed cap
        float vArrival = Mathf.Sqrt(2f * deceleration * Mathf.Max(distance, 0f));
        float desiredSpeed = distance > epsilon ? Mathf.Min(moveSpeed, vArrival) : 0f;

        Vector2 desiredVel = dir * desiredSpeed;
        velocity = Vector2.MoveTowards(velocity, desiredVel, acceleration * Time.fixedDeltaTime);

        Vector2 step = velocity * Time.fixedDeltaTime;
        if (distance <= epsilon || step.magnitude >= distance)
        {
            rb.MovePosition(target);
            velocity = Vector2.zero;
        }
        else
        {
            rb.MovePosition(rb.position + step);
        }

        // ---- Animation ----
        bool isMoving = velocity.sqrMagnitude > 0.0001f;

        int dirIndex;
        if (isMoving)
        {
            float facingAngle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            float wrapped = (facingAngle + 360f) % 360f;
            dirIndex = Mathf.FloorToInt(((wrapped + 45f) % 360f) / 90f); // 0..3
            lastDirIndex = dirIndex; // remember last non-zero direction
        }
        else
        {
            dirIndex = lastDirIndex; // reuse last direction for idle
        }

        Sprite[] frames = null;
        float fps = isMoving ? runFps : idleFps;
        bool flipX = false;

        switch (dirIndex)
        {
            case 0: // Right
                frames = isMoving ? runRight : idleRight;
                flipX = false;
                break;
            case 1: // Up
                frames = isMoving ? runUp : idleUp;
                flipX = false;
                break;
            case 2: // Left (reuse right, flip)
                frames = isMoving ? runRight : idleRight;
                flipX = true;
                break;
            case 3: // Down
                frames = isMoving ? runDown : idleDown;
                flipX = false;
                break;
        }

        spriteRenderer.flipX = flipX;

        // advance animation
        animTimer += Time.fixedDeltaTime;
        if (frames != null && frames.Length > 0 && animTimer >= (1f / fps))
        {
            animTimer -= 1f / fps;
            animFrame = (animFrame + 1) % frames.Length;
            spriteRenderer.sprite = frames[animFrame];
        }
        else if (frames != null && frames.Length > 0 && spriteRenderer.sprite == null)
        {
            spriteRenderer.sprite = frames[0];
            animFrame = 0;
            animTimer = 0f;
        }

        atDesk.Value = rb.position.y > 2f;
    }
}
