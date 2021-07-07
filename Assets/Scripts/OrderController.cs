using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class OrderController : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateText(string text)
    {
        //Add new line characters to make sure the order tickets don't render weird
        string[] arr = text.Split(' ');

        text = "";

        for(int i = 0; i < arr.Length; i++)
        {
            arr[i] += "\n";
            text += arr[i];
        }


        //DEBUGGING
        //Debug.Log(gameObject.transform.Find("Canvas").transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>());
        TextMeshProUGUI t = gameObject.transform.Find("Canvas").transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        t.text = text;
    }
}
