using System.Collections;
using UnityEngine;

// Movement and physics for a character — walking, jumping, dashing, pushing.
public class CharacterController2D : ObjectController2D
{

    [SerializeField]
    private CharacterData cData;

    [SerializeField]
    private int extraJumps = 0;
    [SerializeField]
    private int airDashes = 0;
    private float dashCooldown = 0;

    public bool Immobile { get; set; }
    public bool Dashing { get; set; }
    public bool Pushing { get; private set; }

    public event System.Action OnJumped;

    public override void Start()
    {
        Dashing = false;
        base.Start();
    }

    public override void FixedUpdate()
    {
        Pushing = false;
        UpdateTimers();
        UpdateDash();
        collisions.Reset();
        Move((TotalSpeed) * Time.fixedDeltaTime);
        PostMove();
    }


    public override Vector2 Move(Vector2 deltaMove)
    {
        int layer = gameObject.layer;
        gameObject.layer = Physics2D.IgnoreRaycastLayer;
        PreMove(ref deltaMove);
        float xDir = Mathf.Sign(deltaMove.x);
        if (deltaMove.x != 0)
        {
            // Slope checks and processing
            if (deltaMove.y <= 0 && cData.CanUseSlopes)
            {
                if (collisions.onSlope)
                {
                    if (collisions.groundDirection == xDir)
                    {
                        if (!Dashing || cData.DashDownSlopes)
                        {
                            DescendSlope(ref deltaMove);
                        }
                    }
                    else
                    {
                        ClimbSlope(ref deltaMove);
                    }
                }
            }
            HorizontalCollisions(ref deltaMove);
        }
        if (collisions.hHit && cData.CanWallSlide && TotalSpeed.y <= 0)
        {
            externalForce.y = 0;
            speed.y = -cData.WallSlideSpeed;
        }
        if (collisions.onSlope && collisions.groundAngle >= minWallAngle &&
            collisions.groundDirection != xDir && speed.y < 0)
        {
            float sin = Mathf.Sin(collisions.groundAngle * Mathf.Deg2Rad);
            float cos = Mathf.Cos(collisions.groundAngle * Mathf.Deg2Rad);
            deltaMove.x = cos * cData.WallSlideSpeed * Time.fixedDeltaTime * collisions.groundDirection;
            deltaMove.y = sin * -cData.WallSlideSpeed * Time.fixedDeltaTime;
            speed.y = -cData.WallSlideSpeed;
            speed.x = 0;
            Vector2 origin = collisions.groundDirection == -1 ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
            collisions.hHit = Physics2D.Raycast(origin, Vector2.left * collisions.groundDirection,
                1f, collisionMask);
        }
        if (collisions.onGround && deltaMove.x != 0 && speed.y <= 0)
        {
            HandleSlopeChange(ref deltaMove);
        }
        if (deltaMove.y > 0 || (deltaMove.y < 0 && (!collisions.onSlope || deltaMove.x == 0)))
        {
            VerticalCollisions(ref deltaMove);
        }
#if UNITY_EDITOR
        Debug.DrawRay(transform.position, deltaMove * 3f, Color.green);
#endif
        transform.Translate(deltaMove);
        // Checks for ground and ceiling, resets jumps if grounded
        if (collisions.vHit)
        {
            if ((collisions.below && TotalSpeed.y < 0) || (collisions.above && TotalSpeed.y > 0))
            {
                if (collisions.below)
                {
                    ResetJumpsAndDashes();
                }
                if (!collisions.onSlope || collisions.groundAngle < minWallAngle)
                {
                    speed.y = 0;
                    externalForce.y = 0;
                }
            }
        }
        gameObject.layer = layer;
        return deltaMove;
    }

    protected override void VerticalCollisions(ref Vector2 deltaMove)
    {
        float directionY = Mathf.Sign(deltaMove.y);
        float rayLength = Mathf.Abs(deltaMove.y) + skinWidth;
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = directionY == -1 ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + deltaMove.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY,
                rayLength, collisionMask);
#if UNITY_EDITOR
            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);
#endif
            // for one way platforms
            if (ignorePlatformsTime <= 0 && directionY < 0 && !hit)
            {
                RaycastHit2D[] hits = Physics2D.RaycastAll(rayOrigin, Vector2.down,
                    rayLength, pConfig.owPlatformMask);
                foreach (RaycastHit2D h in hits)
                {
                    if (h.distance > 0)
                    {
                        hit = h;
                        continue;
                    }
                }
            }
            if (hit)
            {
                deltaMove.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;
                if (collisions.onSlope && directionY == 1)
                {
                    deltaMove.x = deltaMove.y / Mathf.Tan(collisions.groundAngle * Mathf.Deg2Rad) *
                        Mathf.Sign(deltaMove.x);
                    speed.x = 0;
                    externalForce.x = 0;
                }
                collisions.above = directionY > 0;
                collisions.below = directionY < 0;
                collisions.vHit = hit;

            }
        }
    }

    protected override void HandleSlopeChange(ref Vector2 deltaMove)
    {
        if (deltaMove.y <= 0 && Dashing && !cData.DashDownSlopes)
        {
            return;
        }
        else
        {
            base.HandleSlopeChange(ref deltaMove);
        }
    }

    protected override void UpdateGravity()
    {
        if (!Dashing)
        {
            base.UpdateGravity();
        }
    }

    protected override void UpdateExternalForce()
    {
        if (!Dashing)
        {
            base.UpdateExternalForce();
        }
    }

    public override void SetForce(Vector2 force)
    {
        base.SetForce(force);
        // cancels dash
        Dashing = false;
    }

    protected override bool OnHorizontalHit(RaycastHit2D hit, float directionX)
    {
        PushableObject pushable = hit.collider.GetComponent<PushableObject>();
        if (pushable != null && collisions.onGround)
        {
            // Box is against a wall — block the player like a normal wall
            if (pushable.IsBlocked)
            {
                Pushing = false;
                return false;
            }
            float playerForce = speed.x + externalForce.x;
            pushable.Push(playerForce);
            Pushing = true;
            // Clamp player's speed to the box speed so they can't outrun it
            // (takes effect next frame since deltaMove is already committed this frame)
            float boxSpeed = playerForce / Mathf.Max(pushable.pushResistance, 0.01f);
            speed.x = Mathf.Sign(playerForce) * Mathf.Min(Mathf.Abs(playerForce), Mathf.Abs(boxSpeed));
            externalForce.x = 0;
            return true;
        }
        return false;
    }



    public void Walk(float direction)
    {
        if (collisions.onSlope && collisions.groundAngle > maxSlopeAngle && collisions.groundAngle < minWallAngle)
        {
            direction = 0;
        }
        if (CanMove() && !Dashing)
        {
            float acc = cData.AccelerationTime;
            float dec = cData.DecelerationTime;
            if (acc > 0)
            {
                if (externalForce.x != 0 && Mathf.Sign(externalForce.x) != Mathf.Sign(direction))
                {
                    externalForce.x += direction * (1 / acc) * cData.MaxSpeed * Time.fixedDeltaTime;
                }
                else
                {
                    if (Mathf.Abs(speed.x) < cData.MaxSpeed)
                    {
                        speed.x += direction * (1 / acc) * cData.MaxSpeed * Time.fixedDeltaTime;
                        speed.x = Mathf.Min(Mathf.Abs(speed.x), cData.MaxSpeed * Mathf.Abs(direction)) *
                            Mathf.Sign(speed.x);
                    }
                }

            }
            else
            {
                speed.x = cData.MaxSpeed * direction;
            }
            if (direction == 0 || Mathf.Sign(direction) != Mathf.Sign(speed.x))
            {
                if (dec > 0)
                {
                    speed.x = Mathf.MoveTowards(speed.x, 0, (1 / dec) * cData.MaxSpeed * Time.fixedDeltaTime);
                }
                else
                {
                    speed.x = 0;
                }
            }
        }
    }



    public void Jump()
    {
        if (CanMove() && (!Dashing || cData.CanJumpDuringDash))
        {
            if (collisions.onGround || extraJumps > 0 || (cData.CanWallJump && collisions.hHit))
            {
                // air jump
                if (!collisions.onGround)
                {
                    extraJumps--;
                    externalForce = Vector2.zero;
                }
                float height = cData.MaxJumpHeight;
                speed.y = Mathf.Sqrt(-2 * pConfig.gravity * height);
                externalForce.y = 0;
                OnJumped?.Invoke();
                // wall jump
                if (cData.CanWallJump && collisions.hHit && !collisions.below)
                {
                    externalForce.x += collisions.left ? cData.WallJumpSpeed : -cData.WallJumpSpeed;
                    ResetJumpsAndDashes();
                }
                // slope sliding jump
                if (collisions.onSlope && collisions.groundAngle > maxSlopeAngle &&
                    collisions.groundAngle < minWallAngle)
                {
                    speed.x = cData.MaxSpeed * collisions.groundDirection;
                }
                ignorePlatformsTime = 0;
            }
        }
    }

    public void EndJump()
    {
        float yMove = Mathf.Sqrt(-2 * pConfig.gravity * cData.MinJumpHeight);
        if (speed.y > yMove)
        {
            speed.y = yMove;
        }
    }

    public void Dash(Vector2 direction)
    {
        if (CanMove() && cData.CanDash && dashCooldown <= 0)
        {
            if (!collisions.onGround)
            {
                if (airDashes > 0)
                {
                    airDashes--;
                }
                else
                {
                    return;
                }
            }
            Dashing = true;
            if (direction.magnitude == 0 || (collisions.onGround && direction.y < 0))
            {
                direction = FacingRight ? Vector2.right : Vector2.left;
            }
            // wall dash
            if (collisions.hHit)
            {
                direction = FacingRight ? Vector2.left : Vector2.right;
                ResetJumpsAndDashes();
            }
            if (!cData.OmnidirectionalDash)
            {
                direction = Vector2.right * Mathf.Sign(direction.x);
            }
            direction = direction.normalized * cData.DashSpeed;
            speed.x = 0;
            speed.y = 0;
            externalForce = direction;
            dashCooldown = cData.MaxDashCooldown;
            StartCoroutine(StopDashAfter(cData.DashDistance / cData.DashSpeed));
        }
    }

    private IEnumerator StopDashAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        Dashing = false;
    }

    public void JumpDown()
    {
        if (CanMove())
        {
            if (collisions.vHit && pConfig.owPlatformMask ==
                (pConfig.owPlatformMask | (1 << collisions.vHit.collider.gameObject.layer)))
            {
                IgnorePlatforms();
            }
            else
            {
                Jump();
            }
        }
    }

    private void IgnorePlatforms()
    {
        ignorePlatformsTime = owPlatformDelay;
    }


    public void ResetJumpsAndDashes()
    {
        extraJumps = cData.MaxExtraJumps;
        airDashes = cData.MaxAirDashes;
    }

    private void UpdateDash()
    {
        if (dashCooldown > 0)
        {
            dashCooldown -= Time.fixedDeltaTime;
        }
    }


    private void UpdateTimers()
    {
        if (ignorePlatformsTime > 0)
        {
            ignorePlatformsTime -= Time.fixedDeltaTime;
        }
    }

    public bool CanMove()
    {
        return !Immobile;
    }
}