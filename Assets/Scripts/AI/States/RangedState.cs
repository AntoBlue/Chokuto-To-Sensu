using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class RangedState : State
{
    [SerializeField] private float lostSightTime = 1f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private Transform projectileSpawnPoint;
    
    private ObjectPool _objectPool;
    private bool isExiting = false;
    private Animator _animator;

    
    new void Awake()
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
            HasOwner owner = bullet.GetComponent<HasOwner>();
            Debug.Log(Target.transform.position.x - transform.position.x);
            owner.Owner = gameObject;
            player.Activate(Mathf.Sign(Target.transform.position.x - transform.position.x));
        }
     
    }
    
}
