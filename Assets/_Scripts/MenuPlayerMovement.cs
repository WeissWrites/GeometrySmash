using UnityEngine;

public class MenuPlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float playerSpeed = 7.8f;
    [SerializeField] float jumpForce = 19.5f;

    [Header("Seamless Loop Settings")]
    [SerializeField] float loopDistance = 50f;
    [SerializeField] Vector2 startPosition = new Vector2(-7.22f, -2.1f);

    [Header("References")]
    public MenuCameraFollow menuCamera;
    public Transform PlayerSprite;
    public Transform GroundCheckTransform;
    public float GroundCheckRadius = 0.12f;
    public LayerMask GroundMask;

    private Rigidbody2D rb;
    private bool isJumping;
    private float lastJumpTime;
    private float jumpCooldown = 0.2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        ResetToStart();
    }

    void Update()
    {
        if (transform.position.x >= startPosition.x + loopDistance)
        {
            ResetToStart();
        }

        bool grounded = OnGround();

        if (grounded)
        {
            if (Time.time > lastJumpTime + 0.1f)
            {
                isJumping = false;
                rb.angularVelocity = 0f;

                Vector3 rot = PlayerSprite.rotation.eulerAngles;
                rot.z = Mathf.Round(rot.z / 90) * 90;
                PlayerSprite.rotation = Quaternion.Euler(0, 0, rot.z);
            }
        }
        else
        {
            PlayerSprite.Rotate(Vector3.back * 570 * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(playerSpeed, rb.linearVelocity.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("JumpPoint") && OnGround() && !isJumping && Time.time > lastJumpTime + jumpCooldown)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

            isJumping = true;
            lastJumpTime = Time.time;
            PlayerSprite.rotation = Quaternion.identity;
        }
        if (other.CompareTag("SmashCoin"))
        {
            if (other.TryGetComponent(out Animator anim))
                anim.SetTrigger("PickedUp");

            if (other.TryGetComponent(out Collider2D col))
                col.enabled = false;

            StartCoroutine(HideCoinAfterDelay(other.gameObject, 1.1f));
        }
    }

    void ResetToStart()
    {
        transform.position = new Vector3(startPosition.x, startPosition.y, transform.position.z);
        rb.linearVelocity = new Vector2(playerSpeed, 0);
        rb.angularVelocity = 0f;
        PlayerSprite.rotation = Quaternion.identity;
        isJumping = false;

        if (menuCamera != null) menuCamera.ResetCamera();

        MenuCoin[] coins = FindObjectsByType<MenuCoin>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (MenuCoin coin in coins)
        {
            coin.Respawn();
        }
    }

    private System.Collections.IEnumerator HideCoinAfterDelay(GameObject coin, float delay)
    {
        yield return new WaitForSeconds(delay);
        coin.SetActive(false);
    }

    bool OnGround()
    {
        return Physics2D.OverlapCircle(GroundCheckTransform.position, GroundCheckRadius, GroundMask);
    }

    private void OnDrawGizmos()
    {
        if (GroundCheckTransform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(GroundCheckTransform.position, GroundCheckRadius);
        }
    }
}