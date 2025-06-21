using UnityEngine;

public class UnlockableDoor : MonoBehaviour
{
    private bool triggered;
    [SerializeField] float openingSpeed = 50;
    [SerializeField] float openingTimer = 3;
    private float timer;

    //when player enters trigger box while having the key, open the door
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<CharacterMovement>().hasKey == true && triggered == false) 
        {
            triggered = true;
        }
    }

    void Update()
    {
        //translate door upwards for X seconds, where X is openingTimer
        if(triggered && timer < openingTimer)
        {
            transform.Translate(0, openingSpeed * 0.005f, 0);
            timer += Time.deltaTime;
        }
    }
}
