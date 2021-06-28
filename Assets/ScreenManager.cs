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
        //Read the player's final score from the score text file
        line = "";
        using(StreamReader sr = new StreamReader(Application.dataPath + "/score.txt"))
        {
            line = sr.ReadLine();
        }

        scoreText.text = "Final score: " + line;

        File.Delete(Application.dataPath + "/score.txt");

    }

    public void BackToMain()
    {
        //Load the main menu
        SceneManager.LoadScene(0);
    }

    //Called by pressing the main menu button
    public void UploadScoreAndName()
    {
        //Get the name of the player from the text input box
        string playerName = "";
        GameObject nameBox = GameObject.Find("NameBox");
        playerName = nameBox.transform.Find("Text Area").transform.Find("Text").GetComponent<TextMeshProUGUI>().text;

        //Begin upload
        StartCoroutine(UploadScore(line, playerName));
    }

    IEnumerator UploadScore(string scoreText, string playerName)
    {
        //Create new form to send data in
        WWWForm form = new WWWForm();

        //Add the data
        //If the player did not enter a name, mark their name as "Anon", otherwise add their custom name
        if (playerName.Length <= 1)
        {
            form.AddField("name", "Anon");
        }
        else
        {
            Debug.Log(playerName.Length);
            form.AddField("name", playerName);
        }
        //PLAYER NAME MUST BE 4 CHARACTERS OR LESS OTHERWISE DATABASE WILL REJECT THE SCORE
        form.AddField("score", int.Parse(scoreText));

        //Debugging output
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
