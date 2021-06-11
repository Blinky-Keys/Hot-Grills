using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        //Write the default controls to the contols file
        using(StreamWriter sw = new StreamWriter(Application.dataPath + "/controls.txt"))
        {
            sw.WriteLine("P");
            sw.WriteLine("B");
            sw.WriteLine("R");
            sw.WriteLine("F");
            sw.WriteLine("S");
            sw.WriteLine("C");
            sw.WriteLine("Space");
        }
    }

    public void PlayGame()
    {
        //Load the main game (forgot to change the scene name lol)
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
