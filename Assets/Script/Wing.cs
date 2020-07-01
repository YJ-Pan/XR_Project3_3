using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.UI;

public class Wing : MonoBehaviour
{
    public Transform controller;
    public bool flying;
    public bool talkable;
    public int dialog;
    public SteamVR_Behaviour_Vector2 steamVR_Behaviour_Vector2;
    public SteamVR_Behaviour_Boolean steamVR_Behaviour_Boolean;
    public Image dialog1;
    public Image dialog2;
    public Image dialog3;

    Vector2 touchpadAxis;

    Vector3 lastPos;
    float maxY;
    float minY;

    // Start is called before the first frame update
    void Start()
    {
        lastPos = controller.position;
        maxY = 0.0f;
        minY = 10.0f;
        StartCoroutine(CheckFlying());
        dialog = 0;
        talkable = false;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.rotation = controller.rotation;
        gameObject.transform.Rotate(-20.0f, -150.0f, -90.0f);
        
        if(controller.position.y > maxY)
        {
            maxY = controller.localPosition.y;
        }
        if(controller.position.y < minY)
        {
            minY = controller.localPosition.y;
        }

        if(maxY - minY > 0.2f)
        {
            flying = true;
        }
        else
        {
            flying = false;
        }

        touchpadAxis = SteamVR_Input.GetVector2("Default", "chooseDialog", steamVR_Behaviour_Vector2.inputSource);
        if (talkable)
        {
            if(touchpadAxis.y > 0.5)
            {
                dialog1.color = Color.yellow;
                dialog2.color = Color.white;
                dialog3.color = Color.white;
                if(SteamVR_Input.GetStateDown("default", "clickDialog", steamVR_Behaviour_Boolean.inputSource))
                {
                    dialog = 1;
                }
            }
            else if(touchpadAxis.y < -0.5)
            {
                dialog1.color = Color.white;
                dialog2.color = Color.white;
                dialog3.color = Color.yellow;
                if (SteamVR_Input.GetStateDown("default", "clickDialog", steamVR_Behaviour_Boolean.inputSource))
                {
                    dialog = 3;
                }
            }
            else
            {
                dialog1.color = Color.white;
                dialog2.color = Color.yellow;
                dialog3.color = Color.white; 
                if (SteamVR_Input.GetStateDown("default", "clickDialog", steamVR_Behaviour_Boolean.inputSource))
                {
                    dialog = 2;
                }
            }
        }

    }

    IEnumerator CheckFlying()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            maxY = 0.0f;
            minY = 10.0f;
        }
    }
}
