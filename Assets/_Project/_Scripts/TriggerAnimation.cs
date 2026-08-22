using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
    private Animator animator;
    private CharacterController controller;

    private void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpecialStart")) // your tag
        {
            animator.SetTrigger("specialJump");
        }
    }

    void Update()
    {

    }
}
