using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Valve.VR;

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
        Tree_end_prepare,
        Tree_end,
        Transform,
        Parrot_begin_prepare,
        Parrot_begin,
        Parrot_in_cage,
        Parrot_to_home,
        Parrot,
        Parrot_night,
        Parrot_end_prepare,
        Parrot_end,
        Cat_begin_prepare,
        Cat_begin,
        Cat,
        Cat_end_prepare,
        Cat_end,
        Transform_human,
        The_End_prepare,
        The_End
    }

    [Header("User")]
    public GameObject VrUser;
    public GameObject mainCamera;
    public cursor pcPlayer;
    public head vrPlayer;

    [Header("UI")]
    public GameObject UI;
    public Image Fade;

    [Header("Other")]
    public Scenario scenario;
    public CameraPosition cameraPos;
    public light_controll light_c;
    public EnemyManager enemyManager;
    public List<Transform> vrPos;
    public Transform tramsPos;
    public Trans trans;

    [Header("GameObject")]
    public GameObject angel;
    public GameObject tree;
    public GameObject BirdMom;
    public GameObject Nest;
    public GameObject statue;
    public GameObject statue_human;
    public GameObject statue_hand;

    [Header("Parrot")]
    public GameObject parrot;
    public GameObject parrot_body;
    public GameObject parrot_feet;
    public GameObject left_wing;
    public GameObject right_wing;
    public GameObject Cage;
    public Transform CagePosOnRoad;
    public Transform CagePosAtHome;

    [Header("Cat")]
    public GameObject cat;
    public GameObject left_claw;
    public GameObject right_claw;

    [Header("Audio")]
    public AudioSource BGM;
    public AudioClip morning;
    public AudioClip birdMomBGM;
    public AudioSource nightBGM;
    public AudioClip transformBGM;
    public AudioClip ParrotMorning;
    public AudioClip CatMorning;
    public AudioSource birdSound;
    public List<AudioClip> bird;
    public AudioSource marketBGM;

    [Header("Video")]
    public VideoClip treeDead;
    public VideoClip transformParrot;
    public VideoClip parrotDead;
    public VideoClip transformCat;
    public VideoClip catDead;
    public VideoClip TheEnd;


    [Header("Chapter")]
    public Chapter chapter;

    private bool moviePlaying = false;
    private int count;
    private int dayCount = 0;
    private bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        chapter = Chapter.Begin_movie;
        mainCamera.transform.position = cameraPos.pos[0].position;
        mainCamera.transform.rotation = cameraPos.pos[0].rotation;
        vrPlayer.cameraRig.transform.position += new Vector3(0.0f, 1000.0f, 0.0f);
        vrPlayer.blackS.SetActive(true);
        vrPlayer.moviePlane.SetActive(true);
        //SteamVR_Fade.Start(Color.black, 0f);
        //SteamVR_Fade.Start(Color.clear, 5f);
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

        if (angel.GetComponent<AngelAI>().scare >= UI.GetComponent<UIManager>().scareMax)
        {
            gameOver = true;
            UI.GetComponent<UIManager>().over.SetActive(true);
            UI.GetComponent<UIManager>().because.text = "把女主嚇跑了";
        }

        if (UI.GetComponent<UIManager>().over.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                gameOver = false;
                UI.GetComponent<UIManager>().over.SetActive(false);
                angel.GetComponent<AngelAI>().scare = 0;
                angel.GetComponent<AngelAI>().love = 0;
                dayCount = 0;
                mainCamera.transform.position = cameraPos.pos[0].position;
                mainCamera.transform.rotation = cameraPos.pos[0].rotation;
                chapter = Chapter.Tree;
                angel.GetComponent<AngelAI>().teleport(angel.GetComponent<AngelAI>().home.position);
                angel.GetComponent<AngelAI>().stopWalk();
                angel.GetComponent<AngelAI>().anim.Play("Idle");
                UI.GetComponent<UIManager>().FadeIn();
                pcPlayer.day = true;
                enemyManager.end = false;
            }
        }

        #region Flow Control
        if (chapter == Chapter.Begin_movie)
        {
            if (mainCamera.GetComponent<VideoPlayer>().isPlaying)
            {
                UI.SetActive(false);
                moviePlaying = true;
            }
            if (mainCamera.GetComponent<VideoPlayer>().isPlaying == false && moviePlaying)
            {
                moviePlaying = false;
                mainCamera.GetComponent<VideoPlayer>().enabled = false;
                vrPlayer.cameraRig.transform.position -= new Vector3(0.0f, 1000.0f, 0.0f);
                vrPlayer.blackS.SetActive(false);
                vrPlayer.moviePlane.SetActive(false);
                UI.SetActive(true);
                UI.GetComponent<UIManager>().FadeIn();
                BGM.Play();
                chapter = Chapter.Begin_scenario;
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                mainCamera.GetComponent<VideoPlayer>().Stop();
                vrPlayer.gameObject.GetComponent<VideoPlayer>().Stop();
            }

        }
        else if (chapter == Chapter.Begin_scenario)
        {
            if (scenario.dialog.activeSelf == false && UI.GetComponent<UIManager>().fading == false && Fade.color.a < 1.0f)
            {
                UI.GetComponent<UIManager>().FadeOut();
            }
            if (scenario.dialog.activeSelf == false && UI.GetComponent<UIManager>().fading == false && Fade.color.a >= 1.0f)
            {
                mainCamera.transform.position = cameraPos.pos[1].position;
                mainCamera.transform.rotation = cameraPos.pos[1].rotation;
                UI.GetComponent<UIManager>().FadeIn();
                chapter = Chapter.Tree_scenario;
            }
        }
        else if (chapter == Chapter.Tree_scenario)
        {
            if (angel.GetComponent<AngelAI>().love > 0.0 && angel.GetComponent<AngelAI>().anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                angel.GetComponent<AngelAI>().target = angel.GetComponent<AngelAI>().home;
                angel.GetComponent<AngelAI>().fruit = null;
                angel.GetComponent<AngelAI>().walk();
                chapter = Chapter.End_tutorial;
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
            if (scenario.dialog.activeSelf == false && UI.GetComponent<UIManager>().fading == false && Fade.color.a < 1.0f)
            {
                BirdMom.SetActive(false);
                light_c.NightToDay();
                UI.GetComponent<UIManager>().FadeOut();
            }
            if (scenario.dialog.activeSelf == false && UI.GetComponent<UIManager>().fading == false && Fade.color.a >= 1.0f)
            {
                mainCamera.transform.position = cameraPos.pos[0].position;
                mainCamera.transform.rotation = cameraPos.pos[0].rotation;
                UI.GetComponent<UIManager>().FadeIn();
                angel.GetComponent<AngelAI>().stopWalk();
                angel.GetComponent<AngelAI>().teleport(angel.GetComponent<AngelAI>().home.position);
                pcPlayer.day = true;
                angel.GetComponent<AngelAI>().calculateProb();
                scenario.dialogText.GetComponent<TypewriterEffect>().charsPerSecond = 0.02f;
                UI.GetComponent<UIManager>().isEndDay = true;
                chapter = Chapter.Tree;
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
            if (!pcPlayer.day && angel.GetComponent<AngelAI>().isHome() && light_c.isDay)
            {
                light_c.DayToNight();
            }

            if (!pcPlayer.day && !light_c.isDay && !angel.GetComponent<AngelAI>().getplate && !angel.GetComponent<AngelAI>().getwater)
            {
                chapter = Chapter.Tree_night;
                enemyManager.generate = true;
                angel.GetComponent<AngelAI>().target = angel.GetComponent<AngelAI>().bed;
                angel.GetComponent<AngelAI>().walk();
            }
        }
        else if (chapter == Chapter.Tree_night)
        {
            if (!light_c.isDay && enemyManager.end && UI.GetComponent<UIManager>().fading == false && Fade.color.a < 1.0f)
            {
                light_c.NightToDay();
                UI.GetComponent<UIManager>().FadeOut();
                dayCount++;
            }
            if (enemyManager.end && Fade.color.a >= 1.0f)
            {
                if (dayCount > 2)
                {
                    if (UI.GetComponent<UIManager>().fading == false && Fade.color.a >= 1.0f)
                    {
                        tree.SetActive(false);
                        vrPlayer.isStatic = false;
                        vrPlayer.cameraRig.transform.position += new Vector3(0.0f, 1000.0f, 0.0f);
                        vrPlayer.blackS.SetActive(true);
                        mainCamera.transform.position = tramsPos.position;
                        mainCamera.transform.rotation = tramsPos.rotation;
                        UI.GetComponent<UIManager>().FadeOut();
                        pcPlayer.day = true;
                        enemyManager.end = false;
                        SteamVR_Fade.Start(Color.black, 0f);
                        SteamVR_Fade.Start(Color.clear, 5f);
                        BGM.Stop();
                        mainCamera.GetComponent<VideoPlayer>().enabled = true;
                        mainCamera.GetComponent<VideoPlayer>().clip = treeDead;
                        vrPlayer.gameObject.GetComponent<VideoPlayer>().clip = treeDead;
                        vrPlayer.moviePlane.SetActive(true);

                        chapter = Chapter.Tree_end_prepare;
                    }
                }
                else
                {
                    mainCamera.transform.position = cameraPos.pos[0].position;
                    mainCamera.transform.rotation = cameraPos.pos[0].rotation;
                    chapter = Chapter.Tree;
                    angel.GetComponent<AngelAI>().teleport(angel.GetComponent<AngelAI>().home.position);
                    angel.GetComponent<AngelAI>().calculateProb();
                    UI.GetComponent<UIManager>().isEndDay = true;
                    UI.GetComponent<UIManager>().FadeIn();
                    pcPlayer.day = true;
                    enemyManager.end = false;
                }
            }
        }
        else if (chapter == Chapter.Tree_end_prepare)
        {
            mainCamera.GetComponent<VideoPlayer>().enabled = true;
            if (mainCamera.GetComponent<VideoPlayer>().isPrepared && vrPlayer.gameObject.GetComponent<VideoPlayer>().isPrepared)
            {
                mainCamera.GetComponent<VideoPlayer>().Play();
                vrPlayer.gameObject.GetComponent<VideoPlayer>().Play();
                chapter = Chapter.Tree_end;
            }
        }
        else if (chapter == Chapter.Tree_end)
        {
            if (mainCamera.GetComponent<VideoPlayer>().isPlaying)
            {
                UI.SetActive(false);
                moviePlaying = true;
            }
            if (mainCamera.GetComponent<VideoPlayer>().isPlaying == false && moviePlaying)
            {
                moviePlaying = false;
                mainCamera.GetComponent<VideoPlayer>().enabled = false;
                vrPlayer.moviePlane.SetActive(false);
                UI.SetActive(true);
                UI.GetComponent<UIManager>().FadeIn();
                BGM.clip = transformBGM;
                BGM.Play();
                SteamVR_Fade.Start(Color.black, 0f);
                SteamVR_Fade.Start(Color.clear, 5f);
                statue.SetActive(true);
                statue_hand.SetActive(true);
                chapter = Chapter.Transform;
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                mainCamera.GetComponent<VideoPlayer>().Stop();
                vrPlayer.gameObject.GetComponent<VideoPlayer>().Stop();
            }
        }
        else if (chapter == Chapter.Transform)
        {
            if (trans.isTrans)
            {
                if (UI.GetComponent<UIManager>().fading == false && Fade.color.a < 1.0f)
                {
                    UI.GetComponent<UIManager>().FadeOut();
                    BGM.Stop();
                    statue_hand.SetActive(false);
                }

                if (Fade.color.a >= 1.0f)
                {
                    if (trans.charactor == Trans.Charactor.Parrot)
                    {
                        statue.SetActive(false);
                        mainCamera.GetComponent<VideoPlayer>().enabled = true;
                        mainCamera.GetComponent<VideoPlayer>().clip = transformParrot;
                        vrPlayer.gameObject.GetComponent<VideoPlayer>().clip = transformParrot;
                        vrPlayer.moviePlane.SetActive(true);
                        trans.isTrans = false;
                        chapter = Chapter.Parrot_begin_prepare;
                    }
                    else if(trans.charactor == Trans.Charactor.Cat)
                    {
                        statue.SetActive(false);
                        mainCamera.GetComponent<VideoPlayer>().enabled = true;
                        mainCamera.GetComponent<VideoPlayer>().clip = transformCat;
                        vrPlayer.gameObject.GetComponent<VideoPlayer>().clip = transformCat;
                        vrPlayer.moviePlane.SetActive(true);
                        trans.isTrans = false;
                        chapter = Chapter.Cat_begin_prepare;
                    }
                }  
            }
        }
        else if (chapter == Chapter.Parrot_begin_prepare)
        {
            mainCamera.GetComponent<VideoPlayer>().enabled = true;
            if (mainCamera.GetComponent<VideoPlayer>().isPrepared && vrPlayer.gameObject.GetComponent<VideoPlayer>().isPrepared)
            {
                mainCamera.GetComponent<VideoPlayer>().Play();
                vrPlayer.gameObject.GetComponent<VideoPlayer>().Play();
                chapter = Chapter.Parrot_begin;
            }
        }
        else if (chapter == Chapter.Parrot_begin)
        {
            if (mainCamera.GetComponent<VideoPlayer>().isPlaying)
            {
                UI.SetActive(false);
                moviePlaying = true;
            }
            if (mainCamera.GetComponent<VideoPlayer>().isPlaying == false && moviePlaying)
            {
                moviePlaying = false;
                mainCamera.GetComponent<VideoPlayer>().enabled = false;
                vrPlayer.moviePlane.SetActive(false);
                UI.SetActive(true);
                UI.GetComponent<UIManager>().FadeIn();
                BGM.clip = ParrotMorning;
                BGM.Play();
                marketBGM.Play();
                SteamVR_Fade.Start(Color.black, 0f);
                SteamVR_Fade.Start(Color.clear, 5f);

                angel.GetComponent<AngelAI>().teleport(angel.GetComponent<AngelAI>().parrot1.position);
                vrPlayer.blackS.SetActive(false);
                vrPlayer.cameraRig.transform.position = vrPos[1].position;
                vrPlayer.cameraRig.transform.rotation = vrPos[1].rotation;
                vrPlayer.cameraRig.transform.localScale = vrPos[1].localScale;
                vrPlayer.staticpos = vrPos[1];
                vrPlayer.isStatic = true;
                cameraPos.now_pos = 1;
                mainCamera.transform.position = cameraPos.ParrotPos.position;
                mainCamera.transform.rotation = cameraPos.ParrotPos.rotation;
                cameraPos.pos[1] = cameraPos.ParrotPos;
                parrot_body.layer = 9;
                parrot_feet.layer = 9;
                parrot.GetComponent<Parrot>().enabled = true;
                parrot.GetComponent<Parrot>().talkable = true;
                left_wing.GetComponent<Wing>().enabled = true;
                right_wing.GetComponent<Wing>().enabled = true;

                chapter = Chapter.Parrot_in_cage;
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                mainCamera.GetComponent<VideoPlayer>().Stop();
                vrPlayer.gameObject.GetComponent<VideoPlayer>().Stop();
            }
        }
        else if(chapter == Chapter.Parrot_in_cage)
        {
            angel.GetComponent<AngelAI>().target = angel.GetComponent<AngelAI>().parrot2;
            angel.GetComponent<AngelAI>().walk();

            if (parrot.GetComponent<Parrot>().backHome)
            {
                chapter = Chapter.Parrot_to_home;
                parrot.GetComponent<Parrot>().talkable = false;
                angel.GetComponent<AngelAI>().target = angel.GetComponent<AngelAI>().home;
                angel.GetComponent<AngelAI>().walk();
                vrPlayer.isStatic = false;
                vrPlayer.movewithbody = true;
                cameraPos.follow = true;
            }
        }
        else if (chapter == Chapter.Parrot_to_home)
        {
            Cage.transform.position = CagePosOnRoad.position;
            Vector3 parrotPos = CagePosOnRoad.position;
            parrotPos.x += 0.66782f;
            parrotPos.z -= 0.4113464f;
            parrot.transform.position = parrotPos;

            if((angel.transform.position - angel.GetComponent<AngelAI>().parrot2.position).magnitude > 20.0f)
            {
                if (UI.GetComponent<UIManager>().fading == false && Fade.color.a < 1.0f)
                {
                    UI.GetComponent<UIManager>().FadeOut();
                }
                if (UI.GetComponent<UIManager>().fading == false && Fade.color.a >= 1.0f)
                {
                    chapter = Chapter.Parrot;
                    mainCamera.transform.position = cameraPos.pos[0].position;
                    mainCamera.transform.rotation = cameraPos.pos[0].rotation;
                    angel.GetComponent<AngelAI>().teleport(angel.GetComponent<AngelAI>().home.position);
                    Cage.transform.position = CagePosAtHome.position;
                    parrotPos = CagePosAtHome.position;
                    parrotPos.x += 0.66782f;
                    parrotPos.z -= 0.4113464f;
                    parrot.transform.position = parrotPos;
                    parrot.GetComponent<Parrot>().openFlying = true;
                    parrot.GetComponent<Parrot>().talkable = true;
                    angel.GetComponent<AngelAI>().talk_count = 1;
                    marketBGM.Stop();
                    cameraPos.follow = false;
                    UI.GetComponent<UIManager>().isEndDay = true;
                    pcPlayer.day = true;
                    dayCount = 0;
                    UI.GetComponent<UIManager>().FadeIn();
                }
            }
        }
        else if (chapter == Chapter.Parrot)
        {
            if (!pcPlayer.day && angel.GetComponent<AngelAI>().isHome() && light_c.isDay)
            {
                light_c.DayToNight();
            }

            if (!pcPlayer.day && !light_c.isDay && !angel.GetComponent<AngelAI>().getplate && !angel.GetComponent<AngelAI>().getwater)
            {
                chapter = Chapter.Parrot_night;
                angel.GetComponent<AngelAI>().target = angel.GetComponent<AngelAI>().bed;
                angel.GetComponent<AngelAI>().walk();
                pcPlayer.NightChickCount = 0;
                parrot.GetComponent<Rigidbody>().velocity = Vector3.zero;
                Vector3 parrotPos = CagePosAtHome.position;
                parrotPos.x += 0.66782f;
                parrotPos.z -= 0.4113464f;
                parrot.transform.position = parrotPos;
                parrot.GetComponent<Parrot>().openFlying = false;
            }

            if(dayCount > 2 && right_wing.GetComponent<Wing>().dialog < 0)
            {
                if (angel.GetComponent<AngelAI>().love > 40.0)
                {
                    if(UI.GetComponent<UIManager>().fading == false && Fade.color.a < 1.0f)
                    {
                        UI.GetComponent<UIManager>().FadeOut();
                    }
                    else if(UI.GetComponent<UIManager>().fading == false && Fade.color.a >= 1.0f)
                    {
                        parrot.SetActive(false);
                        vrPlayer.isStatic = false;
                        vrPlayer.cameraRig.transform.localScale = vrPos[0].localScale;
                        vrPlayer.cameraRig.transform.position += new Vector3(0.0f, 1000.0f, 0.0f);
                        vrPlayer.blackS.SetActive(true);
                        mainCamera.transform.position = tramsPos.position;
                        mainCamera.transform.rotation = tramsPos.rotation;
                        pcPlayer.day = true;
                        SteamVR_Fade.Start(Color.black, 0f);
                        SteamVR_Fade.Start(Color.clear, 5f);
                        BGM.Stop();
                        mainCamera.GetComponent<VideoPlayer>().enabled = true;
                        mainCamera.GetComponent<VideoPlayer>().clip = parrotDead;
                        vrPlayer.gameObject.GetComponent<VideoPlayer>().clip = parrotDead;
                        vrPlayer.moviePlane.SetActive(true);
                        vrPlayer.movewithbody = false;

                        chapter = Chapter.Parrot_end_prepare;
                    }
                }
                else
                {
                    gameOver = true;
                    UI.GetComponent<UIManager>().over.SetActive(true);
                    UI.GetComponent<UIManager>().because.text = "沒有達到好感度條件";
                }
            }

        }
        else if(chapter == Chapter.Parrot_night)
        {
            parrot.GetComponent<Rigidbody>().velocity = Vector3.zero;
            parrot.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            if (!light_c.isDay && pcPlayer.NightChickCount > 2 && UI.GetComponent<UIManager>().fading == false && Fade.color.a < 1.0f)
            {
                Debug.Log("ParrotDay");
                light_c.NightToDay();
                UI.GetComponent<UIManager>().FadeOut();
                dayCount++;
            }
            if (pcPlayer.NightChickCount > 2 && Fade.color.a >= 1.0f)
            {
                if (dayCount > 2)
                {
                    parrot.GetComponent<Parrot>().end = true;
                }
                mainCamera.transform.position = cameraPos.pos[0].position;
                mainCamera.transform.rotation = cameraPos.pos[0].rotation;
                chapter = Chapter.Parrot;
                angel.GetComponent<AngelAI>().teleport(angel.GetComponent<AngelAI>().home.position);
                angel.GetComponent<AngelAI>().calculateProb();
                angel.GetComponent<AngelAI>().talk_count = 1;
                UI.GetComponent<UIManager>().isEndDay = true;
                UI.GetComponent<UIManager>().FadeIn();
                pcPlayer.day = true;
                parrot.GetComponent<Parrot>().openFlying = true;
            }
        }
        else if (chapter == Chapter.Parrot_end_prepare)
        {
            mainCamera.GetComponent<VideoPlayer>().enabled = true;
            if (mainCamera.GetComponent<VideoPlayer>().isPrepared && vrPlayer.gameObject.GetComponent<VideoPlayer>().isPrepared)
            {
                mainCamera.GetComponent<VideoPlayer>().Play();
                vrPlayer.gameObject.GetComponent<VideoPlayer>().Play();
                chapter = Chapter.Parrot_end;
            }
        }
        else if (chapter == Chapter.Parrot_end)
        {
            if (mainCamera.GetComponent<VideoPlayer>().isPlaying)
            {
                UI.SetActive(false);
                moviePlaying = true;
            }
            if (mainCamera.GetComponent<VideoPlayer>().isPlaying == false && moviePlaying)
            {
                moviePlaying = false;
                mainCamera.GetComponent<VideoPlayer>().enabled = false;
                vrPlayer.moviePlane.SetActive(false);
                UI.SetActive(true);
                UI.GetComponent<UIManager>().FadeIn();
                BGM.clip = transformBGM;
                BGM.Play();
                SteamVR_Fade.Start(Color.black, 0f);
                SteamVR_Fade.Start(Color.clear, 5f);
                statue.SetActive(true);
                statue_hand.SetActive(true);
                chapter = Chapter.Transform;
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                mainCamera.GetComponent<VideoPlayer>().Stop();
                vrPlayer.gameObject.GetComponent<VideoPlayer>().Stop();
            }
        }
        else if (chapter == Chapter.Cat_begin_prepare)
        {
            mainCamera.GetComponent<VideoPlayer>().enabled = true;
            if (mainCamera.GetComponent<VideoPlayer>().isPrepared && vrPlayer.gameObject.GetComponent<VideoPlayer>().isPrepared)
            {
                mainCamera.GetComponent<VideoPlayer>().Play();
                vrPlayer.gameObject.GetComponent<VideoPlayer>().Play();
                chapter = Chapter.Cat_begin;
                
            }
        }
        else if (chapter == Chapter.Cat_begin)
        {
            if (mainCamera.GetComponent<VideoPlayer>().isPlaying)
            {
                UI.SetActive(false);
                moviePlaying = true;
            }
            if (mainCamera.GetComponent<VideoPlayer>().isPlaying == false && moviePlaying)
            {
                moviePlaying = false;
                mainCamera.GetComponent<VideoPlayer>().enabled = false;
                vrPlayer.moviePlane.SetActive(false);
                UI.SetActive(true);
                UI.GetComponent<UIManager>().FadeIn();
                BGM.clip = CatMorning;
                BGM.Play();
                marketBGM.Play();
                SteamVR_Fade.Start(Color.black, 0f);
                SteamVR_Fade.Start(Color.clear, 5f);

                //angel.GetComponent<AngelAI>().teleport(angel.GetComponent<AngelAI>().parrot1.position);
                vrPlayer.blackS.SetActive(false);
                vrPlayer.cameraRig.transform.position = vrPos[2].position;
                vrPlayer.cameraRig.transform.rotation = vrPos[2].rotation;
                vrPlayer.cameraRig.transform.localScale = vrPos[2].localScale;
                vrPlayer.staticpos = vrPos[2];
                vrPlayer.isStatic = false;
                vrPlayer.movewithbody = true;
                cameraPos.now_pos = 1;
                mainCamera.transform.position = cameraPos.CatPos.position;
                mainCamera.transform.rotation = cameraPos.CatPos.rotation;
                cameraPos.pos[1] = cameraPos.CatPos;
                left_claw.SetActive(true);
                right_claw.SetActive(true);
                chapter = Chapter.Cat;
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                mainCamera.GetComponent<VideoPlayer>().Stop();
                vrPlayer.gameObject.GetComponent<VideoPlayer>().Stop();
            }
        }
        else if(chapter == Chapter.Cat)
        {
            if(left_claw.GetComponent<Claw>().end || right_claw.GetComponent<Claw>().end)
            {
                if (UI.GetComponent<UIManager>().fading == false && Fade.color.a < 1.0f)
                {
                    UI.GetComponent<UIManager>().FadeOut();
                }
                else if (UI.GetComponent<UIManager>().fading == false && Fade.color.a >= 1.0f)
                {
                    cat.SetActive(false);
                    left_claw.SetActive(false);
                    right_claw.SetActive(false);

                    vrPlayer.isStatic = false;
                    vrPlayer.cameraRig.transform.localScale = vrPos[0].localScale;
                    vrPlayer.cameraRig.transform.position += new Vector3(0.0f, 1000.0f, 0.0f);
                    vrPlayer.blackS.SetActive(true);
                    mainCamera.transform.position = tramsPos.position;
                    mainCamera.transform.rotation = tramsPos.rotation;
                    pcPlayer.day = true;
                    SteamVR_Fade.Start(Color.black, 0f);
                    SteamVR_Fade.Start(Color.clear, 5f);
                    BGM.Stop();
                    mainCamera.GetComponent<VideoPlayer>().enabled = true;
                    mainCamera.GetComponent<VideoPlayer>().clip = catDead;
                    vrPlayer.gameObject.GetComponent<VideoPlayer>().clip = catDead;
                    vrPlayer.moviePlane.SetActive(true);
                    vrPlayer.movewithbody = false;

                    chapter = Chapter.Cat_end_prepare;
                }
            }
        }
        else if (chapter == Chapter.Cat_end_prepare)
        {
            mainCamera.GetComponent<VideoPlayer>().enabled = true;
            if (mainCamera.GetComponent<VideoPlayer>().isPrepared && vrPlayer.gameObject.GetComponent<VideoPlayer>().isPrepared)
            {
                mainCamera.GetComponent<VideoPlayer>().Play();
                vrPlayer.gameObject.GetComponent<VideoPlayer>().Play();
                chapter = Chapter.Cat_end;
            }
        }
        else if (chapter == Chapter.Cat_end)
        {
            if (mainCamera.GetComponent<VideoPlayer>().isPlaying)
            {
                UI.SetActive(false);
                moviePlaying = true;
            }
            if (mainCamera.GetComponent<VideoPlayer>().isPlaying == false && moviePlaying)
            {
                moviePlaying = false;
                mainCamera.GetComponent<VideoPlayer>().enabled = false;
                vrPlayer.moviePlane.SetActive(false);
                UI.SetActive(true);
                UI.GetComponent<UIManager>().FadeIn();
                BGM.clip = transformBGM;
                BGM.Play();
                SteamVR_Fade.Start(Color.black, 0f);
                SteamVR_Fade.Start(Color.clear, 5f);
                statue_human.SetActive(true);
                statue_hand.SetActive(true);
                chapter = Chapter.Transform_human;
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                mainCamera.GetComponent<VideoPlayer>().Stop();
                vrPlayer.gameObject.GetComponent<VideoPlayer>().Stop();
            }
        }
        else if(chapter == Chapter.Transform_human)
        {
            if (trans.isTrans)
            {
                if (UI.GetComponent<UIManager>().fading == false && Fade.color.a < 1.0f)
                {
                    UI.GetComponent<UIManager>().FadeOut();
                    BGM.Stop();
                    statue_hand.SetActive(false);
                }

                if (Fade.color.a >= 1.0f)
                {
                    statue_human.SetActive(false);
                    mainCamera.GetComponent<VideoPlayer>().enabled = true;
                    mainCamera.GetComponent<VideoPlayer>().clip = TheEnd;
                    vrPlayer.gameObject.GetComponent<VideoPlayer>().clip = TheEnd;
                    vrPlayer.moviePlane.SetActive(true);
                    trans.isTrans = false;
                    chapter = Chapter.The_End_prepare;
                }
            }
        }
        if (chapter == Chapter.The_End_prepare)
        {
            mainCamera.GetComponent<VideoPlayer>().enabled = true;
            if (mainCamera.GetComponent<VideoPlayer>().isPrepared && vrPlayer.gameObject.GetComponent<VideoPlayer>().isPrepared)
            {
                mainCamera.GetComponent<VideoPlayer>().Play();
                vrPlayer.gameObject.GetComponent<VideoPlayer>().Play();
                chapter = Chapter.The_End;

            }
        }
        else if (chapter == Chapter.The_End)
        {
            if (mainCamera.GetComponent<VideoPlayer>().isPlaying)
            {
                UI.SetActive(false);
                moviePlaying = true;
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                mainCamera.GetComponent<VideoPlayer>().Stop();
                vrPlayer.gameObject.GetComponent<VideoPlayer>().Stop();
            }
        }

        #endregion

        // for cheating
        if (Input.GetKeyDown(KeyCode.A))
        {
            angel.GetComponent<AngelAI>().love += 4.0f;
            UI.GetComponent<UIManager>().showFeeling = UIManager.Emoji.Happy;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            enemyManager.nestLife++;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            if(trans.charactor == Trans.Charactor.Parrot)
            {
                parrot.SetActive(false);
                vrPlayer.cameraRig.transform.localScale = vrPos[0].localScale;
            }

            tree.SetActive(false);
            statue.SetActive(true);
            statue_hand.SetActive(true);
            vrPlayer.isStatic = false;
            vrPlayer.cameraRig.transform.position += new Vector3(0.0f, 1000.0f, 0.0f);
            vrPlayer.blackS.SetActive(true);
            mainCamera.transform.position = tramsPos.position;
            mainCamera.transform.rotation = tramsPos.rotation;
            UI.GetComponent<UIManager>().FadeIn();
            pcPlayer.day = true;
            enemyManager.end = false;
            SteamVR_Fade.Start(Color.black, 0f);
            SteamVR_Fade.Start(Color.clear, 5f);
            BGM.clip = transformBGM;
            BGM.Play();
            chapter = Chapter.Transform;
        }
    }
}
