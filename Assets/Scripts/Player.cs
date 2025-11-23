using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("Player")]
    [SerializeField]
    private float moveSpeed = 5.0f;

    [Header("Camera")]
    [SerializeField]
    private Camera eyes;
    [SerializeField]
    private Vector2 deadZoneSize = new Vector2(2.0f, 2.0f);
    [SerializeField]

    private Rigidbody2D rb;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void LateUpdate()
    {
        FollowPlayer();
    }

    public void OnMove(InputValue value)
    {
        Vector2 moveInput = value.Get<Vector2>();
        rb.linearVelocity = moveInput * moveSpeed;
    }

    private void FollowPlayer()
    {
        Vector3 camPos = eyes.transform.position;
        Vector3 playerPos = transform.position;

        // --- DEAD ZONE LOGIC ---
        Vector2 difference = playerPos - camPos;

        // Only move camera if player leaves dead zone on X
        if (Mathf.Abs(difference.x) > deadZoneSize.x / 2f)
        {
            camPos.x = Mathf.Lerp(
                camPos.x,
                playerPos.x - Mathf.Sign(difference.x) * (deadZoneSize.x / 2f),
                moveSpeed * Time.deltaTime
            );
        }

        // Only move camera if player leaves dead zone on Y
        if (Mathf.Abs(difference.y) > deadZoneSize.y / 2f)
        {
            camPos.y = Mathf.Lerp(
                camPos.y,
                playerPos.y - Mathf.Sign(difference.y) * (deadZoneSize.y / 2f),
                moveSpeed * Time.deltaTime
            );
        }

        // --- CLAMP CAMERA TO LEVEL BOUNDS ---
        float camHeight = eyes.orthographicSize;
        float camWidth = camHeight * eyes.aspect;

        camPos.x = Mathf.Clamp(
            camPos.x,
            LevelTilemap.Instance.GetMinBounds().x + camWidth,
            LevelTilemap.Instance.GetMaxBounds().x - camWidth
        );
        camPos.y = Mathf.Clamp(
            camPos.y,
            LevelTilemap.Instance.GetMinBounds().y + camHeight,
            LevelTilemap.Instance.GetMaxBounds().y - camHeight
        );

        // Apply final position
        eyes.transform.position = new Vector3(camPos.x, camPos.y, eyes.transform.position.z);
    }
}
