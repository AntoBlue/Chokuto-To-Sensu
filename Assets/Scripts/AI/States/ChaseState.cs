using System;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : State
{
    [Header("Chase Settings")] [SerializeField]
    private float lostSightTime = 5f;
    
    [SerializeField] private float playerEnemyDistanceTolerance = 1f;

    [SerializeField] private float runSpeed = 4;
    private GameObject target;
    private Vector3 currentDirection;


    public override void OnStateEnter(bool bypassActivationCheck = false)
    {
        base.OnStateEnter(bypassActivationCheck);
        if (enabled)
        {
            Invoke(nameof(PrevState), lostSightTime);
        }
    }

    private void FixedUpdate()
    {
        if (target)
        {
            currentDirection = target.transform.position - transform.position;
            currentDirection.y = 0;

            if (!IsGrounded) return;

            if (!isRotating)
            {
                if (CanWalkForward && currentDirection.magnitude > playerEnemyDistanceTolerance)
                {
                    rb.MovePosition(currentDirection.normalized * (runSpeed * Time.fixedDeltaTime) + rb.position);
                }
            }
        }
    }

    public override void ReceiveTrigger(string triggerName, bool enter, Collider other)
    {
        if (triggerName == "AttackZone" && other.CompareTag("Player"))
        {
            NextState();
        }
        else if (other.CompareTag("Player"))
        {
            if (target)
            {
                float degresAngle = Vector3.Angle(transform.forward, target.transform.position - transform.position);

                if (degresAngle > sightDegrees && !isRotating)
                {
                    Debug.Log(degresAngle);
                    StartCoroutine(Rotate(Quaternion.Euler(0, -90 * transform.forward.x, 0)));
                }
            }

            if (Physics.Raycast(transform.position, other.transform.position - transform.position,
                    out RaycastHit hit) && hit.collider && hit.collider.CompareTag("Player"))
            {
                base.ReceiveTrigger(triggerName, enter, other);

                if (enter)
                {
                    target = other.gameObject;
                    CancelInvoke(nameof(PrevState));
                }
                else
                {
                    target = null;
                    Invoke(nameof(PrevState), lostSightTime);
                }
            }
        }
    }
}