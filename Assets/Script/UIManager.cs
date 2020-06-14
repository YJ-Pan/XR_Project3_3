using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public AngelAI angelAI;
    public GameManager gameManager;
    public GameObject Fade;

    [Header("Button")]
    public GameObject viewChange;
    public GameObject endDay;

    [Header("Love")]
    public GameObject love;
    float loveMax;
    float loveAmount; // 0 ~ 120;

    [Header("Scare")]
    public GameObject scare;
    float scareMax;
    float scareAmount; // 0 ~ 120;

    [Header("GameOver")]
    public GameObject over;
    public Text because;

    // Start is called before the first frame update
    void Start()
    {
        loveMax = 120;
        loveAmount = 0;
        scareMax = 120;
        scareAmount = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (gameManager.chapter == GameManager.Chapter.Tree || gameManager.chapter == GameManager.Chapter.Tree_night)
        {
            Fade.SetActive(false);
            viewChange.SetActive(true);
            endDay.SetActive(true); 
        }
        else
        {
            Fade.SetActive(true);
            viewChange.SetActive(false);
            endDay.SetActive(false);
        }

        loveAmount = angelAI.love;
        scareAmount = angelAI.scare;

        love.GetComponent<RectTransform>().offsetMax = new Vector2(love.GetComponent<RectTransform>().offsetMax.x, -(loveMax - loveAmount));
        scare.GetComponent<RectTransform>().offsetMax = new Vector2(scare.GetComponent<RectTransform>().offsetMax.x, -(scareMax - scareAmount));
    }
}