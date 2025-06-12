using UnityEngine;

public class Spinning : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 50f;

    [SerializeField]
    private Vector3 rotationAxis = Vector3.forward;
    
    void Update()
    {
        transform.Rotate(rotationAxis, rotationSpeed * Time.unscaledDeltaTime);
    }
}
