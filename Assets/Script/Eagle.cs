using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    public float speed = 1.0f;
    public AudioClip scream;
    Vector3 orig_pos;

    public bool hitBird = false;

    int life = 5;

    void Start()
    {
        orig_pos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = target.position;
        gameObject.transform.LookAt(dir);
        if(Vector3.Distance(dir, gameObject.transform.position) > 1.0f)
        {
            gameObject.transform.position += speed * gameObject.transform.forward;
        }

        if (!gameObject.GetComponent<AudioSource>().isPlaying)
        {
            gameObject.GetComponent<AudioSource>().Play();
        }

        if (gameObject.tag == "lb_bird")
        {
            gameObject.GetComponent<Animator>().SetBool("flying", true);
            
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Branch") && gameObject.tag != "lb_bird") 
        {
            life -= 1;
            gameObject.GetComponentInChildren<AudioSource>().PlayOneShot(scream);
            gameObject.transform.position -= 12.0f * gameObject.transform.forward;
            if (life == 0)
            {
                Destroy(gameObject);
            }
        }
        else if (other.CompareTag("Nest") && gameObject.tag != "lb_bird")
        {
            hitBird = true;
        }
    }

    
}
