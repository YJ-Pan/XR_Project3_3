using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corn : MonoBehaviour
{
    public Camera gameCamera;
    public GameObject chicken;

    private Vector3 screenPoint;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(gameObject.transform.position, chicken.transform.position) < 3.0f)
        {
            Vector3 dir = gameObject.transform.position;
            chicken.transform.LookAt(dir);
            //chicken1.transform.position += 0.1f * chicken1.transform.forward;
            chicken.transform.Translate(chicken.transform.forward * Time.deltaTime, Space.World);
        }
    }

    private void OnMouseDown()
    {
        screenPoint = gameCamera.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - gameCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    private void OnMouseDrag()
    {
        
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = gameCamera.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = new Vector3(curPosition.x, 0, curPosition.z);

        //transform.position = curPosition;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == chicken.name)
        {
            gameObject.SetActive(false);
        }
    }
}
