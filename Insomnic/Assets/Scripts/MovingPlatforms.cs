using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    public float speed = 2f; // movement speed
    public float distance;

    private Vector3 startPos;
    private int direction = 1;

    void Start()
    {
        // Remember where this object started
        startPos = transform.position;
    }

    void Update()
    {
        if (CompareTag("horizontal"))
        {
            transform.Translate(Vector3.right * speed * direction * Time.deltaTime);

            if (Mathf.Abs(transform.position.x - startPos.x) >= distance)
                direction *= -1;
        }
        else if (CompareTag("vertical"))
        {
            transform.Translate(Vector3.up * speed * direction * Time.deltaTime);

            if (Mathf.Abs(transform.position.y - startPos.y) >= distance)
                direction *= -1;
        }
    }
}

