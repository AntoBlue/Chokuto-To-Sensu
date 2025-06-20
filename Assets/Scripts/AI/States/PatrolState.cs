using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : State
{
    [Header("Patrol Settings")]
    [SerializeField] private float walkSpeed = 2;
    
    
    private void FixedUpdate()
    {

        if (!IsGrounded) return;

        if (!IsRotating)
        {
            if (!CanWalkForward)
            {
                StartCoroutine(Rotate(Quaternion.Euler(0, -90 * transform.forward.x, 0)));
            }
            else
            {
                Rb.MovePosition(StateSettings.direction.normalized * (walkSpeed * Time.fixedDeltaTime) + Rb.position);
            }
        }

        if (CanSeePlayer)
        {
            NextState();
        }
    }
    
}