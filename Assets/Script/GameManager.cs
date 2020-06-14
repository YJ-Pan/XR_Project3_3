using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum Chapter
    {
        Begin_movie,
        Begin_scenario,
        Tree_scenario,
        End_tutorial,
        Tree_night_scenario,
        Tree,
        Tree_night,
        Transform
    }

    public GameObject VrUser;
    public GameObject mainCamera;
    public GameObject UI;
    public Image Fade;
    public Scenario scenario;
    public CameraPosition cameraPos;
    public light_controll light_c;
    public GameObject angel;
    public EnemyManager enemyManager;
    public cursor pcPlayer;
    public head vrPlayer;
    public List<Transform> vrPos;
    public Trans trans;
    public GameObject tree;
    public GameObject BirdMom;
    public GameObject Nest;
    public GameObject blackBox;
    public GameObject statue;

    [Header("Audio")]
    public AudioSource BGM;
    public AudioClip morning;
    public AudioClip birdMomBGM;
    public AudioSource nightBGM;
    public AudioSource birdSound;
    public List<AudioClip> bird;

    [Header("Chapter")]
    public Chapter chapter;

    private bool moviePlaying = false;
    private bool fadeIn;
    private bool fadeOut;
    private int count;
    private int dayCount = 0;
    private bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        /*scenario = Chapter.Begin_movie;
        mainCamera.transform.position = cameraPos.pos[0].position;
        mainCamera.transform.rotation = cameraPos.pos[0].rotation;
        vrPlayer.cameraRig.transform.position += new Vector3(0.0f, 1000.0f, 0.0f);
        vrPlayer.blackS.SetActive(true);
        vrPlayer.moviePlane.SetActive(true);*/
        fadeIn = false;
        fadeOut = false;
        //SteamVR_Fade.Start(Color.black, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        pcPlayer.NestLife = enemyManager.nestLife;
        if (enemyManager.nestLife <= 0)
        {
            Nest.SetActive(false);
            gameOver = true;
            UI.GetComponent<UIManager>().over.SetActive(true);
            UI.GetComponent<UIManager>().because.text = "沒有保護好鳥巢";
        }

        if (angel.GetComponent<AngelAI>().scare >= 100.0f)
        {
            gameOver = true;
            UI.GetComponent<UIManager>().over.SetActive(true);
            UI.GetComponent<UIManager>().because.text = "把女主嚇跑了";
        }
            

        if (chapter == Chapter.Begin_movie)
        {
            if (mainCamera.GetComponent<UnityEngine.Video.VideoPlayer>().isPlaying)
            {
                UI.SetActive(false);
                moviePlaying = true;
            }
            if (mainCamera.GetComponent<UnityEngine.Video.VideoPlayer>().isPlaying == false && moviePlaying)
            {
                moviePlaying = false;
                mainCamera.GetComponent<UnityEngine.Video.VideoPlayer>().enabled = false;
                chapter = Chapter.Begin_scenario;
                UI.SetActive(true);
                BGM.Play();
                StartCoroutine(FadeImage(true));
                fadeIn = true;
                vrPlayer.cameraRig.transform.position -= new Vector3(0.0f, 1000.0f, 0.0f);
                vrPlayer.blackS.SetActive(false);
                vrPlayer.moviePlane.SetActive(false);
                //SteamVR_Fade.Start(Color.clear, 5f);
            }
        }
        else if (chapter == Chapter.Begin_scenario)
        {
            if (Fade.color.a <= 0.0f)
            {
                StopCoroutine(FadeImage(true));
                fadeIn = false;
            }
            if (scenario.dialog.activeSelf == false && fadeOut == false)
            {
                StartCoroutine(FadeImage(false));
                fadeOut = true;
            }
            if (scenario.dialog.activeSelf == false && Fade.color.a >= 1.0f)
            {
                mainCamera.transform.position = cameraPos.pos[1].position;
                mainCamera.transform.rotation = cameraPos.pos[1].rotation;
                chapter = Chapter.Tree_scenario;
                StopCoroutine(FadeImage(false));
                StartCoroutine(FadeImage(true));
                fadeOut = false;
                fadeIn = true;
            }
        }
        else if (chapter == Chapter.Tree_scenario)
        {
            if (Fade.color.a <= 0.0f)
            {
                StopCoroutine(FadeImage(true));
                fadeIn = false;
            }
            if (angel.GetComponent<AngelAI>().love > 3.0)
            {
                chapter = Chapter.End_tutorial;
                angel.GetComponent<AngelAI>().target = angel.GetComponent<AngelAI>().home;
                angel.GetComponent<AngelAI>().fruit = null;
                angel.GetComponent<AngelAI>().walk();
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                angel.GetComponent<AngelAI>().love += 4.0f;
            }
        }
        else if (chapter == Chapter.End_tutorial)
        {
            if (scenario.dialog.activeSelf == false && light_c.isDay && light_c.rotateSpeedX == 0.0f)
            {
                light_c.DayToNight();
                BirdMom.SetActive(true);
            }

            if (!light_c.isDay)
            {
                chapter = Chapter.Tree_night_scenario;
                BGM.clip = birdMomBGM;
                BGM.Play();
                nightBGM.Play();
                count = 0;
            }
        }
        else if (chapter == Chapter.Tree_night_scenario)
        {
            if (scenario.dialog.activeSelf == false && fadeOut == false)
            {
                BirdMom.SetActive(false);
                light_c.NightToDay();
                StartCoroutine(FadeImage(false));
                fadeOut = true;
            }
            if (scenario.dialog.activeSelf == false && Fade.color.a >= 1.0f)
            {
                mainCamera.transform.position = cameraPos.pos[0].position;
                mainCamera.transform.rotation = cameraPos.pos[0].rotation;
                StopCoroutine(FadeImage(false));
                StartCoroutine(FadeImage(true));
                fadeOut = false;
                fadeIn = true;
                angel.GetComponent<AngelAI>().stopWalk();
                angel.GetComponent<AngelAI>().teleport(angel.GetComponent<AngelAI>().home.position);
                pcPlayer.day = true;
                angel.GetComponent<AngelAI>().calculateProb();
                chapter = Chapter.Tree;
                scenario.dialogText.GetComponent<TypewriterEffect>().charsPerSecond = 0.02f;
            }
            if (scenario.dialog.activeSelf && Input.GetKeyDown(KeyCode.Space) && count < 3)
            {
                if (!scenario.dialogText.GetComponent<TypewriterEffect>().isActive)
                {
                    scenario.dialogText.GetComponent<TypewriterEffect>().charsPerSecond = 0.2f;
                    birdSound.clip = bird[count];
                    birdSound.Play();
                    count++;
                }
            }
        }
        else if (chapter == Chapter.Tree)
        {
            if (Fade.color.a <= 0.0f)
            {
                StopCoroutine(FadeImage(true));
                fadeIn = false;
            }

            if (!pcPlayer.day && angel.GetComponent<AngelAI>().isHome())
            {
                chapter = Chapter.Tree_night;
                light_c.DayToNight();
                enemyManager.generate = true;
                angel.GetComponent<AngelAI>().target = angel.GetComponent<AngelAI>().bed;
                angel.GetComponent<AngelAI>().walk();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                light_c.NightToDay();
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                light_c.DayToNight();
            }
        }
        else if (chapter == Chapter.Tree_night)
        {

            if (enemyManager.end && fadeOut == false)
            {
                light_c.NightToDay();
                StartCoroutine(FadeImage(false));
                fadeOut = true;
                dayCount++;
            }
            if (enemyManager.end && Fade.color.a >= 1.0f)
            {
                if (dayCount > 2)
                {
                    mainCamera.transform.position = cameraPos.pos[5].position;
                    mainCamera.transform.rotation = cameraPos.pos[5].rotation;
                    vrPlayer.isStatic = true;
                    vrPlayer.staticpos = vrPos[1];
                    chapter = Chapter.Transform;
                    tree.SetActive(false);
                    blackBox.SetActive(true);
                }
                else
                {
                    mainCamera.transform.position = cameraPos.pos[0].position;
                    mainCamera.transform.rotation = cameraPos.pos[0].rotation;
                    chapter = Chapter.Tree;
                    angel.GetComponent<AngelAI>().teleport(angel.GetComponent<AngelAI>().home.position);
                    angel.GetComponent<AngelAI>().calculateProb();
                }
                StopCoroutine(FadeImage(false));
                StartCoroutine(FadeImage(true));
                fadeOut = false;
                fadeIn = true;
                pcPlayer.day = true;
                enemyManager.end = false;
            }
        }
        else if (chapter == Chapter.Transform)
        {
            if(gameOver)
            {
                statue.SetActive(false);
            }
            else if(trans.isTrans)
            {
                if (trans.charactor == Trans.Charactor.Parrot)
                {
                    vrPlayer.cameraRig.transform.position = vrPos[2].position;
                    vrPlayer.isStatic = false;
                }
            }

        }
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
                Fade.color = new Color(0, 0, 0, i);
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
                Fade.color = new Color(0, 0, 0, i);
                yield return null;
            }
        }
    }
}
