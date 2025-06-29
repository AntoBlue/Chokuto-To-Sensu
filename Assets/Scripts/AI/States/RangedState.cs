using System;
using AI;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class RangedState : State, I_Attack
{
    [SerializeField] private float lostSightTime = 1f;
    [SerializeField] private Transform projectileSpawnPoint;
    
    private ObjectPool _objectPool;
    private bool isExiting = false;

    
    new void Awake()
    {
        _objectPool = gameObject.GetComponent<ObjectPool>();
        base.Awake();
    }
    
    public override void OnStateEnter(bool bypassActivationCheck = false)
    {
        base.OnStateEnter(bypassActivationCheck);
        if (isActiveAndEnabled)
        {
            StateController.Animator.SetBool("IsAttacking", true);
        }
        
    }

    private void FixedUpdate()
    {
        
        if (StateController.CanAttackPlayer)
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
        StateController.Animator.SetBool("IsAttacking", false);
    }

    public void Attack()
    {
        if (!Target) return;
   
        GameObject bullet = _objectPool.GetPooledObject();
        if (bullet != null)
        {
            bullet.transform.position = projectileSpawnPoint.transform.position;
            bullet.SetActive(true);
            PlayerProjectile player = bullet.GetComponent<PlayerProjectile>();
            HasOwner owner = bullet.GetComponent<HasOwner>();
            Debug.Log(Target.transform.position.x - transform.position.x);
            owner.Owner = gameObject;
            player.Activate(Mathf.Sign(Target.transform.position.x - transform.position.x));
        }
     
    }
    
}
