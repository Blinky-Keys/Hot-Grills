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

    // Start is called before the first frame update
    void Start()
    {
        string line = "";
        using(StreamReader sr = new StreamReader(Application.dataPath + "/score.txt"))
        {
            line = sr.ReadLine();
        }

        scoreText.text = "Final score: " + line;

        //Upload score to server
        StartCoroutine(UploadScore(line));

        File.Delete(Application.dataPath + "/score.txt");

    }

    public void BackToMain()
    {
        //Load the main menu
        SceneManager.LoadScene(0);
    }

    IEnumerator UploadScore(string line)
    {
        //Create new form to send data in
        WWWForm form = new WWWForm();

        //Add the data
        form.AddField("name", "test");
        form.AddField("score", int.Parse(line));

        Debug.Log(int.Parse(line));

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
