using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class State : MonoBehaviour
{
    [Header("State Settings")]
    [SerializeField] protected bool startState;
    [SerializeField] protected State prevState;
    [SerializeField] protected State nextState;
    
    [SerializeField] protected float rotationTime = 0.5f;
    [SerializeField] protected Vector3 direction = new Vector3(-1, 0, 0);
    [SerializeField] protected float sightDegrees = 90;

    
    
    [Header("Depth Check")]
    [SerializeField] protected float depthTestWall = 0.4f;
    [SerializeField] protected LayerMask wallLayer;
    [SerializeField] protected float depthTestFloor = 1.6f;
    [SerializeField] protected float depthTestGround = 2;
    [SerializeField] protected LayerMask groundFloorLayer;
    [SerializeField] protected float floorCheckRotation = -55;
    
    
    protected Rigidbody rb;
    protected bool isRotating = false;
    
    protected YieldInstruction waitFor = new WaitForFixedUpdate();

    protected bool IsGrounded
    {
        get
        {
            Debug.DrawLine(rb.position, rb.position + Vector3.down * (depthTestGround * 0.6f), Color.green);
            return Physics.Raycast(rb.position, Vector3.down, out RaycastHit hit, depthTestGround * 0.6f,
                groundFloorLayer);
        }
    }


    protected bool CanWalkForward
    {
        get
        {
            Debug.DrawLine(rb.position, rb.position + transform.forward * depthTestWall,
                Color.green);
            if (!Physics.Raycast(rb.position, transform.forward, out RaycastHit wall, depthTestWall, wallLayer))
            {
                Vector3 rot = Quaternion.Euler(0, 0, floorCheckRotation * transform.forward.x) * transform.forward;
                Debug.DrawLine(rb.position, rb.position +
                                            (rot) * depthTestFloor,
                    Color.green);
                return Physics.Raycast(rb.position, (rot), out RaycastHit floor, depthTestFloor, groundFloorLayer);
            }

            return false;
        }
    }
    
    protected IEnumerator Rotate(Quaternion desiredRotation)
    {
        isRotating = true;
        Quaternion orgRotation = transform.rotation;
        float time = 0;
        
        while (time < rotationTime)
        {
            time += Time.fixedDeltaTime;

            transform.rotation =
                Quaternion.Slerp(orgRotation, desiredRotation,
                    time / rotationTime);
            yield return waitFor;
        }

        direction.x *= -1;
        transform.rotation = desiredRotation;
        isRotating = false;
    }


    protected void Awake()
    {
        rb = GetComponent<Rigidbody>();

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