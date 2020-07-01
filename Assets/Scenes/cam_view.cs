using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cam_view : MonoBehaviour
{
    public GameObject cam;
    public GameObject from_123, to_123;
    Vector3 from, to, dir;

    public float rotateSpeedX = 10.0f;

    public float speed = 4.0f;
    int fps = 3000;

    float x, y, z;

    bool flag = false;

    // Start is called before the first frame update
    void Start()
    {
        from = from_123.transform.position;
        to = to_123.transform.position;

        dir = Vector3.Normalize(to - from);

        /*
        x = ( - from.x + to.x) / fps;
        y = ( - from.y + to.y) / fps;
        z = ( - from.z + to.z) / fps;
        */
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(dir);
        //cam.transform.position += dir;
        if (Input.GetKeyUp(KeyCode.Space))
        {
            flag = !flag;
        }

        if (flag)
        {
            transform.Translate(dir * speed * Time.deltaTime, Space.World);
            //gameObject.transform.Rotate(rotateSpeedX, 0, 0);
        }
        
    }
}
