using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    bool flag = true;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            flag = !flag;

            if (flag)
            {
                gameObject.GetComponent<Camera>().rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
            }
            else
            {
                gameObject.GetComponent<Camera>().rect = new Rect(0.0f, 0.0f, 0.0f, 1.0f);
            }

        }
    }
}