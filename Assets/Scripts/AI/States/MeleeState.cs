using System;
using UnityEngine;
using UnityEngine.AI;

public class MeleeState : State
{
    [SerializeField] private float lostSightTime = 1f;
    [SerializeField] private float fireRate = 1f;
    
    private bool isExiting = false;

    
    void Awake()
    {
        base.Awake();
    }
    
    public override void OnStateEnter(bool bypassActivationCheck = false)
    {
        base.OnStateEnter(bypassActivationCheck);
        if (enabled)
        {
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
        base.OnStateExit();
        CancelInvoke(nameof(Attack));
    }

    private void Attack()
    {
        if (!Target) return;
        
        Debug.Log(name + " is attacking");
     
    }
    
}
