using System;
using AI;
using UnityEngine;

public class AnimationAttackReciever : MonoBehaviour
{

    private I_Attack _attackState;

    private void Awake()
    {
        _attackState = GetComponentInParent<I_Attack>();
    }

    public void Attack()
    {
        _attackState.Attack();
    }
}
