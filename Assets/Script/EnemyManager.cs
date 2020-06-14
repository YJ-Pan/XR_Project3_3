using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //public GameManager gameManager;
    public GameObject eagle;
    public Transform treePos;
    public float InstantiateTime = 1.0f;

    public bool generate = false;
    public bool end = false;
    private int Count = 0;
    public int nestLife = 3;

    GameObject childEagle;
    bool instantiateOn = false;

    // Start is called before the first frame update
    void Start()
    {
        childEagle = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Count > 1 && childEagle == null)
        {
            Count = 0;
            end = true;
            generate = false;
        }
        if (generate && childEagle == null && !instantiateOn)
        {
            instantiateOn = true;
            StartCoroutine("InstantiateEagle");
        }
        if(childEagle != null)
        {
            if(childEagle.GetComponent<Eagle>().hitBird)
            {
                nestLife--;
                Destroy(childEagle);
            }
        }
    }

    IEnumerator InstantiateEagle()
    {
        yield return new WaitForSeconds(InstantiateTime);
        Vector3 pos = treePos.position;
        pos.y += 50.0f;
        pos.x += Random.Range(-100.0f, 100.0f);
        pos.z += Random.Range(-100.0f, 100.0f);
        childEagle = Instantiate(eagle, pos, Quaternion.identity);
        childEagle.GetComponent<Eagle>().target = treePos;
        childEagle.SetActive(true);
        instantiateOn = false;
        Count++;
    }
}
