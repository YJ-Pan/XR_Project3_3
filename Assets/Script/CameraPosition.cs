using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    public List<Transform> pos;
    public Transform ParrotPos;
    public Transform CatPos;

    public int now_pos;
    public AudioSource UISource;
    public AudioClip change;
    public Transform Angel;

    public bool follow = false;

    // Start is called before the first frame update
    void Start()
    {
        now_pos = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (follow)
        {
            gameObject.transform.position = new Vector3(Angel.position.x - 3.0f, Angel.position.y + 3.0f, Angel.position.z);
            gameObject.transform.LookAt(new Vector3(Angel.position.x, Angel.position.y + 1.0f, Angel.position.z));
        }
    }

    public void SetPos()
    {
        follow = false;
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

    public void FollowAngel()
    {
        follow = true;
        UISource.PlayOneShot(change);
    }
}
