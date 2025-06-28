using System;
using AI;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.AI;

public class MeleeState : State, I_Attack
{
    [SerializeField] private float lostSightTime = 1f;
    [SerializeField] private GameObject MeleeAttack;
    private bool isExiting = false;
    
    new void Awake()
    {
        MeleeAttack.GetComponent<HasOwner>().Owner = gameObject;
        base.Awake();
    }
    
    public override void OnStateEnter(bool bypassActivationCheck = false)
    {
        base.OnStateEnter(bypassActivationCheck);
        if (enabled)
        {
            _animator.SetBool("IsAttacking", true);
            isExiting = true;
            Invoke(nameof(PrevState), lostSightTime);
        }
        
    }

    private void FixedUpdate()
    {
        
        if (CanAttackPlayer)
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
    }

    public override void OnStateExit()
    {
        _animator.SetBool("IsAttacking", false);
        base.OnStateExit();
    }

    public void Attack()
    {
        if (!Target) return;
        
        MeleeAttack.SetActive(true);
        Invoke(nameof(DeactivateMelee), 0.25f);
     
    }

    private void DeactivateMelee()
    {
        MeleeAttack.SetActive(false);
    }
    
}
