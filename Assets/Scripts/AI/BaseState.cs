using System.Collections;
using System.Linq;
using AI;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(StateSettings))]
public class State : MonoBehaviour
{
    [SerializeField] private bool startState;
    [SerializeField] protected State prevState;
    [SerializeField] protected State nextState;
    

    protected GameObject Target;
    protected StateSettings StateSettings;
    protected Rigidbody Rb;
    protected bool IsRotating = false;

    protected bool IsGrounded
    {
        get
        {
            Debug.DrawLine(Rb.position, Rb.position + Vector3.down * (StateSettings.Settings.DepthTestGround * 0.6f),
                Color.green);
            return Physics.Raycast(Rb.position, Vector3.down, out RaycastHit hit,
                StateSettings.Settings.DepthTestGround * 0.6f,
                StateSettings.Settings.GroundFloorLayer);
        }
    }

    protected bool CanSeePlayer
    {
        get { return SphereCast(StateSettings.Settings.SightRadius, Color.red); }
    }

    protected bool CanAttackPlayer
    {
        get { return SphereCast(StateSettings.Settings.DamageRadius, Color.yellow); }
    }


    protected bool CanWalkForward
    {
        get
        {
            Debug.DrawLine(Rb.position, Rb.position + transform.forward * StateSettings.Settings.DepthTestWall,
                Color.green);
            if (!Physics.Raycast(Rb.position, transform.forward, out RaycastHit wall,
                    StateSettings.Settings.DepthTestWall, StateSettings.Settings.WallLayer))
            {
                Vector3 rot = Quaternion.Euler(0, 0, StateSettings.Settings.FloorCheckRotation * transform.forward.x) *
                              transform.forward;
                Debug.DrawLine(Rb.position, Rb.position +
                                            (rot) * StateSettings.Settings.DepthTestFloor,
                    Color.green);
                return Physics.Raycast(Rb.position, (rot), out RaycastHit floor, StateSettings.Settings.DepthTestFloor,
                    StateSettings.Settings.GroundFloorLayer);
            }

            return false;
        }
    }

    private bool SphereCast(float traceRadius, Color color)
    {
        Debug.DrawLine(transform.position - transform.forward * traceRadius, transform.position + transform.forward * traceRadius,
            color);
        Collider[] hits = Physics.OverlapSphere(transform.position, traceRadius);
        Collider hit = hits.FirstOrDefault(hit => hit.gameObject.CompareTag("Player"));
        
        if (hit)
        {
            Vector3 rayDirection = (hit.transform.position - StateSettings.EyeTransform.position).normalized;
            
            float degresAngle = Vector3.Angle(transform.forward, rayDirection);

            if (degresAngle <= StateSettings.Settings.SightDegrees)
            {
                Debug.DrawLine(StateSettings.EyeTransform.position, transform.position + rayDirection * traceRadius,
                    Color.magenta);
                if (Physics.Raycast(StateSettings.EyeTransform.position, rayDirection, out RaycastHit playerTest,
                        traceRadius) && playerTest.collider.CompareTag("Player"))
                {
                    Target = hit.gameObject;
                    return true;
                }
            }
        }

        return false;
    }

    protected IEnumerator Rotate(Quaternion desiredRotation)
    {
        IsRotating = true;
        Quaternion orgRotation = transform.rotation;
        float time = 0;

        while (time < StateSettings.Settings.RotationTime)
        {
            time += Time.deltaTime;

            transform.rotation =
                Quaternion.Slerp(orgRotation, desiredRotation,
                    time / StateSettings.Settings.RotationTime);
            yield return null;
        }

        StateSettings.direction.x *= -1;

        transform.rotation = desiredRotation;
        IsRotating = false;
    }


    protected void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        StateSettings = GetComponent<StateSettings>();

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