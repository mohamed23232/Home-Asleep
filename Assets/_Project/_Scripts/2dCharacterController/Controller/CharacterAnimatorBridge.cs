using UnityEngine;

// Drives the Animator from CharacterController2D state. Decouples animation from physics.
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController2D))]
public class CharacterAnimatorBridge : MonoBehaviour
{
    [SerializeField] private SpriteRenderer visual;

    // Pre-hashed parameter IDs — faster than string lookups every frame
    private static readonly int HSpeedId   = Animator.StringToHash("hSpeed");
    private static readonly int VSpeedId   = Animator.StringToHash("vSpeed");
    private static readonly int ExSpeedId  = Animator.StringToHash("exSpeed");
    private static readonly int GroundedId = Animator.StringToHash("grounded");
    private static readonly int DashingId  = Animator.StringToHash("dashing");
    private static readonly int OnWallId   = Animator.StringToHash("onWall");
    private static readonly int FacingId   = Animator.StringToHash("facingRight");
    private static readonly int PushingId  = Animator.StringToHash("pushing");
    private static readonly int JumpId     = Animator.StringToHash("jump");

    private Animator animator;
    private CharacterController2D character;

    void Start()
    {
        animator  = GetComponent<Animator>();
        character = GetComponent<CharacterController2D>();
        character.OnJumped += TriggerJumpAnimation;
    }

    void OnDestroy()
    {
        if (character) character.OnJumped -= TriggerJumpAnimation;
    }

    // LateUpdate runs after physics so state is always final
    void LateUpdate()
    {
        if (character.TotalSpeed.x != 0)
        {
            character.FacingRight = character.TotalSpeed.x > 0;
            if (visual) visual.flipX = !character.FacingRight;
        }

        animator.SetFloat(HSpeedId,  character.HorizontalSpeed);
        animator.SetFloat(VSpeedId,  character.TotalSpeed.y);
        animator.SetFloat(ExSpeedId, character.ExternalHorizontalSpeed);
        animator.SetBool(GroundedId, character.IsGrounded);
        animator.SetBool(DashingId,  character.Dashing);
        animator.SetBool(OnWallId,   character.IsOnWall);
        animator.SetBool(FacingId,   character.FacingRight);
        animator.SetBool(PushingId,  character.Pushing);
    }

    private void TriggerJumpAnimation()
    {
        animator.SetTrigger(JumpId);
    }
}
