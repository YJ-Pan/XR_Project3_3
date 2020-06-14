using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AngelAI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameManager gameManager;

    [Header("Destination")]
    public Transform tree;
    public Transform ChickenPos;
    public Transform street;
    public Transform home;
    public Transform bed;

    [Header("Don't Change!")]
    public Transform target;
    private NavMeshAgent agent;
    private Animator anim;

    public GameObject SleepAngel;
    public GameObject fruit = null;
    public bool chickenOut = false;
    public bool pickChicken = false;
    public bool noBowl = false;
    private bool isWalk = false;
    private bool leave = false;

    public float love = 0.0f;
    public float scare = 0.0f;
    public float goOut = 0.0f;


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
    }

    // Update is called once per frame
    void Update()
    {

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
                stopWalk();
                if (chickenOut)
                {
                    chickenOut = false;
                    pickChicken = true;
                }

                if(fruit != null)
                {
                    if (fruit.name == "Apple")
                    {
                        love += 3.0f;
                    }
                    else if(fruit.name == "Banana")
                    {
                        love -= 1.0f; 
                    }
                    else if (fruit.name == "Kiwi")
                    {
                        love += 1.0f;
                    }
                    else if (fruit.name == "Pear")
                    {
                        love += 5.0f;
                    }
                    
                    Debug.Log("Love: " + love);
                    fruit.SetActive(false);
                    fruit = null;
                }
                if (gameManager.chapter == GameManager.Chapter.Tree_night)
                    setSleep(true);

                if (gameManager.chapter == GameManager.Chapter.Tree)
                    StartCoroutine("Leave");
            }
        }

        if (anim.GetBool("Sleep"))
            scare -= 0.01f;
    }

    public void walk()
    {
        if (target != ChickenPos && chickenOut)
            chickenOut = false;

        if (chickenOut)
            agent.speed = 6.0f;
        else
            agent.speed = 4.0f;

        isWalk = true;
        agent.ResetPath();
        agent.SetDestination(target.position);
        anim.SetBool("Walk", true);
        anim.SetBool("Idle", false);
        if(leave)
            StopCoroutine("Leave");
    }

    public void stopWalk()
    {
        agent.isStopped = true;
        agent.ResetPath();
        anim.SetBool("Walk", false);
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
        if (gameManager.chapter == GameManager.Chapter.Tree)
        {
            float prob = goOut / Random.Range(30.0f, 100.0f);
            Debug.Log(prob);
            if (prob > 1.0f)
            {
                if(chickenOut)
                    target = ChickenPos;
                else
                    target = tree;
                walk();
                goOut = 0.0f;
            }
        }
    }


    IEnumerator Leave()
    {
        leave = true;
        yield return new WaitForSeconds(5.0f);
        target = home;
        leave = false;
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
        if (other.CompareTag("Branch"))
        {
            love += 0.01f;
            Debug.Log("Love: " + love);
        }
    }
}
