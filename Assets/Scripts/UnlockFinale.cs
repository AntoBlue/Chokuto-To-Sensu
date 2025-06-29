using System;
using TMPro;
using UnityEngine;


public class UnlockFinale : MonoBehaviour
{

    [SerializeField] private GameObject Boss;

    [SerializeField] private Collider prisonCollider;
    [SerializeField] private TextMeshPro prisonText;
    
    private void OnEnable()
    {
        HealthManager.OnDeath += Unlock;
    }

    private void OnDisable()
    {
        HealthManager.OnDeath -= Unlock;
    }


    void Unlock(string gameObjectName)
    {
        if (gameObjectName == Boss.name)
        {
            prisonCollider.enabled = false;
            prisonText.text = "Prison Open";
        }
    }
}