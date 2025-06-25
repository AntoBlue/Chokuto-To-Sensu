using System.Threading;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField] GameObject doorTransform;
    [SerializeField] GameObject door;
    [SerializeField] float openingSpeed;
    [SerializeField] GameObject doorCollider;

    private bool doorOpening;
    private float currentOpening;

    private void Start()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.red;
        
    }


    private void OpenDoor()
    {
        doorOpening = true;
        gameObject.GetComponent<Renderer>().material.color = Color.green;
    }



    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            OpenDoor();
            
        }
    }

    private void FixedUpdate()
    {
        if (doorOpening && currentOpening < 180 )
        {
            doorTransform.transform.Rotate(openingSpeed, 0, 0);

            currentOpening += openingSpeed;

            doorCollider.SetActive(false);
        }
    }
}
