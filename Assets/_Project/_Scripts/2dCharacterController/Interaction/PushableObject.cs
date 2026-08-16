using UnityEngine;

// Physics-driven box that can be pushed horizontally by walking into it.
public class PushableObject : ObjectController2D
{
    [Tooltip("How much the push force is reduced. 1 = same speed, 2 = half speed, etc.")]
    public float pushResistance = 1f;

    // True if touching a wall — used to block the pusher
    public bool IsBlocked => collisions.left || collisions.right;

    public void Push(float force)
    {
        speed.x = force / Mathf.Max(pushResistance, 0.01f);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        // Friction decelerates the box when not being pushed
        if (collisions.onGround)
        {
            speed.x = Mathf.MoveTowards(speed.x, 0, pConfig.groundFriction * Time.fixedDeltaTime);
        }
        if (collisions.left || collisions.right)
        {
            speed.x = 0;
        }
    }
}
