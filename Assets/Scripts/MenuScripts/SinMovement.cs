using UnityEngine;

public class SinMovement : MonoBehaviour
{
    [Header("Oscillation Settings")]
    [SerializeField] private Axis oscillationAxis = Axis.Y;
    [SerializeField] private float amplitude = 1f;     // Quanto si sposta
    [SerializeField] private float frequency = 1f;     // Velocità dell'oscillazione

    private Vector3 startPos;

    public enum Axis { X, Y, Z }

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * frequency) * amplitude;

        Vector3 newPos = startPos;
        switch (oscillationAxis)
        {
            case Axis.X:
                newPos.x += offset;
                break;
            case Axis.Y:
                newPos.y += offset;
                break;
            case Axis.Z:
                newPos.z += offset;
                break;
        }

        transform.position = newPos;
    }
}
