using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corn : MonoBehaviour
{
    public Camera gameCamera;
    public GameObject chicken1;
    public GameObject chicken2;

    private Vector3 screenPoint;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(gameObject.transform.position, chicken1.transform.position) < 3.0f)
        {
            Vector3 dir = gameObject.transform.position;
            chicken1.transform.LookAt(dir);
            //chicken1.transform.position += 0.1f * chicken1.transform.forward;
            chicken1.transform.Translate(chicken1.transform.forward * Time.deltaTime, Space.World);
        }

        if (Vector3.Distance(gameObject.transform.position, chicken2.transform.position) < 3.0f)
        {
            Vector3 dir = gameObject.transform.position;
            chicken2.transform.LookAt(dir);
            //chicken2.transform.position += 0.1f * chicken2.transform.forward;
            chicken2.transform.Translate(chicken2.transform.forward * Time.deltaTime, Space.World);
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
        if (other.name == chicken1.name || other.name == chicken2.name)
        {
            gameObject.SetActive(false);
        }
    }
}
