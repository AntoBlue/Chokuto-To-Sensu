using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ExtraGravity : MonoBehaviour
{
    [SerializeField] private float gravityMultiplier = 2f; // 1 = normale, 2 = doppia gravità

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 extraGravity = (gravityMultiplier - 1f) * Physics.gravity;
        rb.AddForce(extraGravity, ForceMode.Acceleration);
    }
}