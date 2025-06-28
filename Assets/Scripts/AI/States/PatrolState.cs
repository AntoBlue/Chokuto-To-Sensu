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
        if (enabled)
        {
            StopCoroutine(nameof(InterpSpeedTo));
            StartCoroutine(InterpSpeedTo(0.5f));
        }
    }

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
                Rb.MovePosition(StateSettings.direction.normalized * (walkSpeed * Time.fixedDeltaTime * _animator.GetFloat("Blend")) + Rb.position);
            }
        }

        if (CanSeePlayer)
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