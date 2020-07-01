using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public enum Emoji
    {
        None,
        Happy,
        Sad,
        Scared,
        Angey,
        Dissatisfied,
        Confused
    }

    public GameObject angel;
    public Camera gameCamera;
    public GameManager gameManager;
    
    [Header("Fade")]
    public GameObject Fade;
    public bool fading = false;

    [Header("Angel Feeling")]
    public GameObject feeling;
    public Emoji showFeeling;
    
    [Header("Button")]
    public GameObject followAngel;
    public GameObject viewChange;
    public GameObject endDay;
    public bool isEndDay;

    [Header("Love")]
    public GameObject love;
    public float loveMax;
    float loveAmount; // 0 ~ 120;

    [Header("Scare")]
    public GameObject scare;
    public float scareMax;
    float scareAmount; // 0 ~ 120;

    [Header("Task")]
    public Text Task1;
    public Text Task2;

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
        showFeeling = Emoji.None;
        isEndDay = false;
    }
    // Update is called once per frame
    void Update()
    {
        #region Fade
        Fade.SetActive(fading);
        #endregion

        #region button
        endDay.SetActive(isEndDay);

        if (gameManager.chapter == GameManager.Chapter.Tree || gameManager.chapter == GameManager.Chapter.Tree_night || gameManager.chapter == GameManager.Chapter.Parrot || gameManager.chapter == GameManager.Chapter.Parrot_night)
        {
            followAngel.SetActive(true);
            viewChange.SetActive(true);
        }
        else
        {
            followAngel.SetActive(false);
            viewChange.SetActive(false);
            endDay.SetActive(false);
        }
        #endregion

        #region feeling
        if(showFeeling != Emoji.None)
        {
            feeling.SetActive(true);

            switch (showFeeling)
            {
                case Emoji.Happy:
                    feeling.GetComponentInChildren<Text>().text = "o(^▽^)o";
                    break;
                case Emoji.Sad:
                    feeling.GetComponentInChildren<Text>().text = "(◞‸◟)";
                    break;
                case Emoji.Dissatisfied:
                    feeling.GetComponentInChildren<Text>().text = "(¬_¬)";
                    break;
                case Emoji.Scared:
                    feeling.GetComponentInChildren<Text>().text = "Σ( ° △ °|||)";
                    break;
                case Emoji.Angey:
                    feeling.GetComponentInChildren<Text>().text = "(╬•᷅д•᷄╬)";
                    break;
                case Emoji.Confused:
                    feeling.GetComponentInChildren<Text>().text = "( ・◇・)？";
                    break;
                default:
                    break;
            }  
            StartCoroutine(ShowFeeling());
            showFeeling = Emoji.None;
        }
        RectTransform r = gameObject.GetComponentInChildren<RectTransform>();
        //世界物件在螢幕上的座標，螢幕左下角為(0,0)，右上角為(1,1)
        Vector2 screenPos = gameCamera.WorldToViewportPoint(angel.transform.position);
        //世界物件在螢幕上轉換為UI的座標，UI的Pivot point預設是(0.5, 0.5)，這邊把座標原點置中，並讓一個單位從0.5改為1
        Vector2 viewPos = (screenPos - r.pivot) * 2;
        //UI一半的寬，因為原點在中心
        float width = r.rect.width / 2;
        //UI一半的高
        float height = r.rect.height / 2;
        feeling.GetComponent<RectTransform>().anchoredPosition = new Vector2(viewPos.x * width, viewPos.y * height + 100.0f);
        #endregion

        #region love & scare
        loveAmount = angel.GetComponent<AngelAI>().love;
        scareAmount = angel.GetComponent<AngelAI>().scare;

        love.GetComponent<RectTransform>().offsetMax = new Vector2(love.GetComponent<RectTransform>().offsetMax.x, -(loveMax - loveAmount));
        scare.GetComponent<RectTransform>().offsetMax = new Vector2(scare.GetComponent<RectTransform>().offsetMax.x, -(scareMax - scareAmount));
        #endregion

        #region Task
        if (gameManager.chapter == GameManager.Chapter.Begin_scenario)
        {
            Task1.text = "序章|";
            Task2.text = "　　獲得系統";
        }
        else if (gameManager.chapter == GameManager.Chapter.Tree_scenario)
        {
            Task1.text = "新手教學|";
            Task2.text = "　　獻上水果";
        }
        else if (gameManager.chapter == GameManager.Chapter.Tree_night_scenario)
        {
            Task1.text = "樹之章|";
            Task2.text = "　　鳥媽媽的託付";
        }
        else if (gameManager.chapter == GameManager.Chapter.Tree)
        {
            Task1.text = "樹之章|";
            Task2.text = "　　刷好感度囉";
        }
        else if (gameManager.chapter == GameManager.Chapter.Tree_night)
        {
            Task1.text = "樹之章支線|";
            Task2.text = "　　保護鳥巢";
        }
        else if (gameManager.chapter == GameManager.Chapter.Transform)
        {
            Task1.text = "轉生之間|";
            Task2.text = "　　選擇轉生角色";
        }
        else if (gameManager.chapter == GameManager.Chapter.Parrot_in_cage)
        {
            Task1.text = "鸚之章|";
            Task2.text = "　　進行初次對話";
        }
        else if (gameManager.chapter == GameManager.Chapter.Parrot_to_home)
        {
            Task1.text = "鸚之章|";
            Task2.text = "　　回家囉";
        }
        else if (gameManager.chapter == GameManager.Chapter.Parrot)
        {
            Task1.text = "鸚之章|";
            Task2.text = "　　尋找女主進行對話";
        }
        else if (gameManager.chapter == GameManager.Chapter.Parrot_night)
        {
            Task1.text = "鸚之章|";
            Task2.text = "　　點擊雞來迎接早上";
        }
        else
        {
            Task1.text = "";
            Task2.text = "";
        }

        #endregion
    }

    public void FadeIn()
    {
        fading = true;
        StartCoroutine(FadeImage(true));
       
    }

    public void FadeOut()
    {
        fading = true;
        StartCoroutine(FadeImage(false));
    }

    IEnumerator FadeImage(bool fadeAway)
    {
        // fade from opaque to transparent
        if (fadeAway)
        {
            // loop over 1 second backwards
            for (float i = 1; i >= -0.5f; i -= Time.deltaTime)
            {
                // set color with i as alpha
                Fade.GetComponent<Image>().color = new Color(0, 0, 0, i);
                yield return null;
            }
        }
        // fade from transparent to opaque
        else
        {
            // loop over 1 second
            for (float i = 0; i <= 1.5f; i += Time.deltaTime)
            {
                // set color with i as alpha
                Fade.GetComponent<Image>().color = new Color(0, 0, 0, i);
                yield return null;
            }
        }
        fading = false;
    }

    IEnumerator ShowFeeling()
    {
        yield return new WaitForSeconds(2.0f);
        feeling.SetActive(false);  
    }
}
 