using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class State : MonoBehaviour
{
    [SerializeField] protected bool startState;
    [SerializeField] protected State prevState;
    [SerializeField] protected State nextState;


    protected void Awake()
    {
        if (!startState)
        {
            OnStateExit();
            return;
        }

        OnStateEnter(true);
    }

    public virtual void OnStateExit()
    {
        Debug.Log("StateExit: " + GetType().Name, this);
        enabled = false;
    }

    public virtual void OnStateEnter(bool bypassActivationCheck = false)
    {
        if (!enabled || bypassActivationCheck)
        {
            Debug.Log("StateEnter: " + GetType().Name, this);
            enabled = true;
        }
        else
        {
            Debug.Log("State " + GetType().Name + " Already Enabled", this);
        }
    }

    public virtual void ReceiveTrigger(string triggerName, bool enter, Collider other)
    {
        Debug.Log(GetType().Name + " Has " + (enter ? "Gained " : "Lost ") + triggerName, this);
    }
    
    public void PrevState()
    {
        if (prevState)
        {
            OnStateExit();
            prevState.OnStateEnter();
        }
    }
    
    public void NextState()
    {
        if (nextState)
        {
            OnStateExit();
            nextState.OnStateEnter();
        }
    }
    
    
}