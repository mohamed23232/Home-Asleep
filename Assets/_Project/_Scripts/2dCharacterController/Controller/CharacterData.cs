using UnityEngine;

// Character stats. Create via Assets > Create > Game > Character Data, assign in CharacterController2D.
[CreateAssetMenu(fileName = "CharacterData", menuName = "Game/Character Data")]
public class CharacterData : ScriptableObject
{
    [Header("Movement")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float accelerationTime;
    [SerializeField] private float decelerationTime;
    [SerializeField] private bool canUseSlopes;

    [Header("Jumping")]
    [SerializeField] private int maxExtraJumps;
    [SerializeField] private float maxJumpHeight;
    [SerializeField] private float minJumpHeight;

    [Header("Wall Sliding/Jumping")]
    [SerializeField] private bool canWallSlide;
    [SerializeField] private float wallSlideSpeed;
    [SerializeField] private bool canWallJump;
    [SerializeField] private float wallJumpSpeed;

    [Header("Dashing")]
    [SerializeField] private bool canDash;
    [SerializeField] private bool omnidirectionalDash;
    [SerializeField] private bool dashDownSlopes;
    [SerializeField] private bool canJumpDuringDash;
    [SerializeField] private float dashDistance;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float maxDashCooldown;
    [SerializeField] private int maxAirDashes;

    public float MaxSpeed { get => maxSpeed; set => maxSpeed = value; }
    public float AccelerationTime => accelerationTime;
    public float DecelerationTime => decelerationTime;
    public bool CanUseSlopes => canUseSlopes;
    public int MaxExtraJumps { get => maxExtraJumps; set => maxExtraJumps = value; }
    public float MaxJumpHeight => maxJumpHeight;
    public float MinJumpHeight => minJumpHeight;
    public bool CanWallSlide { get => canWallSlide; set => canWallSlide = value; }
    public float WallSlideSpeed => wallSlideSpeed;
    public bool CanWallJump { get => canWallJump; set => canWallJump = value; }
    public float WallJumpSpeed => wallJumpSpeed;
    public bool CanDash { get => canDash; set => canDash = value; }
    public bool OmnidirectionalDash => omnidirectionalDash;
    public bool DashDownSlopes => dashDownSlopes;
    public bool CanJumpDuringDash => canJumpDuringDash;
    public float DashDistance => dashDistance;
    public float DashSpeed => dashSpeed;
    public float MaxDashCooldown => maxDashCooldown;
    public int MaxAirDashes { get => maxAirDashes; set => maxAirDashes = value; }
}