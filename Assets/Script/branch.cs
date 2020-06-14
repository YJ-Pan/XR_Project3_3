using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class branch : MonoBehaviour
{
    // Start is called before the first frame update
    /*public enum Fruit
    {
        None,
        Apple,
        Banana,
        Kiwi,
        Pear,
        Strawberry
    }*/
    public List<GameObject> fruits;
    public SteamVR_Behaviour_Boolean steamVR_Behaviour_Boolean;
    bool changefruit = false;

    public Transform controller;
    public Transform fruitPos;

    private GameObject currentFruit;
    private int currentIdx;

    Vector3 last_rot;
    int counter = 0;
    
    void Start()
    {
        currentFruit = null;
        currentIdx = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //gameObject.transform.forward = controller.forward;
        changefruit = SteamVR_Input.GetStateDown("default", "changeFruit", steamVR_Behaviour_Boolean.inputSource);
        if(changefruit)
        {
            currentIdx++;
            if (currentFruit != null)
                currentFruit.SetActive(false);

            if (currentIdx < fruits.Count)
            {
                currentFruit = fruits[currentIdx];
                currentFruit.SetActive(true);
                currentFruit.tag = "Untagged";
            }
            else
            {
                currentIdx = 0;
                currentFruit = null;
            }
        }

        gameObject.transform.rotation = controller.rotation;
        gameObject.transform.Rotate(180.0f, 0.0f, 0.0f);
        if(currentFruit != null){
            if(counter > 10){
                currentFruit.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * 3);
                currentFruit.tag = "Fruit";
                currentFruit = null;
                counter = 0;
            }else{
                currentFruit.transform.position = fruitPos.position;
            }
            computeRot();
        }
    }

    void computeRot()
    {
        Vector3 differ = (gameObject.transform.rotation).eulerAngles - last_rot;
        if((System.Math.Abs(differ.x)+System.Math.Abs(differ.y)+System.Math.Abs(differ.z)) > 30)
        {
            counter += 1;
        }
        last_rot = (gameObject.transform.rotation).eulerAngles;
    }
}
