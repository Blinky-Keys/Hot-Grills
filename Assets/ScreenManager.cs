using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Networking;

public class ScreenManager : MonoBehaviour
{
    private int score;
    public TextMeshProUGUI scoreText;
    string line;


    // Start is called before the first frame update
    void Start()
    {
        line = "";
        using(StreamReader sr = new StreamReader(Application.dataPath + "/score.txt"))
        {
            line = sr.ReadLine();
        }

        scoreText.text = "Final score: " + line;

        //Upload score to server
        //StartCoroutine(UploadScore(line));

        //UploadScore();

        File.Delete(Application.dataPath + "/score.txt");

    }

    public void BackToMain()
    {
        //Load the main menu
        SceneManager.LoadScene(0);
    }

    public void UploadScoreAndName()
    {
        string playerName = "";
        GameObject nameBox = GameObject.Find("NameBox");
        playerName = nameBox.transform.Find("Text Area").transform.Find("Text").GetComponent<TextMeshProUGUI>().text;

        StartCoroutine(UploadScore(line, playerName));
    }

    IEnumerator UploadScore(string scoreText, string playerName)
    {
        //Create new form to send data in
        WWWForm form = new WWWForm();

        //Add the data
        if (playerName.Length <= 1)
        {
            form.AddField("name", "Anon");
        }
        else
        {
            Debug.Log(playerName.Length);
            form.AddField("name", playerName);
        }
        //MUST BE 4 CHARACTERS OR LESS OTHERWISE DATABASE WILL REJECT THE SCORE
        //form.AddField("name", "yyyy");
        form.AddField("score", int.Parse(scoreText));

        Debug.Log(playerName);
        Debug.Log(int.Parse(scoreText));

        //Select destination and send data
        UnityWebRequest www = UnityWebRequest.Post("https://nathan-ellison.com/pages/add.php", form);
        yield return www.SendWebRequest();

        //Check the result, and log the error if there is one
        if(www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete");
        }
    }
}
