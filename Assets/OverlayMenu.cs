using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class OverlayMenu : MonoBehaviour
{
    public GameObject overlayUI;
    
    //Changeable controls
    public TextMeshProUGUI pattyControl;
    public TextMeshProUGUI bunControl;
    public TextMeshProUGUI baconControl;
    public TextMeshProUGUI sauceFlipControl;
    public TextMeshProUGUI saladControl;
    public TextMeshProUGUI cheeseControl;

    //Nonchangeable controls
    public TextMeshProUGUI moveControl;
    public TextMeshProUGUI serveControl;


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            if(!PauseMenu.GameIsPaused)
            {
                //Show overlay
                Time.timeScale = 0;
                ShowOverlay();
            }
        }
        if(Input.GetKeyUp(KeyCode.F1))
        {
            //Hide overlay
            Time.timeScale = 1;
            HideOverlay();
        }
    }

    void ShowOverlay()
    {
        overlayUI.SetActive(true);
        UpdateControlsText();
    }

    void HideOverlay()
    {
        overlayUI.SetActive(false);
    }

    void UpdateControlsText()
    {
        using(StreamReader sr = new StreamReader(Application.dataPath + "/controls.txt"))
        {
            pattyControl.text = ReadControl(sr);
            bunControl.text = ReadControl(sr);
            baconControl.text = ReadControl(sr);
            sauceFlipControl.text = ReadControl(sr);
            saladControl.text = ReadControl(sr);
            cheeseControl.text = ReadControl(sr);
            serveControl.text = "Space";
            moveControl.text = "M";
        }
    }

    string ReadControl(StreamReader sr)
    {
        return sr.ReadLine();
    }
}
