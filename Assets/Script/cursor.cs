﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cursor : MonoBehaviour
{
    public Texture2D image;
    public Camera gameCamera;
    public GameObject hint;
    public GameObject UI;
    public UIManager uIManager;
    public int NestLife;
    public int waterVolume;

    [Header("Audio")]
    public AudioSource waterBank;
    public AudioClip cockCrow;
    public AudioClip waterSound;
    public AudioClip broken;
    public AudioClip guzheng;

    [Header("PC Player")]
    public AngelAI angel;
    public bool day = false;
    public int NightChickCount = 0;
    public int InteractionCount = 0;
    public int MaxCount;
    public GameObject corn;
    public Transform cornPos;
    bool ActiveCornOn = false;
    public Door door;

    private List<GameObject> brokenPlate;
    private GameObject currentObject;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        waterVolume = 100;
        brokenPlate = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        // 當滑鼠左鍵按下時，向滑鼠所在的螢幕位置發射一條射線  
        Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);  
        RaycastHit hit;  
        if(Physics.Raycast(ray, out hit))  
        {  
            // 如果與物體發生碰撞，在Scene檢視中繪製射線  
            //Debug.DrawLine(ray.origin, hit.point, Color.green);  
            // 列印射線檢測到的物體的名稱  
            //Debug.Log("射線檢測到的物體名稱: " + hit.transform.name);

            // highlight
            if (hit.transform.gameObject != null && hit.transform.gameObject != currentObject)
            {
                if(hit.transform.gameObject.tag == "Interactable")
                {
                    if(currentObject != null)
                    {
                        currentObject.GetComponent<Outline>().enabled = false;
                    }

                    hint.SetActive(true);
                    Vector2 hintposition;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(UI.transform as RectTransform, Input.mousePosition, null, out hintposition);
                    hint.GetComponent<RectTransform>().anchoredPosition = new Vector2(hintposition.x, hintposition.y + 50);
                    if(hit.transform.gameObject.name == "Toon Chicken")
                    {
                        hint.GetComponentInChildren<Text>().text = "一隻雞，會叫";
                    }
                    else if(hit.transform.gameObject.name == "Corn")
                    {
                        hint.GetComponentInChildren<Text>().text = "玉米，可引誘雞";
                    }
                    else if (hit.transform.gameObject.name == "water bank")
                    {
                        hint.GetComponentInChildren<Text>().text = "水缸\n剩餘水量: " + waterVolume.ToString();
                    }
                    else if (hit.transform.gameObject.name == "Chinese+harp")
                    {
                        hint.GetComponentInChildren<Text>().text = "古箏，可以彈奏";
                    }
                    else if (hit.transform.gameObject.name == "Guzheng")
                    {
                        hint.GetComponentInChildren<Text>().text = "古箏，可以彈奏";
                    }
                    else if (hit.transform.gameObject.name == "plate")
                    {
                        hint.GetComponentInChildren<Text>().text = "盤子，可以打破";
                    }


                    hit.transform.gameObject.GetComponent<Outline>().enabled = true;
                    currentObject = hit.transform.gameObject;
                }
                else if(hit.transform.gameObject.tag == "Nest")
                {
                    if (currentObject != null)
                    {
                        currentObject.GetComponent<Outline>().enabled = false;
                    }

                    hint.SetActive(true);
                    Vector2 hintposition;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(UI.transform as RectTransform, Input.mousePosition, null, out hintposition);
                    hint.GetComponent<RectTransform>().anchoredPosition = new Vector2(hintposition.x, hintposition.y + 50);
                    hint.GetComponentInChildren<Text>().text = "鳥巢\n生命值: " + NestLife.ToString();
                    hit.transform.gameObject.GetComponent<Outline>().enabled = true;
                    currentObject = hit.transform.gameObject;
                }
                else
                {
                    if(currentObject != null)
                    {
                        currentObject.GetComponent<Outline>().enabled = false;
                        currentObject = null;
                    }
                }
                
            }

            if(Input.GetMouseButtonDown(0))
            {
                if(hit.transform.gameObject != null)
                {
                    if(hit.transform.gameObject.name == "Toon Chicken")
                    {
                        hit.transform.gameObject.GetComponent<AudioSource>().PlayOneShot(cockCrow);
                        angel.goOut_tree += 5.0f;
                        if (day)
                        {
                            InteractionCount++;
                            angel.calculateProb();
                        }
                        else
                        {
                            NightChickCount++;
                        }
                        StartCoroutine("WaitCursor");
                    }
                    else if (hit.transform.gameObject.name == "water bank")
                    {
                        if (waterVolume > 0)
                        {
                            hit.transform.gameObject.GetComponent<AudioSource>().PlayOneShot(waterSound);
                            waterVolume -= 20;
                            hint.GetComponentInChildren<Text>().text = "水缸\n剩餘水量: " + waterVolume.ToString();
                            angel.goOut_water = 100.0f - waterVolume;
                            if (day)
                            {
                                angel.scare += 20.0f;
                                InteractionCount++;
                                angel.calculateProb();
                            }
                            else
                            {
                                angel.scare += 10.0f;
                            }
                        }
                        else
                        {
                            hint.GetComponentInChildren<Text>().text = "沒水了啦!\n Q A Q";
                            angel.goOut_water = 100.0f - waterVolume;
                            if (day)
                            {
                                angel.calculateProb();
                            }
                        }
                        StartCoroutine("WaitCursor");
                    }
                    else if (hit.transform.gameObject.name == "Guzheng")
                    {
                        hit.transform.gameObject.GetComponent<AudioSource>().PlayOneShot(guzheng);
                        angel.goOut_tree += 20.0f;
                        
                        if (day)
                        {
                            angel.scare += 20.0f;
                            InteractionCount++;
                            angel.calculateProb();
                        }
                        else
                        {
                            angel.scare += 30.0f;
                        }
                        StartCoroutine("WaitCursor");
                    }
                    else if (hit.transform.gameObject.name == "plate")
                    {
                        gameObject.GetComponent<AudioSource>().PlayOneShot(broken);
                        brokenPlate.Add(hit.transform.gameObject);
                        hit.transform.gameObject.SetActive(false);
                        angel.goOut_plateshop += 10.0f;
                        
                        if (day)
                        {
                            angel.scare += 10.0f;
                            InteractionCount++;
                            angel.calculateProb();
                        }
                        else
                        {
                            angel.scare += 15.0f;
                        }
                        StartCoroutine("WaitCursor");
                    }
                    //Debug.Log("Count: " + InteractionCount);
                }
                
            }
        }
        else
        {
            if(currentObject != null)
            {
                currentObject.GetComponent<Outline>().enabled = false;
                currentObject = null;
            }
        }

        if(currentObject == null)
        {
            hint.SetActive(false);
        }

        if(day)
        {
            if(InteractionCount >= MaxCount)
            {
                day = false;
                InteractionCount = 0;
                corn.SetActive(false);
                uIManager.isEndDay = false;
            }
            else if (!corn.activeSelf && !ActiveCornOn)
            {
                ActiveCornOn = true;
                StartCoroutine("ActiveCorn");
            }

            if(door.chickenOut)
            {
                corn.SetActive(false);
                angel.chickenOut = true;
                angel.calculateProb();
                door.chickenOut = false;
            }

        }

        if (angel.fillwater)
        {
            waterBank.PlayOneShot(waterSound);
            waterVolume = 100;
            angel.fillwater = false;
        }

        if (angel.fillplate)
        {
            for(int i=0;i< brokenPlate.Count; i++)
            {
                brokenPlate[i].SetActive(true);
            }
            brokenPlate.Clear();
            angel.fillplate = false;
        }

        if (angel.pickChicken && door.origChicken != null)
        {
            door.chicken.SetActive(false);
            angel.pickChicken = false;
            door.origChicken.SetActive(true);
        }
    }

    IEnumerator ActiveCorn()
    {
        yield return new WaitForSeconds(5.0f);
        corn.transform.position = cornPos.position;
        corn.SetActive(true);
        InteractionCount++;
        ActiveCornOn = false;
    }

    IEnumerator WaitCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        yield return new WaitForSeconds(1.0f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void EndDay()
    {
        day = false;
        InteractionCount = 0;
        corn.SetActive(false);
        uIManager.isEndDay = false;
    }
}
