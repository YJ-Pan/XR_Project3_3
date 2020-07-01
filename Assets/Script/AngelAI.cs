using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AngelAI : MonoBehaviour
{
    enum AngelAction
    {
        Idle,
        SeeFish,
        Harvest,
        FeedChicken,
        PlayQin,
        GoOut
    }

    public GameManager gameManager;
    public UIManager uIManager;

    [Header("Audio")]
    public AudioSource GuzhengBGM;
    public AudioSource wellBGM;

    [Header("Destination")]
    public Transform tree;
    public Transform well;
    public Transform afterwell;
    public Transform ChickenPos;
    public Transform plateshop;
    public Transform home;
    public Transform bed;
    public Transform waterbank;
    public Transform plate;
    public Transform parrot1;
    public Transform parrot2;

    public Transform chicken;
    public Transform farm;
    public Transform pool;
    public Transform qin;
    public Transform idle;

    [Header("Don't Change!")]
    public Transform target;
    private NavMeshAgent agent;
    public Animator anim;

    public GameObject SleepAngel;
    public GameObject fruit = null;
    public GameObject pickfruit;
    public bool chickenOut = false;
    public bool pickChicken = false;
    private bool isWalk = false;
    private bool leave = false;

    public float love = 0.0f;
    public float scare = 0.0f;
    public float goOut_tree = 0.0f;
    public float goOut_plateshop = 0.0f;
    public float goOut_water = 0.0f;
    public bool getwater = false;
    public bool fillwater = false;
    public bool getplate = false;
    public bool fillplate = false;

    public int talk_count = 0;

    private AngelAction angelAction;
    private List<AngelAction> angelAtHome;
    private List<AngelAction> angelAtHomeScare;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        SleepAngel.GetComponent<Animator>().SetBool("Sleep", true);
        SkinnedMeshRenderer[] comps = SleepAngel.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer comp in comps)
        {
            comp.enabled = false;
        }

        #region creat angelAtHome
        angelAtHome = new List<AngelAction>();
        angelAtHome.Add(AngelAction.Idle);
        angelAtHome.Add(AngelAction.Idle);
        angelAtHome.Add(AngelAction.Idle);
        angelAtHome.Add(AngelAction.FeedChicken);
        angelAtHome.Add(AngelAction.FeedChicken);
        angelAtHome.Add(AngelAction.Harvest);
        angelAtHome.Add(AngelAction.Harvest);
        angelAtHome.Add(AngelAction.PlayQin);
        angelAtHome.Add(AngelAction.SeeFish);
        angelAtHome.Add(AngelAction.SeeFish);
        #endregion

        #region creat angelAtHomeScare
        angelAtHomeScare = new List<AngelAction>();
        angelAtHomeScare.Add(AngelAction.Idle);
        angelAtHomeScare.Add(AngelAction.Idle);
        angelAtHomeScare.Add(AngelAction.Idle);
        angelAtHomeScare.Add(AngelAction.Idle);
        angelAtHomeScare.Add(AngelAction.FeedChicken);
        angelAtHomeScare.Add(AngelAction.Harvest);
        angelAtHomeScare.Add(AngelAction.PlayQin);
        angelAtHomeScare.Add(AngelAction.PlayQin);
        angelAtHomeScare.Add(AngelAction.PlayQin);
        angelAtHomeScare.Add(AngelAction.SeeFish);
        #endregion

        angelAction = AngelAction.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        #region ensure love & scare
        if (love < 0.0f)
            love = 0.0f;
        if (love > uIManager.loveMax)
            love = uIManager.loveMax;
        if (scare < 0.0f)
            scare = 0.0f;
        if (scare > uIManager.scareMax)
            scare = uIManager.scareMax;
        #endregion

        if (fruit != null)
        {
            if (!fruit.activeSelf || fruit.tag != "Fruit")
            {
                fruit = null;
                stopWalk();
            }
            else
            {
                target = fruit.transform;
                walk();
            }
        }

        if (isWalk)
        {
            Vector3 dis = target.position - gameObject.transform.position;
            dis.y = 0.0f;
            if (dis.magnitude < 0.3)
            {
                Debug.Log(angelAction);

                if (target == tree)
                {
                    anim.Play("Sit_ground");
                }
                else if(target == well)
                {
                    getwater = true;
                    wellBGM.Play();
                    pickfruit.SetActive(false);
                    StartCoroutine(wellBGMPlay());
                }
                else if(target == plateshop)
                {
                    getplate = true;
                }
                else if (target == waterbank)
                {
                    getwater = false;
                    fillwater = true;
                }
                else if (target == plate)
                {
                    getplate = false;
                    fillplate = true;
                }

                stopWalk();

                if (gameManager.chapter == GameManager.Chapter.Tree && gameManager.pcPlayer.day)
                {
                    if (angelAction == AngelAction.FeedChicken || angelAction == AngelAction.Harvest)
                    {
                        anim.Play("Grab_from_floor");
                        return;
                    }
                    else if (angelAction == AngelAction.SeeFish)
                    {
                        gameObject.transform.rotation = pool.rotation;
                        anim.Play("Sit_chair");
                        return;
                    }
                    else if (angelAction == AngelAction.PlayQin)
                    {
                        gameObject.transform.rotation = qin.rotation;
                        anim.Play("Play_piano");
                        GuzhengBGM.Play();
                        scare -= 40.0f;
                        return;
                    }
                    else if (angelAction == AngelAction.Idle)
                    {
                        anim.Play("Stand");
                        return;
                    }
                }
                
                if (chickenOut)
                {
                    chickenOut = false;
                    pickChicken = true;
                }

                if(fruit != null)
                {
                    anim.Play("Grab_from_floor");
                    if (fruit.name == "Apple")
                    {
                        love += 3.0f;
                        uIManager.showFeeling = UIManager.Emoji.Happy;
                    }
                    else if(fruit.name == "Banana")
                    {
                        love -= 1.0f;
                        uIManager.showFeeling = UIManager.Emoji.Dissatisfied;
                    }
                    else if (fruit.name == "Kiwi")
                    {
                        love += 1.0f;
                        uIManager.showFeeling = UIManager.Emoji.Happy;
                    }
                    else if (fruit.name == "Pear")
                    {
                        love += 5.0f;
                        uIManager.showFeeling = UIManager.Emoji.Happy;
                    }
                    else if (fruit.name == "Strawberry")
                    {
                        uIManager.showFeeling = UIManager.Emoji.Confused;
                    }

                    fruit.SetActive(false);
                    fruit = null;
                }

                if (gameManager.chapter == GameManager.Chapter.Tree_night || gameManager.chapter == GameManager.Chapter.Parrot_night)
                    setSleep(true);

                if (gameManager.chapter == GameManager.Chapter.Tree && angelAction == AngelAction.GoOut)
                    StartCoroutine("Leave");
            }
        }

        #region IF Angle at home and Idle
        if (gameManager.chapter == GameManager.Chapter.Tree
            && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !isWalk && angelAction != AngelAction.GoOut && !getwater && !getplate)
        {
            AngelAction newAction;

            if (scare < 100)
            {
                angelAtHome = RandomSortList(angelAtHome);
                newAction = angelAtHome[0];
            }
            else
            {
                angelAtHomeScare = RandomSortList(angelAtHomeScare);
                newAction = angelAtHomeScare[0];
            }

            if (newAction == angelAction && newAction != AngelAction.Idle)
            {
                return;
            }

            //Debug.Log(newAction);

            if (newAction == AngelAction.FeedChicken)
            {
                target = chicken;
                walk();
            }
            else if (newAction == AngelAction.Harvest)
            {
                target = farm;
                walk();
            }
            else if (newAction == AngelAction.SeeFish)
            {
                target = pool;
                walk();
            }
            else if (newAction == AngelAction.PlayQin)
            {
                target = qin;
                walk();
            }
            else if (newAction != angelAction)
            {
                target = idle;
                walk();
            }
            else
            {
                anim.Play("Stand");
                return;
            }

            angelAction = newAction;
        }
        #endregion

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Play_piano"))
        {
            GuzhengBGM.Stop();
        }

        if (anim.GetBool("Sleep"))
            scare -= 0.1f;
    }

    public void walk()
    {
        if (target != ChickenPos && chickenOut)
            chickenOut = false;

        anim.SetBool("Idle", false);
        if (chickenOut)
        {
            agent.speed = 6.0f;
            anim.SetBool("Walk", false);
            anim.SetBool("Run", true);
        }    
        else
        {
            agent.speed = 2.0f;
            anim.SetBool("Run", false);
            anim.SetBool("Walk", true);
        }
            

        isWalk = true;
        agent.ResetPath();
        agent.SetDestination(target.position);
        if(leave)
            StopCoroutine("Leave");
    }

    public void stopWalk()
    {
        if (target == home || target == waterbank || target == plate)
        {
            angelAction = AngelAction.Idle;
        }
        agent.isStopped = true;
        agent.ResetPath();
        anim.SetBool("Walk", false);
        anim.SetBool("Run", false);
        anim.SetBool("Idle", true);
        isWalk = false;
        target = null;
    }

    void setSleep(bool isSleep)
    { 
        anim.SetBool("Sleep", isSleep);
        SkinnedMeshRenderer[] comps1 = SleepAngel.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer comp in comps1)
        {
            comp.enabled = isSleep;
        }
        SkinnedMeshRenderer[] comps2 = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach(SkinnedMeshRenderer comp in comps2)
        {
            comp.enabled = !isSleep;
        }
    }

    public void teleport(Vector3 pos)
    {
        agent.Warp(pos);
        setSleep(false);
    }

    public void calculateProb()
    {
        if (gameManager.chapter == GameManager.Chapter.Tree && angelAction != AngelAction.GoOut)
        {
            if (chickenOut)
            {
                target = ChickenPos;
                angelAction = AngelAction.GoOut;
                anim.Play("Idle");
                walk();
            }
            else if (goOut_tree / Random.Range(30.0f, 100.0f) >= 1.0f)
            {
                target = tree;
                goOut_tree = 0.0f;
                angelAction = AngelAction.GoOut;
                anim.Play("Idle");
                walk();
            }
            else if (goOut_plateshop / Random.Range(30.0f, 100.0f) >= 1.0f)
            {
                target = plateshop;
                goOut_plateshop = 0.0f;
                angelAction = AngelAction.GoOut;
                anim.Play("Idle");
                walk();
            }
            else if (goOut_water / Random.Range(30.0f, 100.0f) >= 1.0f)
            {
                target = well;
                goOut_water = 0.0f;
                angelAction = AngelAction.GoOut;
                anim.Play("Idle");
                walk();
            }
        }
    }

    public List<T> RandomSortList<T>(List<T> ListT)
    {
        List<T> newList = new List<T>();
        int countNum = ListT.Count;
        while (newList.Count < countNum)
        {
            int index = Random.Range(0, ListT.Count - 1);
            newList.Add(ListT[index]);
            ListT.Remove(ListT[index]);
        }
        return newList;
    }

    IEnumerator Leave()
    {
        leave = true;
        yield return new WaitForSeconds(5.0f);
        if (getwater)
        {
            target = waterbank;
        }
        else if (getplate)
        {
            target = plate;
        }
        else
        {
            target = home;
        }
        
        leave = false;
        walk();
    }

    IEnumerator wellBGMPlay()
    {
        yield return new WaitForSeconds(5.2f);
        wellBGM.Stop();
        pickfruit.SetActive(true);
        target = afterwell;
        walk();
    }

    public bool isHome()
    {
        Vector3 dis = gameObject.transform.position - home.position;
        dis.y = 0.0f;
        return dis.magnitude < 6.0f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Branch") && anim.GetCurrentAnimatorStateInfo(0).IsName("Sit_ground"))
        {
            love += 0.1f;
            Debug.Log("Love: " + love);
        }
    }
}
