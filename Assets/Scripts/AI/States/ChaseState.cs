using System;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : State
{
    [SerializeField] private float lostSightTime = 5f;
    [SerializeField] private float walkRepeatRate = 1f;
    [SerializeField] private float fixedZ = 0f;
    private NavMeshAgent agent;
    private GameObject target;
    private NavMeshPath navMeshPath;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navMeshPath = new NavMeshPath();
        agent = GetComponent<NavMeshAgent>();
    }

    public override void OnStateEnter(bool bypassActivationCheck = false)
    {
        base.OnStateEnter(bypassActivationCheck);
        if (enabled)
        {
            Invoke(nameof(PrevState), lostSightTime);
            InvokeRepeating(nameof(Move), 0, walkRepeatRate);
        }
        
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        CancelInvoke(nameof(Move));
    }

    private void Update()
    {
        Vector3 newPosition = transform.position;
        newPosition.z = fixedZ;
        transform.position = newPosition;
    }

    private void Move()
    {
        if (!target) return;

        Vector3 destination = target.transform.position;

        if (agent.CalculatePath(destination, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
        {
            agent.SetDestination(destination);
        }
    }
    

    public override void ReceiveTrigger(string triggerName, bool enter, Collider other)
    {
        if (triggerName == "AttackZone" && other.CompareTag("Player"))
        {
            NextState();
        }
        else if (other.CompareTag("Player"))
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