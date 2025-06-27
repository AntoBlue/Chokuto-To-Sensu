using System;
using UnityEngine;
using UnityEngine.AI;


public class ChaseState : State
{
    [Header("Chase Settings")] [SerializeField]
    private float lostSightTime = 2f;

    [SerializeField] private float playerEnemyDistanceTolerance = 1f;

    [SerializeField] private float runSpeed = 4;
    
    private Animator animator;

    private Vector3 currentDirection;

    private bool isExiting = false;

    private new void Awake()
    {
        animator = gameObject.GetComponentInChildren<Animator>();
        base.Awake();
    }

    public override void OnStateEnter(bool bypassActivationCheck = false)
    {
        base.OnStateEnter(bypassActivationCheck);
        if (enabled)
        {
            animator.SetBool("IsRunning", true);
            isExiting = true;
            Invoke(nameof(PrevState), lostSightTime);
        }
    }

    public override void OnStateExit()
    {
        animator.SetBool("IsRunning", false);
        base.OnStateExit();
    }

    private void FixedUpdate()
    {
        if (Target)
        {
            currentDirection = Target.transform.position - transform.position;
            currentDirection.y = 0;

            if (!IsGrounded) return;

            float degresAngle = Vector3.Angle(transform.forward, Target.transform.position - transform.position);

            if (degresAngle > StateSettings.Settings.SightDegrees && !IsRotating)
            {
                StartCoroutine(Rotate(Quaternion.Euler(0, -90 * transform.forward.x, 0)));
            }

            if (!IsRotating)
            {
                if (CanWalkForward && currentDirection.magnitude > playerEnemyDistanceTolerance)
                {
                    Rb.MovePosition(currentDirection.normalized * (runSpeed * Time.fixedDeltaTime) + Rb.position);
                }
            }
        }

        if (CanSeePlayer)
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

        if (CanAttackPlayer)
        {
            NextState();
        }
    }

   
}