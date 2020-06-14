using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scenario : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject dialog;
    public GameObject dialogText;
    private List<string> sentence = new List<string>();
    public int count;
    private bool talk;
    private GameManager.Chapter now;
    private System.IO.StreamReader sr;

    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        talk = false;
        now = GameManager.Chapter.Begin_movie;
        readFile(now.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        if (now != gameManager.chapter)
        {
            count = 0;
            now = gameManager.chapter;
            readFile(now.ToString());

            if (sentence.Count > 0)
            {
                talk = true;
                dialog.SetActive(true);
                dialogText.GetComponent<TypewriterEffect>().newWords = sentence[count];
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && talk)
        {  
            if (dialogText.GetComponent<TypewriterEffect>().isActive)
            {
                dialogText.GetComponent<TypewriterEffect>().OnFinish();
            }
            else
            {
                count++;

                if (count < sentence.Count)
                {
                    dialogText.GetComponent<TypewriterEffect>().newWords = sentence[count];
                }
                else
                {
                    talk = false;
                    dialogText.GetComponent<Text>().text = "";
                    dialog.SetActive(false);
                }
            }
        }
    }

    private void readFile(string filename)
    {
        sentence.Clear();
        TextAsset textFile = Resources.Load<TextAsset>("Story/" + filename);
        if (textFile == null)
            return;

        string text = textFile.text;
        string[] textArr = text.Split('#');
        foreach(string line in textArr)
        {
            sentence.Add(line);
        }

        /*string filepath = "Assets/Resources/Story/" + filename + ".txt";
        //var temp = Resources.Load(filename).ToString();
        if (!System.IO.File.Exists(filepath))
            return;

        sr = new System.IO.StreamReader(@filepath);
        string line = "";
        while (!sr.EndOfStream)
        {
            string temp = sr.ReadLine();
            
            if (temp == "#")
            {
                sentence.Add(line);
                line = "";
            }
            else
            {
                line += temp + '\n';
            }
        }
        sr.Close();*/
    }
}
