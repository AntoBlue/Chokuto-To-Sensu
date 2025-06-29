using System;
using UnityEngine;
using UnityEngine.AI;


public class ChaseState : State
{
    [Header("Chase Settings")] [SerializeField]
    private float lostSightTime = 2f;

    [SerializeField] private float playerEnemyDistanceTolerance = 1f;

    [SerializeField] private float runSpeed = 4;
    

    private Vector3 currentDirection;

    private bool isExiting = false;

    private new void Awake()
    {
        base.Awake();
    }

    public override void OnStateEnter(bool bypassActivationCheck = false)
    {
        base.OnStateEnter(bypassActivationCheck);
        if (isActiveAndEnabled)
        {
            isExiting = true;
            Invoke(nameof(PrevState), lostSightTime);
        }
    }

    private void FixedUpdate()
    {
        if (Target)
        {
            currentDirection = Target.transform.position - transform.position;
            currentDirection.y = 0;

            if (!StateController.IsGrounded) return;

            float degresAngle = Vector3.Angle(transform.forward, Target.transform.position - transform.position);

            if (degresAngle > StateController.Settings.SightDegrees && !IsRotating)
            {
                StateController.StartRotation(Quaternion.Euler(0, -90 * transform.forward.x, 0));
            }

            if (!IsRotating)
            {
                if (StateController.CanWalkForward && currentDirection.magnitude > playerEnemyDistanceTolerance)
                {
                    StateController.InterpSpeedTo(1f);

                    StateController.Rb.MovePosition(currentDirection.normalized *
                        (runSpeed * Time.fixedDeltaTime * StateController.Animator.GetFloat("Blend")) + StateController.Rb.position);
                }
                else
                {
                    StateController.InterpSpeedTo(0f);
                }
            }
        }

        if (StateController.CanSeePlayer)
        {
            if (isExiting)
            {
                isExiting = false;
                CancelInvoke(nameof(PrevState));
            }
        }
        else if (!isExiting)
        {
            isExiting = true;
            Invoke(nameof(PrevState), lostSightTime);
        }

        if (StateController.CanAttackPlayer)
        {
            NextState();
        }
    }
}