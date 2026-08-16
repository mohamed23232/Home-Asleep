using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This script controls the movement and crumbling of platforms.
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider2D))]
public class PlatformController : MonoBehaviour
{
    private const float WAYPOINT_REACH_THRESHOLD = 0.00001f;

    [SerializeField] private PlatformWaypoint currentWaypoint;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float accelerationDistance;
    [SerializeField] private float decelerationDistance;
    [SerializeField] private float waitTime;
    [SerializeField] private float crumbleTime;
    [SerializeField] private float restoreTime;
    [SerializeField] private bool onlyPlayerCrumble;

    [SerializeField]
    private Vector2 speed = Vector2.zero;
    private float currentWaitTime = 0;
    private float currentCrumbleTime = 0;
    private float currentRestoreTime = 0;
    private bool crumbled = false;
    private List<ObjectController2D> objs = new List<ObjectController2D>();
    private Animator animator;
    private Collider2D myCollider;
    private PhysicsConfig pConfig;

    private static readonly string ANIMATION_CRUMBLING = "crumbling";
    private static readonly string ANIMATION_CRUMBLE = "crumble";
    private static readonly string ANIMATION_RESTORE = "restore";

    void Start()
    {
        animator = GetComponent<Animator>();
        myCollider = GetComponent<Collider2D>();
        pConfig = PhysicsConfig.Instance;
        if (!pConfig)
        {
            pConfig = (PhysicsConfig)new GameObject().AddComponent(typeof(PhysicsConfig));
            pConfig.gameObject.name = "Physics Config";
            Debug.LogWarning("PhysicsConfig not found on the scene! Using default config.");
        }
    }

    void FixedUpdate()
    {
        if (crumbled)
        {
            if (currentRestoreTime > 0)
            {
                currentRestoreTime -= Time.fixedDeltaTime;
                if (currentRestoreTime <= 0)
                {
                    Restore();
                }
            }
        }
        else
        {
            if (currentCrumbleTime > 0)
            {
                currentCrumbleTime -= Time.fixedDeltaTime;
                if (currentCrumbleTime <= 0)
                {
                    crumbled = true;
                    animator.SetTrigger(ANIMATION_CRUMBLE);
                    myCollider.enabled = false;
                    if (restoreTime > 0)
                    {
                        currentRestoreTime = restoreTime;
                    }
                }
            }
            if (currentWaypoint)
            {
                if (currentWaitTime > 0)
                {
                    currentWaitTime -= Time.fixedDeltaTime;
                    return;
                }
                Vector2 distance = currentWaypoint.transform.position - transform.position;
                float distMag = distance.magnitude;
                float speedMag = speed.magnitude;
                if (distMag <= decelerationDistance)
                {
                    if (distMag > 0)
                    {
                        speed -= Time.fixedDeltaTime * distance.normalized * maxSpeed * maxSpeed /
                            (2 * decelerationDistance);
                    }
                    else
                    {
                        speed = Vector2.zero;
                    }
                }
                else if (speedMag < maxSpeed)
                {
                    if (accelerationDistance > 0)
                    {
                        speed += Time.fixedDeltaTime * distance.normalized * maxSpeed * maxSpeed /
                            (2 * accelerationDistance);
                    }
                    speedMag = speed.magnitude;
                    if (speedMag > maxSpeed || accelerationDistance <= 0)
                    {
                        speed = distance.normalized * maxSpeed;
                        speedMag = maxSpeed;
                    }
                }
                Vector3 newPos = Vector2.MoveTowards(transform.position, currentWaypoint.transform.position,
                    speedMag * Time.fixedDeltaTime);
                Vector2 velocity = newPos - transform.position;
                if (speed.y > 0)
                {
                    MoveObjects(velocity);
                    transform.position = newPos;
                }
                else
                {
                    transform.position = newPos;
                    MoveObjects(velocity);
                }
                distance = currentWaypoint.transform.position - transform.position;
                if (distance.magnitude < WAYPOINT_REACH_THRESHOLD)
                {
                    speed = Vector2.zero;
                    currentWaypoint = currentWaypoint.nextWaipoint;
                    currentWaitTime = waitTime;
                }
            }
        }
    }

    private void MoveObjects(Vector2 velocity)
    {
        for (int i = 0; i < objs.Count; i++)
        {
            objs[i].Move(velocity);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        AttachObject(other);
    }

    private void AttachObject(Collider2D other)
    {
        if (crumbled)
        {
            return;
        }
        ObjectController2D obj = other.GetComponent<ObjectController2D>();
        if (obj && !objs.Contains(obj))
        {
            if (pConfig.owPlatformMask == (pConfig.owPlatformMask | (1 << gameObject.layer)) &&
                (obj.transform.position.y < transform.position.y || obj.TotalSpeed.y > 0))
            {
                return;
            }
            else
            {
                objs.Add(obj);
                if (crumbleTime > 0 && currentCrumbleTime <= 0)
                {
                    if (!onlyPlayerCrumble || obj.GetComponent<PlayerController>())
                    {
                        currentCrumbleTime = crumbleTime;
                        animator.SetTrigger(ANIMATION_CRUMBLING);
                    }
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        ObjectController2D obj = other.GetComponent<ObjectController2D>();
        if (obj && objs.Contains(obj))
        {
            objs.Remove(obj);
            obj.ApplyForce(speed);
        }
    }

    public void Restore()
    {
        crumbled = false;
        myCollider.enabled = true;
        animator.SetTrigger(ANIMATION_RESTORE);
    }
}