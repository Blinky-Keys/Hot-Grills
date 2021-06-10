using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;

public class ControlsManager : MonoBehaviour
{
    public Image img;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeToWaiting()
    {
        GameObject go = EventSystem.current.currentSelectedGameObject;

        ChangeButtonText(go, "Waiting");

        StartCoroutine(WaitForKeyPress(go));
    }

    IEnumerator WaitForKeyPress(GameObject go)
    {
        while(!Input.anyKey)
        {
            yield return null;
        }

        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(vKey))
            {
                //Debug.Log(vKey.ToString());
                ChangeButtonText(go, vKey.ToString());
            }
        }
    }

    void ChangeButtonText(GameObject go, string s)
    {
        go.transform.Find("Text").GetComponent<Text>().text = s;
    }

    public void WriteControls()
    {
        using(StreamWriter sw = new StreamWriter("Assets/controls.txt"))
        {
            foreach(Transform child in img.transform)
            {
                switch(child.name)
                {
                    case "PattyButton":
                        WriteControl(child, sw);
                        break;
                    case "BunButton":
                        WriteControl(child, sw);
                        break;
                    case "BaconButton":
                        WriteControl(child, sw);
                        break;
                    case "Sauce/FlipButton":
                        WriteControl(child, sw);
                        break;
                    case "SaladButton":
                        WriteControl(child, sw);
                        break;
                    case "CheeseButton":
                        WriteControl(child, sw);
                        break;
                    case "ServeButton":
                        WriteControl(child, sw);
                        break;
                }
            }
        }
    }

    void WriteControl(Transform child, StreamWriter sw)
    {
        sw.WriteLine(child.transform.Find("Text").GetComponent<Text>().text);
    }
}
