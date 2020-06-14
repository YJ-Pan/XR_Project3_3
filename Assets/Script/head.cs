using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class head : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject cameraRig;
    public Transform staticpos;
    public GameManager gameManager;

    public bool isStatic = true;

    [Header("Movie")]
    public GameObject blackS;
    public GameObject moviePlane;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isStatic)
        {
            Vector3 shift = gameObject.transform.position - staticpos.position;
            shift.y = 0;
            cameraRig.transform.position -= shift;
        }
    }
}
