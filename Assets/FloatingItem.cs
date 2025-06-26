using UnityEngine;

public class FloatingItem : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float floatAmplitude = 0.25f; 
    [SerializeField] private float floatFrequency = 1f;   
    
    
    private Vector3 startPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //  Y Rotattion
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);

        // Up and Down
        float newY = startPosition.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
