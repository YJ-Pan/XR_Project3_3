using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class branch : MonoBehaviour
{
    public List<GameObject> fruits;
    public SteamVR_Behaviour_Boolean steamVR_Behaviour_Boolean;
    bool changefruit = false;

    public Transform controller;
    public Transform fruitPos;

    public GameManager gameManager;

    private GameObject currentFruit;
    private int currentIdx;

    Vector3 last_rot;
    int counter = 0;
    int fruitCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentFruit = null;
        currentIdx = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //gameObject.transform.forward = controller.forward;
        if (gameManager.chapter == GameManager.Chapter.Tree_night)
        {
            fruitCount = 0;
            for (int i = 1; i < fruits.Count; ++i)
                fruits[i].SetActive(false);
        }

        changefruit = SteamVR_Input.GetStateDown("default", "changeFruit", steamVR_Behaviour_Boolean.inputSource);
        if(changefruit)
        {
            currentIdx++;
            if (currentFruit != null)
                currentFruit.SetActive(false);

            if (currentIdx < fruits.Count && fruitCount < 21)
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
                if (gameManager.chapter == GameManager.Chapter.Tree)
                    fruitCount++;
            }
            else
            {
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
