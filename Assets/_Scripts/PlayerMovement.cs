using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float coyoteTime = 0.1f;
    [SerializeField] float jumpBufferTime = 0.1f;
    [SerializeField] float playerSpeed = 10.4f;

    public Transform GroundCheckTransform;
    public float GroundCheckRadius;
    public LayerMask GroundMask;
    public Transform PlayerSprite;
    public GameManager GameOver;
    public AudioManager audioManager;

    private Rigidbody2D rb;
    private float lastJumpTime;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private bool isJumping;
    public int SmashCoins = 0;

    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        bool grounded = OnGround();

        if (grounded)
        {
            coyoteTimeCounter = coyoteTime;

            if (Time.time > lastJumpTime + 0.1f)
            {
                isJumping = false;
            }

            Vector3 rot = PlayerSprite.rotation.eulerAngles;
            rot.z = Mathf.Round(rot.z / 90) * 90;
            PlayerSprite.rotation = Quaternion.Euler(rot);
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
            PlayerSprite.Rotate(Vector3.back * 500 * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

#if UNITY_EDITOR
        HandleDevTeleport();
#endif
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(playerSpeed, rb.linearVelocity.y);

        Vector2 rayStart = (Vector2)transform.position + new Vector2(0, 0.4f);
        RaycastHit2D wallHit = Physics2D.Raycast(rayStart, Vector2.right, 0.55f, GroundMask);
        Debug.DrawRay(rayStart, Vector2.right * 0.55f, Color.red);

        if (wallHit.collider != null && wallHit.collider.CompareTag("Platform"))
        {
            audioManager.PlaySFX(audioManager.death);
            GameOver.Restart();
        }

        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f && !isJumping)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 23f);

            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
            isJumping = true;
            lastJumpTime = Time.time;
        }
    }

    bool OnGround()
    {
        return Physics2D.OverlapCircle(GroundCheckTransform.position, GroundCheckRadius, GroundMask);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            audioManager.PlaySFX(audioManager.death);
            GameOver.Restart();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SmashCoin"))
        {
            SmashCoins++;
            audioManager.PlaySFX(audioManager.smashCoin);
            other.GetComponent<Animator>().SetTrigger("PickedUp");
            other.enabled = false;
            Destroy(other.gameObject, 1.1f);
            Debug.Log("You have " + SmashCoins + " Smash Coins!");
        }
        if (other.CompareTag("Goal"))
        {
            Debug.Log("Level Finished!");
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
            FindFirstObjectByType<EndLevelSequence>().StartFinishSequence(this.transform);

        }
    }

    void HandleDevTeleport()
    {
        if (Input.GetKeyDown(KeyCode.R)) TeleportToPoint(0);
        if (Input.GetKeyDown(KeyCode.Alpha1)) TeleportToPoint(1);
        if (Input.GetKeyDown(KeyCode.Alpha2)) TeleportToPoint(2);
        if (Input.GetKeyDown(KeyCode.Alpha3)) TeleportToPoint(3);
        if (Input.GetKeyDown(KeyCode.Alpha4)) TeleportToPoint(4);
        if (Input.GetKeyDown(KeyCode.Alpha5)) TeleportToPoint(5);
        if (Input.GetKeyDown(KeyCode.Alpha6)) TeleportToPoint(6);
        if (Input.GetKeyDown(KeyCode.Alpha7)) TeleportToPoint(7);
    }

    void TeleportToPoint(int index)
    {
        _DEVSpawnPoint[] points = FindObjectsByType<_DEVSpawnPoint>(FindObjectsSortMode.None);
        foreach (var point in points)
        {
            if (point.spawnIndex == index)
            {
                RaycastHit2D hit = Physics2D.Raycast(point.transform.position, Vector2.down, 20f, GroundMask);
                Vector3 finalPos = point.transform.position;

                if (hit.collider != null)
                {
                    finalPos = new Vector3(hit.point.x, hit.point.y + 0.5f, 0);
                }

                transform.position = finalPos;
                rb.linearVelocity = Vector2.zero;
                GameOver.SetNewSpawn(finalPos);
                return;
            }
        }
    }
}