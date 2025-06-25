using System;
using UnityEngine;
using UnityEngine.AI;

public class RangedState : State
{
    [SerializeField] private float lostSightTime = 1f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private Transform projectileSpawnPoint;
    
    private ObjectPool _objectPool;
    private bool isExiting = false;
    private Animator _animator;

    
    void Awake()
    {
        _animator = gameObject.GetComponentInChildren<Animator>();
        _objectPool = gameObject.GetComponent<ObjectPool>();
        base.Awake();
    }
    
    public override void OnStateEnter(bool bypassActivationCheck = false)
    {
        base.OnStateEnter(bypassActivationCheck);
        if (enabled)
        {
            _animator.SetBool("IsAttacking", true);
            InvokeRepeating(nameof(Attack), fireRate, fireRate);
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
        _animator.SetBool("IsAttacking", false);
        CancelInvoke(nameof(Attack));
    }

    private void Attack()
    {
        if (!Target) return;
   
        GameObject bullet = _objectPool.GetPooledObject();
        if (bullet != null)
        {
            bullet.transform.position = projectileSpawnPoint.transform.position;
            bullet.SetActive(true);
            PlayerProjectile player = bullet.GetComponent<PlayerProjectile>();
            player.Owner = gameObject;
            player.Activate(gameObject, transform.forward.x);
        }
     
    }


}
