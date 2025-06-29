using System.Collections;
using System.Linq;
using AI;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(StateController))]
public class State : MonoBehaviour
{
    [SerializeField] private bool startState;
    [SerializeField] protected State prevState;
    [SerializeField] protected State nextState;


    protected GameObject Target
    {
        get => StateController.Target;
        set => StateController.Target = value;
    }

    protected StateController StateController;
    protected bool IsRotating = false;
    
    protected void Awake()
    {
        StateController = GetComponent<StateController>();

        if (startState)
        {
            OnStateEnter(true);
        }
        else
        {
            OnStateExit();
        }
    }

    public virtual void OnStateExit()
    {
        //Debug.Log("StateExit: " + GetType().Name, this);
        enabled = false;
    }

    public virtual void OnStateEnter(bool bypassActivationCheck = false)
    {
        if (!enabled || bypassActivationCheck)
        {
            //Debug.Log("StateEnter: " + GetType().Name, this);
            enabled = true;
        }
        else
        {
            //Debug.Log("State " + GetType().Name + " Already Enabled", this);
        }
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