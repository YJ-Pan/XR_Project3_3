using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    public GameObject VRCamera;
    public GameManager gameManager;

    public Claw left;
    public Claw right;

    float speed = 0.1f;
    float maxHeight = 3.0f;

    float maxlength = 0.0f;
    int clawState = 0; // 1: left 2: right
    int stillwalking = 0;
    bool isJump = false;
    bool canJump = true;

    // Start is called before the first frame update
    void Start()
    {
        maxlength = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.eulerAngles = new Vector3(-90.0f, 0.0f, VRCamera.transform.eulerAngles.y);

        Vector3 camPos = new Vector3(VRCamera.transform.position.x, 0.0f, VRCamera.transform.position.z);
        Vector3 leftPos = new Vector3(left.gameObject.transform.position.x, 0.0f, left.gameObject.transform.position.z);
        Vector3 RightPos = new Vector3(right.gameObject.transform.position.x, 0.0f, right.gameObject.transform.position.z);

        if ((camPos - leftPos).magnitude < 0.35 && (camPos - RightPos).magnitude > 0.35)
        {
            clawState = 2;
        }
        else if ((camPos - RightPos).magnitude < 0.35 && (camPos - leftPos).magnitude > 0.35)
        {
            clawState = 1;
        }

        if ((clawState == 1 && (camPos - leftPos).magnitude < 0.35) ||
           (clawState == 2 && (camPos - RightPos).magnitude < 0.35))
        {
            Vector3 translate = VRCamera.transform.forward;
            translate.y = gameObject.GetComponent<Rigidbody>().velocity.y;
            gameObject.GetComponent<Rigidbody>().velocity = translate;
            clawState = 0;
            stillwalking = 20;
        }
        else if (stillwalking > 0)
        {
            stillwalking--;
        }
        else
        {
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        if (gameObject.transform.position.y <= 0.0f)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0.0f, gameObject.transform.position.z);
        }

        /*
        if ((camPos - leftPos).magnitude < 0.35 && (camPos - RightPos).magnitude > 0.35)
        {
            clawState = 2;
            if ((camPos - RightPos).magnitude > maxlength)
            {
                maxlength = (camPos - RightPos).magnitude;
            }
        }
        else if((camPos - RightPos).magnitude < 0.35 && (camPos - leftPos).magnitude > 0.35)
        {
            clawState = 1;
            if ((camPos - RightPos).magnitude > maxlength)
            {
                maxlength = (camPos - leftPos).magnitude;
            }
        }
        else
        {
            if ((clawState == 1 && (camPos - leftPos).magnitude < 0.35) ||
                (clawState == 2 && (camPos - RightPos).magnitude < 0.35))
            {
                Vector3 translate = new Vector3(VRCamera.transform.forward.x, 0.0f, VRCamera.transform.forward.z);
                gameObject.transform.position += translate * maxlength;
            }

            clawState = 0;
            maxlength = 0.0f;
        }
        */

    }
}
