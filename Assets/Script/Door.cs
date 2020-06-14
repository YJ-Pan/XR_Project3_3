using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // Start is called before the first frame update
    //public Transform treePos;
    public GameObject chicken;
    public GameObject origChicken;
    public bool chickenOut = false;
    public Vector3 chickenPos;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Toon Chicken")
        {
            Debug.Log("ChickenOut");
            //origChicken = other.gameObject;
            origChicken.SetActive(false);
            origChicken.transform.position = chickenPos;
            chicken.SetActive(true);
            chickenOut = true;
        }
    }
}
