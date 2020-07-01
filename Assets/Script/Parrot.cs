using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parrot : MonoBehaviour
{
    public GameObject VRCamera;
    public bool openFlying = false;
    public GameManager gameManager;

    public GameObject Angel;
    public bool backHome = false;
    public GameObject DialogMenu;
    public ParrotTalk parrotTalk;
    public bool talkable = false;
    public Wing left;
    public Wing right;

    bool renewDialog = true;
    bool playing = false;
    public bool end = false;

    float speed = 0.1f;
    float maxHeight = 3.0f;

    float addedHeight = 0.0f;
    int stillflying = 0;

    int maxstillflying = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //gameObject.transform.position = new Vector3(VRCamera.transform.position.x, gameObject.transform.position.y, VRCamera.transform.position.z);
        gameObject.transform.eulerAngles = new Vector3(-90.0f, 0.0f, VRCamera.transform.eulerAngles.y);

        if (openFlying)
        {
            if (left.flying && right.flying || Input.GetKeyDown(KeyCode.Q))
            {
                if (gameObject.GetComponent<Rigidbody>().velocity.y < 2.0f)
                {
                    if (gameObject.GetComponent<Rigidbody>().velocity.y < 0.0f)
                    {
                        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    }

                    Vector3 translate = Vector3.zero;
                    translate = VRCamera.transform.forward;
                    translate.y = gameObject.GetComponent<Rigidbody>().velocity.y;
                    gameObject.GetComponent<Rigidbody>().velocity = translate;

                    Vector3 flyForce = Vector3.zero;
                    flyForce.y = 2.0f;
                    gameObject.GetComponent<Rigidbody>().AddForce(flyForce);
                }
                else
                {
                    Vector3 translate = Vector3.zero;
                    translate = VRCamera.transform.forward;
                    translate.y = 2.0f;
                    gameObject.GetComponent<Rigidbody>().velocity = translate;
                }
                stillflying = maxstillflying;
            }
            else if (stillflying > 0)
            {
                stillflying--;
            }
            else
            {
                if (gameObject.transform.position.y > 0.0f)
                {
                    Vector3 gravity = Vector3.zero;
                    if ((left.controller.transform.position - right.controller.transform.position).magnitude > 0.3f)
                    {
                        gravity.y = -1f;
                    }
                    else if((left.controller.transform.position - right.controller.transform.position).magnitude < 0.1f)
                    {
                        gravity.y = -8f;
                        Vector3 translate = Vector3.zero;
                        translate.y = gameObject.GetComponent<Rigidbody>().velocity.y;
                        gameObject.GetComponent<Rigidbody>().velocity = translate;
                    }
                    else
                    {
                        gravity.y = -8f;
                    }

                    gameObject.GetComponent<Rigidbody>().AddForce(gravity);
                }
                else
                {
                    gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0.0f, gameObject.transform.position.z);
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.D))
        {
            maxstillflying++;
            Debug.Log(maxstillflying);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            maxstillflying--;
            Debug.Log(maxstillflying);
        }

        Vector3 dis = gameObject.transform.position - Angel.transform.position;
        dis.y = 0.0f;
        
        if(dis.magnitude < 4.5f && talkable && gameManager.chapter == GameManager.Chapter.Parrot_in_cage)
        {
            parrotTalk.startDialog();
            DialogMenu.SetActive(true);
            right.talkable = true;
        }
        else if (dis.magnitude < 2.5f && talkable && Angel.GetComponent<AngelAI>().talk_count > 0)
        {
            if(end && renewDialog) parrotTalk.endDialog();
            else if (renewDialog) parrotTalk.updateDialog();
            renewDialog = false;
            DialogMenu.SetActive(true);
            right.talkable = true;
            Angel.GetComponent<AngelAI>().stopWalk();
        }
        else if(DialogMenu.activeSelf)
        {
            DialogMenu.SetActive(false);
        }

        if (right.dialog > 0)
        {
            Angel.GetComponent<AngelAI>().talk_count--;
            Angel.GetComponent<AngelAI>().love += parrotTalk.love_added[right.dialog - 1];
            parrotTalk.love_added[0] = 0;
            parrotTalk.love_added[1] = 0;
            parrotTalk.love_added[2] = 0;
            right.talkable = false;
            DialogMenu.SetActive(false);
            right.dialog = 0;
            renewDialog = true;

            if (gameManager.chapter == GameManager.Chapter.Parrot_in_cage)
            {
                parrotTalk.gameObject.GetComponent<AudioSource>().PlayOneShot(parrotTalk.audio[7]);
                playing = true;
                //backHome = true;
            }
            if(end)
            {
                if (Angel.GetComponent<AngelAI>().love > 40.0)
                {
                    parrotTalk.gameObject.GetComponent<AudioSource>().PlayOneShot(parrotTalk.audio[5]);
                }
                else
                {
                    parrotTalk.gameObject.GetComponent<AudioSource>().PlayOneShot(parrotTalk.audio[6]);
                }
                playing = true;
                //right.dialog = -1;
            }

        }

        if(playing && !parrotTalk.gameObject.GetComponent<AudioSource>().isPlaying)
        {
            playing = false;
            if(gameManager.chapter == GameManager.Chapter.Parrot_in_cage)
                backHome = true;
            if (end)
                right.dialog = -1;
        }

        //Debug.Log((left.controller.transform.position - right.controller.transform.position).magnitude);
    }

}
