using System.Threading;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField] GameObject doorTransform;
    [SerializeField] GameObject door;
    [SerializeField] float openingSpeed;


    private bool doorOpening;
    private float currentOpening;

    private void OpenDoor()
    {
        doorOpening = true;
        door.GetComponent<Collider>().enabled = false;
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
        }
    }
}
