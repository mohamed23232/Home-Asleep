using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimatorBridge : MonoBehaviour
{
    [SerializeField] private SpriteRenderer visual;

    private static readonly int HSpeedId = Animator.StringToHash("hSpeed");
    private static readonly int IsSleepingId = Animator.StringToHash("isSleeping");
    private static readonly int JumpId = Animator.StringToHash("jump");
    private static readonly int IsGroundedId = Animator.StringToHash("isGrounded");
    private static readonly int VSpeedId = Animator.StringToHash("vSpeed");

    private Animator animator;
    private CharacterController2D character;

    void Start()
    {
        animator = GetComponent<Animator>();
        character = GetComponentInParent<CharacterController2D>();

        if (animator == null)
            Debug.LogError("[CharacterAnimatorBridge] No Animator found on this GameObject.", this);
        if (animator != null && animator.runtimeAnimatorController == null)
            Debug.LogError("[CharacterAnimatorBridge] Animator has no controller assigned.", this);
        if (character == null)
            Debug.LogError("[CharacterAnimatorBridge] No CharacterController2D found in parent.", this);

        // Sync initial state — PlayerController starts awake
        SetSleeping(false);
        character.OnJumped += OnJumped;
    }

    void OnEnable()
    {
        PlayerController.OnSwitch += OnSwitch;
    }

    void OnDisable()
    {
        PlayerController.OnSwitch -= OnSwitch;
        if (character != null) character.OnJumped -= OnJumped;
    }

    private void OnSwitch(bool isAwake)
    {
        SetSleeping(!isAwake);
    }

    private void OnJumped()
    {
        animator.SetTrigger(JumpId);
    }

    private void SetSleeping(bool sleeping)
    {
        if (animator == null) return;
        animator.SetBool(IsSleepingId, sleeping);
    }

    void LateUpdate()
    {
        if (animator == null) return;

        if (character.TotalSpeed.x != 0)
        {
            character.FacingRight = character.TotalSpeed.x > 0;
            if (visual) visual.flipX = !character.FacingRight;
        }

        animator.SetFloat(HSpeedId, character.HorizontalSpeed);
        animator.SetBool(IsGroundedId, character.IsGrounded);
        animator.SetFloat(VSpeedId, character.TotalSpeed.y);
    }
}
