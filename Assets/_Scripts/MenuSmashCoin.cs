using UnityEngine;

public class MenuCoin : MonoBehaviour
{
    private Vector3 originalPosition;
    private Animator childAnim;
    private Collider2D childCol;
    private GameObject visualChild;

    void Awake()
    {
        originalPosition = transform.position;

        childAnim = GetComponentInChildren<Animator>();
        childCol = GetComponentInChildren<Collider2D>();

        if (childAnim != null) visualChild = childAnim.gameObject;
    }

    public void Respawn()
    {
        transform.position = originalPosition;

        if (visualChild != null)
        {
            visualChild.SetActive(true);

            if (childAnim != null)
            {
                childAnim.Rebind();
                childAnim.Play("CoinAnimation", 0, 0f);
                childAnim.ResetTrigger("PickedUp");
            }
        }

        if (childCol != null) childCol.enabled = true;
    }
}