using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class head : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject cameraRig;
    public Transform staticpos;
    public GameManager gameManager;
    public GameObject parrot;
    public GameObject Cat;

    public bool isStatic = true;
    public bool movewithbody = false;


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

        if (movewithbody)
        {
            if(gameManager.chapter == GameManager.Chapter.Parrot || gameManager.chapter == GameManager.Chapter.Parrot_to_home
                || gameManager.chapter == GameManager.Chapter.Parrot_night)
            {
                Vector3 shift = gameObject.transform.position - parrot.transform.position;
                shift.y = 0;
                cameraRig.transform.position -= shift;

                shift.y = parrot.transform.position.y - 0.106f;
                cameraRig.transform.position = new Vector3(cameraRig.transform.position.x, shift.y, cameraRig.transform.position.z);
            }

            if (gameManager.chapter == GameManager.Chapter.Cat)
            {
                Vector3 shift = gameObject.transform.position - Cat.transform.position;
                shift.y = 0;
                cameraRig.transform.position -= shift;

                shift.y = Cat.transform.position.y;
                cameraRig.transform.position = new Vector3(cameraRig.transform.position.x, shift.y, cameraRig.transform.position.z);
            }
        }
    }
}
