using System;
using UnityEngine;

public class HowToPlayInfo : MonoBehaviour
{
    [SerializeField] private GameObject canvas;

    void Start()
    {
        canvas.gameObject.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canvas.gameObject.SetActive(true);
        }
    }


    public void OnTriggerExit(Collider other)
    {
        canvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }
}