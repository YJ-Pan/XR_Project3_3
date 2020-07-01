using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Trans : MonoBehaviour
{
    public enum Charactor
    {
        Tree,
        Parrot,
        Cat
    }

    public Charactor charactor;
    public bool isTrans = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GhostFire"))
        {
            if(charactor == Charactor.Tree)
                charactor = Charactor.Parrot;
            else if(charactor == Charactor.Parrot)
                charactor = Charactor.Cat;

            isTrans = true;
        }
    }
}
