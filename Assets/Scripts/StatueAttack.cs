using TMPro;
using UnityEditor;
using UnityEngine;

public class StatueAttack : MonoBehaviour
{
    [SerializeField]GameObject shockwave;

    Material transparency;
    bool fadeAway;

    //when colliding with ground, activate attack hitbox
    private void OnCollisionEnter(Collision other)
    {
        shockwave.SetActive(true);
        Invoke("RemoveShockwave", 0.15f);
        fadeAway = true;
    }

    private void RemoveShockwave()
    { shockwave.SetActive(false); }

    public void ResetColor()
    {
        fadeAway = false;
        transparency = gameObject.GetComponent<MeshRenderer>().material;
        Color fade = transparency.color;
        fade.a = 1;
        transparency.color = fade;
    }

    private void Start()
    {
        transparency = gameObject.GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        if (fadeAway)
        {
            Color fade = transparency.color;

            fade.a -= Time.deltaTime;
            
            if (fade.a <= 0)
            {
                fade.a = 0;
            }

            transparency.color = fade;
        }
        
    }
}
