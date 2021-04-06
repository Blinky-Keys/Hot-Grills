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

    //Patties on the grill
    int grillPatties = 0;
    GameObject[] pattiesArr;
    bool[] cooked;

    //Patty positions
    Vector2[] pattyPos;

    //Cookie patties
    int cookedPatties = 0;

    //Import assets that are going to be created whena burger is being made
    public GameObject patty;
    public GameObject cookedPatty;

    // Start is called before the first frame update
    void Start()
    {
        pattyPos = new Vector2[] {
            new Vector2(2, 0),
            new Vector2(2, -2),
            new Vector2(2, -4),
            new Vector2(0, 0),
            new Vector2(0, -2),
            new Vector2(0, -4),
            new Vector2(-2, 0),
            new Vector2(-2, -2),
            new Vector2(-2, -4)
        };

        pattiesArr = new GameObject[9];
        cooked = new bool[9];
    }

    // Update is called once per frame
    void Update()
    {
        //Add functions for each of the controls
        if(Input.GetKeyDown(KeyCode.P))
        {
            if(grillPatties >= 9)
            {
                return;
            }
            pattiesArr[grillPatties] = Instantiate(patty, pattyPos[grillPatties], Quaternion.identity);
            grillPatties++;
            
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            //Flip uncooked patty
            if(grillPatties > 0)
            {


                Destroy(pattiesArr[grillPatties-1]);
                pattiesArr[grillPatties-1] = Instantiate(cookedPatty, pattyPos[grillPatties-1], Quaternion.identity);
                cooked[grillPatties - 1] = true;
                
                
            }
        }
        if(Input.GetKeyDown(KeyCode.M))
        {
            if(grillPatties > 0 && cooked[grillPatties-1])
            {
                //Move patty off the patty
                Destroy(pattiesArr[grillPatties - 1]);
                grillPatties--;
                cookedPatties++;
                cooked[grillPatties - 1] = false;
            }
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
