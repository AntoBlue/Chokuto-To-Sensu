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

        if (!isRotating)
        {
            if (!CanWalkForward)
            {
                StartCoroutine(Rotate(Quaternion.Euler(0, -90 * transform.forward.x, 0)));
            }
            else
            {
                rb.MovePosition(direction.normalized * (walkSpeed * Time.fixedDeltaTime) + rb.position);
            }
        }
    }


    public override void ReceiveTrigger(string triggerName, bool enter, Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 RayDirection = (other.transform.position - transform.position).normalized;

            float degresAngle = Vector3.Angle(transform.forward, RayDirection);

            if (degresAngle <= sightDegrees)
            {
                if (Physics.Raycast(transform.position, RayDirection,
                        out RaycastHit hit) && hit.collider && hit.collider.CompareTag("Player"))
                {
                    Debug.DrawRay(transform.position, RayDirection,
                        Color.red);

                    base.ReceiveTrigger(triggerName, enter, other);
                    if (enter)
                    {
                        NextState();
                    }
                }
            }
        }
    }
}