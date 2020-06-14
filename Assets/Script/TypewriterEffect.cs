using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TypewriterEffect : MonoBehaviour {

    public float charsPerSecond;
    public string newWords;
    private string words;

    public bool isActive = false;
    private float timer;
    private Text myText;
    private int currentPos=0;

    // Use this for initialization
    void Start () {
        timer = 0;
        isActive = true;
        myText = GetComponent<Text>();
        words = newWords;
        myText.text = "";
    }

    // Update is called once per frame
    void Update () {
        /*if(!isActive)
        {
            if(newWords != words)
            {
                OnFinish();
                timer = 0;
                isActive = true;
                words = newWords;
                myText.text = "";
            }
        }*/
        if (newWords != words)
        {
            OnFinish();
            timer = 0;
            isActive = true;
            words = newWords;
            myText.text = "";
        }
        OnStartWriter();
    }

    public void StartEffect(){
        isActive = true;
    }
    
    void OnStartWriter(){

        if(isActive){
            timer += Time.deltaTime;
            if(timer>=charsPerSecond){
                timer = 0;
                
                if(currentPos>=words.Length) {
                    OnFinish();
                    return;
                }
                currentPos++;
                myText.text = words.Substring(0, currentPos);
            }

        }
    }
    
    public void OnFinish(){
        isActive = false;
        timer = 0;
        currentPos = 0;
        myText.text = words;
    }
}