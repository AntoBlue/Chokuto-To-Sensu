using UnityEngine;
using UnityEngine.AI;

public class MeleeState : State
{
    [SerializeField] private float lostSightTime = 1f;
    [SerializeField] private float fireRate = 1f;
    private GameObject target;
    private NavMeshAgent agent;
    
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        base.Awake();
    }
    
    public override void OnStateEnter(bool bypassActivationCheck = false)
    {
        base.OnStateEnter(bypassActivationCheck);
        if (enabled)
        {
            agent.SetDestination(transform.position);
            agent.isStopped = true;
            Invoke(nameof(PrevState), lostSightTime);
            InvokeRepeating(nameof(Attack), 0, fireRate);
        }
        
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        CancelInvoke(nameof(Attack));
        agent.isStopped = false;
    }

    private void Attack()
    {
        if (!target) return;
        
        Debug.Log(name + " is attacking");
     
    }
    

    public override void ReceiveTrigger(string triggerName, bool enter, Collider other)
    {
        if (triggerName == "AttackZone")
        {
            if (other.CompareTag("Player"))
            {
                Physics.Raycast(transform.position, other.transform.position - transform.position, out RaycastHit hit);

                if (hit.collider && hit.collider.CompareTag("Player"))
                {
                    base.ReceiveTrigger(triggerName, enter, other);

                    if (enter)
                    {
                        target = other.gameObject;
                        CancelInvoke(nameof(PrevState));
                    }
                    else
                    {
                        target = null;
                        Invoke(nameof(PrevState), lostSightTime);
                    }
                }
            }
        }
        
    }
}
