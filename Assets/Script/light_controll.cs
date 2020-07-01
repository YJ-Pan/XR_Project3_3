using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class light_controll : MonoBehaviour
{
    public GameObject MainLight;
    public float rotateSpeedX;
    public bool isDay;
    public List<Light> roadLight;
    public AudioSource BGM;
    public AudioSource nightBGM;
    public AudioClip morning;
    public GameObject star_night, crowds;

    bool dayToNight;
    bool nightToDay;
    int count;
    int Max;

    // Start is called before the first frame update
    void Start()
    {
        isDay = true;
        rotateSpeedX = 0.0f;
        dayToNight = false;
        nightToDay = false;
        count = 0;
        Max = 0;
        star_night.SetActive(false);
        crowds.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (dayToNight)
        {
            count++;
            if (count > Max && MainLight.transform.eulerAngles.x > 330)
            {
                dayToNight = false;
                rotateSpeedX = 0.0f;
                count = 0;
                for(int i = 0;i< roadLight.Count; i++)
                {
                    roadLight[i].enabled = true;
                }
                BGM.Stop();
                nightBGM.Play();

                star_night.SetActive(true);
                crowds.SetActive(false);

                isDay = false;
            }
        }

        if (nightToDay && MainLight.transform.eulerAngles.x > 80 && MainLight.transform.eulerAngles.x < 100)
        {
            nightToDay = false;
            rotateSpeedX = 0.0f;
            for (int i = 0; i < roadLight.Count; i++)
            {
                roadLight[i].enabled = false;
            }
            nightBGM.Stop();
            BGM.clip = morning;
            BGM.Play();

            star_night.SetActive(false);
            crowds.SetActive(true);

            isDay = true;
        }

        MainLight.transform.Rotate(rotateSpeedX, 0, 0);
    }

    public void DayToNight()
    {
        dayToNight = true;
        rotateSpeedX = 5.0f;
        Max = 24;
    }

    public void NightToDay()
    {
        nightToDay = true;
        rotateSpeedX = 5.0f;
    }
}
