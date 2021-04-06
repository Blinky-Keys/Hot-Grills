using System.Collections;

using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameControllerFinal : MonoBehaviour
{
    //Camera
    public GameObject camera;
    public Text text;

    //Camera positions
    string currentView = "grills";
    Vector3 grills = new Vector3(0f, -0.52f, -10f);
    Vector3 stacking = new Vector3(25.24f, -0.52f, -10f);

    //Import assets that are going to be created whena burger is being made
    public GameObject patty;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Add functions for each of the controls
        if(Input.GetKeyDown(KeyCode.P))
        {
            //Spawn in a new uncooked patty
            Instantiate(patty, new Vector3(2, 0), Quaternion.identity);
            Instantiate(patty, new Vector3(2, -2), Quaternion.identity);
            Instantiate(patty, new Vector3(2, -4), Quaternion.identity);
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            //Flip uncooked patty
        }
        if(Input.GetKeyDown(KeyCode.M))
        {
            //Move patty off the patty
        }
        if(Input.GetKeyDown(KeyCode.B))
        {
            //Place down bottom bun
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            //Place down cooked patty on the grill
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            //Move the camera back to the grills view
            changeCamera(currentView);
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            //Move the camera to the stacking view
            changeCamera(currentView);
        }
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            //Clear all ingredients on the chopping board
        }
    }

    void changeCamera(string view)
    {
        switch(view)
        {
            case "grills":
                camera.transform.position = stacking;
                currentView = "stacking";
                break;
            case "stacking":
                camera.transform.position = grills;
                currentView = "grills";
                break;
        }
    }
}
