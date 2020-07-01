using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claw : MonoBehaviour
{
    public Transform controller;
    public bool end = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = controller.position;
        gameObject.transform.rotation = controller.rotation;
        gameObject.transform.Rotate(0.0f, 180.0f, 180.0f);

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("KaoShiung"))
        {
            end = true;
        }
    }
}
