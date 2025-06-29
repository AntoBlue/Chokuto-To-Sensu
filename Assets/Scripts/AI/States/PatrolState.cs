using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : State
{
    [Header("Patrol Settings")] [SerializeField]
    private float walkSpeed = 2;

    public override void OnStateEnter(bool bypassActivationCheck = false)
    {
        base.OnStateEnter(bypassActivationCheck);
        if (isActiveAndEnabled)
        {
            StateController.InterpSpeedTo(0.5f);
        }
    }

    private void FixedUpdate()
    {
        if (!StateController.IsGrounded) return;

        if (!IsRotating)
        {
            if (!StateController.CanWalkForward)
            {
                StateController.StartRotation(Quaternion.Euler(0, -90 * transform.forward.x, 0));
            }
            else
            {
                StateController.Rb.MovePosition(StateController.direction.normalized * (walkSpeed * Time.fixedDeltaTime * StateController.Animator.GetFloat("Blend")) + StateController.Rb.position);
            }
        }

        if (StateController.CanSeePlayer)
        {
            NextState();
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Projectile") && other.gameObject.GetComponent<HasOwner>() is { } owner)
        {
            Target = owner.gameObject;
            NextState();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile") && other.gameObject.GetComponent<HasOwner>() is { } owner)
        {
            Target = owner.gameObject;
            NextState();
        }
    }
    
}