using UnityEngine;

public class SpecialJump : MonoBehaviour
{
    public Animator animator;

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        animator = player.GetComponentInChildren<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Star"))
        {
            animator.SetTrigger("StarHit");
        }
    }
}
