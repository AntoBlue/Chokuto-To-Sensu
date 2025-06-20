using System;
using UnityEngine;
using UnityEngine.AI;

public class MeleeState : State
{
    [SerializeField] private float lostSightTime = 1f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private GameObject MeleeAttack;
    private bool isExiting = false;
    private Animator animator;

    
    void Awake()
    {
        animator = gameObject.GetComponentInChildren<Animator>();
        MeleeAttack.GetComponent<MeleeDamage>().Owner = gameObject;
        base.Awake();
    }
    
    public override void OnStateEnter(bool bypassActivationCheck = false)
    {
        base.OnStateEnter(bypassActivationCheck);
        if (enabled)
        {
            animator.SetBool("IsAttacking", true);
            isExiting = true;
            Invoke(nameof(PrevState), lostSightTime);
            InvokeRepeating(nameof(Attack), 0, fireRate);
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
        animator.SetBool("IsAttacking", false);
        base.OnStateExit();
        CancelInvoke(nameof(Attack));
    }

    private void Attack()
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
