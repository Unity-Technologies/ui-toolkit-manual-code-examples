using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class RandomMovement : MonoBehaviour
{
    public float moveSpeed;

    public float movementRange;

    public Vector3 targetPosition;

    private float m_PositionY;

    // Initialize the starting position and target position of the GameObject.
    void Start()
    {
        m_PositionY = transform.position.y;
        targetPosition = new Vector3(0, m_PositionY, 0);
    }

    // Updates the position of the GameObject at fixed intervals.
    // Move the GameObject towards the target position, and sets a new random target position when the current target is reached.
    void FixedUpdate()
    {
        if (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        else
        {
            targetPosition = new Vector3(Random.Range(-movementRange, movementRange), m_PositionY, Random.Range(-movementRange, movementRange));
        }
    }
}
