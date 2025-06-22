using UnityEditor;
using UnityEngine;

public class StatueAttack : MonoBehaviour
{
    [SerializeField]GameObject shockwave;

    //when colliding with ground, activate attack hitbox
    private void OnCollisionEnter(Collision other)
    {
        shockwave.SetActive(true);
        Invoke("RemoveShockwave", 0.15f);
    }

    private void RemoveShockwave()
    { shockwave.SetActive(false); }
}
