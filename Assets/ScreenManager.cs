using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using TMPro;

public class ScreenManager : MonoBehaviour
{
    private int score;
    public TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        string line = "";
        using(StreamReader sr = new StreamReader("Assets/score.txt"))
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
}
