using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : State
{
    [SerializeField] private float walkRadius = 3f;
    [SerializeField] private float walkRepeatRate = 2f;
    [SerializeField] private float fixedZ = 0f;
    private NavMeshAgent agent;
    private NavMeshPath navMeshPath;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navMeshPath = new NavMeshPath();
        agent = GetComponent<NavMeshAgent>();
    }
    
    private void Update()
    {
        Vector3 newPosition = transform.position;
        newPosition.z = fixedZ;
        transform.position = newPosition;
    }

    public override void OnStateEnter(bool bypassActivationCheck = false)
    {
        base.OnStateEnter(bypassActivationCheck);
        if (enabled)
        {
            InvokeRepeating(nameof(Move), 0, walkRadius);
        }
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        CancelInvoke(nameof(Move));
    }

    private void Move()
    {
        int tryNum = 0;
        while (tryNum <= 10)
        {
            tryNum++;
            Vector3 destination = RandomNavmeshLocation(walkRadius);

            if (destination != Vector3.zero && Vector3.Distance(destination, transform.position) > 1f &&
                agent.CalculatePath(destination, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
            {
                agent.SetDestination(destination);
                break;
            }
        }
    }

    private Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        randomDirection.z = fixedZ;

        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
            finalPosition.z = fixedZ;
        }

        return finalPosition;
    }

    public override void ReceiveTrigger(string triggerName, bool enter, Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Physics.Raycast(transform.position, other.transform.position - transform.position, out RaycastHit hit);

            if (hit.collider && hit.collider.CompareTag("Player"))
            {
                base.ReceiveTrigger(triggerName, enter, other);
                if (enter)
                {
                    NextState();
                }
            }
        }
    }
}