using System;
using UnityEngine;

public class TriggerController : MonoBehaviour
{
    [SerializeField] State[] states;

    private void Start()
    {
        states = GetComponentsInParent<State>();
    }

    private void OnTriggerStay(Collider other)
    {
        foreach (var state in states)
        {
            if (state.enabled)
            {
                state.ReceiveTrigger(gameObject.name, true, other);
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        foreach (var state in states)
        {
            if (state.enabled)
            {
                state.ReceiveTrigger(gameObject.name, false, other);
            }
        }
    }
}