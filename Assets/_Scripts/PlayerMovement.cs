using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float playerSpeed = 1f;

    public Transform GroundCheckTransform;
    public float GroundCheckRadius;
    public LayerMask GroundMask;
    public Transform PlayerSprite;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        transform.position += Vector3.right * playerSpeed * Time.deltaTime;
        if (OnGround())
        {
            Vector3 Rotation = PlayerSprite.rotation.eulerAngles;
            Rotation.z = Mathf.Round(Rotation.z / 90) * 90;
            PlayerSprite.rotation = Quaternion.Euler(Rotation);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.linearVelocity = Vector2.zero;
                rb.AddForce(Vector2.up * 26.6581f, ForceMode2D.Impulse);
            }
        }
        else
        {
            PlayerSprite.Rotate(Vector3.back * 500 * Time.deltaTime);
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
            // Hit a Spike
            Reset();
        }
        else if (collision.gameObject.CompareTag("Platform"))
        {
            // Hit side of Platform
            Vector2 contactNormal = collision.contacts[0].normal;
            if (contactNormal.x > 0.5 || contactNormal.x < -0.5)
            {
                Reset();
            }
        }
    }

    void Reset()
    {
        // Player Died or Won
        Debug.Log("You deeeead.");
    }
}