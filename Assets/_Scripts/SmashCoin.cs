using UnityEngine;

public class SmashCoin : MonoBehaviour
{
    public int coinIndex;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.CollectCoin(coinIndex);
        }
    }
    private void OnEnable()
    {
        Animator anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.Play("CoinAnimation", 0, 0f);
            anim.ResetTrigger("PickedUp");
        }
    }
}
