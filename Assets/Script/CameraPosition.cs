using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    public List<Transform> pos;
    public int now_pos;
    public AudioSource UISource;
    public AudioClip change;


    // Start is called before the first frame update
    void Start()
    {
        now_pos = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPos()
    {
        UISource.PlayOneShot(change);
        if (now_pos < pos.Count - 1)
        {
            now_pos++;
        }
        else
        {
            now_pos = 0;
        }
        gameObject.transform.position = pos[now_pos].position;
        gameObject.transform.rotation = pos[now_pos].rotation;
    }
}
