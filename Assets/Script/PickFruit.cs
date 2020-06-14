using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PickFruit : MonoBehaviour
{
    public GameObject angel;

    private GameObject fruit = null;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (fruit != null)
        {
            if (!fruit.activeSelf || fruit.tag != "Fruit")
            {
                fruit = null;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("collision");
        if (other.CompareTag("Fruit") && !angel.GetComponent<AngelAI>().chickenOut)
        {
            fruit = other.gameObject;
            angel.GetComponent<AngelAI>().fruit = other.gameObject;
            angel.GetComponent<AngelAI>().target = fruit.transform;
            angel.GetComponent<AngelAI>().walk();
        }
    }
}
