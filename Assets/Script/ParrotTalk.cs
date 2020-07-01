using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

struct ADialog
{
    public string question;
    public int audioID;
    public string[] choice;
    public string choice_2;
    public string choice_3;
    public int[] love;
    public int love_2;
    public int love_3;
};

public class ParrotTalk : MonoBehaviour
{
    // Start is called before the first frame update
    public List<AudioClip> audio;
    public Text text1;
    public Text text2;
    public Text text3;
    public int[] love_added = new int[] { 0, 0, 0 };

    private List<ADialog> Dialogs = new List<ADialog>();

    void Start()
    {
        readFile("Parrot_Dialog");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateDialog()
    {
        Debug.Log(Dialogs.Count);
        if (Dialogs.Count < 1) return;
        int index = Random.Range(0, Dialogs.Count - 1);
        Debug.Log(Dialogs[index].question);
        gameObject.GetComponent<AudioSource>().PlayOneShot(audio[Dialogs[index].audioID]);
        text1.text = Dialogs[index].choice[0];
        text2.text = Dialogs[index].choice[1];
        text3.text = Dialogs[index].choice[2];
        love_added = Dialogs[index].love;
        Dialogs.RemoveAt(index);
    }

    public void startDialog()
    {
        text1.text = "女人，你引起了我的注意";
        text2.text = "熱呼呼的鸚鵡唷";
        text3.text = "(老闆)高雄，發大財";
        love_added[0] = 3;
        love_added[1] = 5;
        love_added[2] = 7;
    }

    public void endDialog()
    {
        gameObject.GetComponent<AudioSource>().PlayOneShot(audio[4]);
        text1.text = "我們去爬山吧";
        text2.text = "我們去爬山吧";
        text3.text = "我們去爬山吧";
        love_added[0] = 0;
        love_added[1] = 0;
        love_added[2] = 0;
    }

    private void readFile(string filename)
    {
        Dialogs.Clear();
        TextAsset textFile = Resources.Load<TextAsset>("Story/" + filename);
        if (textFile == null)
        {
            Debug.Log("OpenFileError");
            return;
        }

        string text = textFile.text;
        string[] textArr = text.Split('#');
        foreach (string paragraph in textArr)
        {
            ADialog aDialog = new ADialog();
            aDialog.choice = new string[] { "", "", "" };
            aDialog.love = new int[] { 0, 0, 0 };

            string[] lineArr = paragraph.Split('\n');
            aDialog.question = lineArr[0];
            aDialog.audioID = int.Parse(lineArr[1]);
            for (int i=2;i<lineArr.Length;i+=2)
            {
                aDialog.choice[(i / 2)-1] = lineArr[i];
                aDialog.love[(i / 2) - 1] = int.Parse(lineArr[i + 1]);
            }
            Dialogs.Add(aDialog);
            //Debug.Log(aDialog.question + " " + aDialog.choice[0] + " " + aDialog.choice[1] + " " + aDialog.choice[2] + " " + aDialog.love[2]);
        }
    }
}
