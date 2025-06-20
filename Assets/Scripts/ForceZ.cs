using UnityEngine;

public class ForceZ : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 tra = transform.position;
        tra.z = 0;
        
        transform.position = tra;
    }
}
