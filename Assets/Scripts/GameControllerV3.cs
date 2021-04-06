using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameControllerV3 : MonoBehaviour
{

    public GameObject camera;
    public Text text;

    //Game objects that make up a burger
    public GameObject patty;
    public GameObject cooked_patty;
    public GameObject bun;
    public GameObject patty_side;
    public GameObject cheese;
    public GameObject bacon;
    public GameObject salad;
    public GameObject sauce;
    public GameObject top_bun;

    private GameObject[][] burgers;
    private GameObject[] patties;
    private bool[] flipped;

    private int cookedNum;

    private Vector3[] beefPositions;
    private Vector3[] bunPositions;

    private string currentView;
    private Vector3 grillsView;
    private Vector3 stackingView;

    // Start is called before the first frame update
    void Start()
    {
        burgers = new GameObject[7][];
        for (int i = 0; i < burgers.Length; i++)
        {
            burgers[i] = new GameObject[7];
        }

        flipped = new bool[6];
        for(int i = 0; i < flipped.Length; i++)
        {
            flipped[i] = false;
        }

        patties = new GameObject[6];

        currentView = "grills";

        //Positions where beef can be placed on the grill
        beefPositions = new Vector3[] {
            new Vector3(1.8f, 0.8f, 0f),
            new Vector3(1.8f, -1.36f, 0f),
            new Vector3(1.8f, -3.52f, 0f),
            new Vector3(-1.25f, -3.52f, 0f),
            new Vector3(-1.25f, -1.36f, 0f),
            new Vector3(-1.25f, 0.8f, 0f)
        };

        //Positions where burgers can be stacked
        bunPositions = new Vector3[]
        {
            new Vector3(16f, -4.5f, 0),
            new Vector3(19f, -4.5f, 0),
            new Vector3(22f, -4.5f, 0),
            new Vector3(25f, -4.5f, 0),
            new Vector3(28f, -4.5f, 0),
            new Vector3(31f, -4.5f, 0),
            new Vector3(34f, -4.5f, 0)
        };

        //Adjust these to cleaner values later
        grillsView = new Vector3(0f, -0.52f, -10f);
        stackingView = new Vector3(25.24f, -0.52f, -10f);
    }

    // Update is called once per frame
    void Update()
    {
        //Place patty on grill
        if (Input.GetKeyDown(KeyCode.P))
        {
            if(currentView.Equals("grills"))
            {
                for (int i = 0; i < patties.Length; i++)
                {
                    if (patties[i] == null)
                    {
                        patties[i] = Instantiate(patty, beefPositions[i], Quaternion.identity);
                        break;
                    }
                }
            }
            else if(currentView == "stacking")
            {
                for(int i = 0; i < burgers.Length; i++)
                {
                    //If there is a bun placed down (need to change)
                    if(burgers[i][0] != null && burgers[i][1] == null)
                    {

                        burgers[i][FindTop(burgers[i])] = Instantiate(patty_side, bunPositions[i], Quaternion.identity);
                        break;
                    }
                }
            }
        }
        //Flip patty on grill
        if (Input.GetKeyDown(KeyCode.F))
        {
            for(int i = 0; i < patties.Length; i++)
            {
                if(!flipped[i])
                {
                    Destroy(patties[i]);
                    patties[i] = Instantiate(cooked_patty, beefPositions[i], Quaternion.identity);
                    flipped[i] = true;
                    break;
                }
            }
        }
        //Move patty off grill
        if (Input.GetKeyDown(KeyCode.M))
        {
            for(int i = 0; i < flipped.Length; i++)
            {
                if(flipped[i])
                {
                    cookedNum++;
                    Destroy(patties[i]);
                    flipped[i] = false;
                    text.text = "Cooked Patties: " + cookedNum;
                    break;
                }
            }
        }
        //Place down bottom bun
        if (Input.GetKeyDown(KeyCode.B))
        {
            for(int i = 0; i < burgers.Length; i++)
            {
                if(burgers[i][0] == null)
                {
                    burgers[i][0] = Instantiate(bun, bunPositions[i], Quaternion.identity);
                    break;
                }
            }
        }
        //Place down cheese
        if (Input.GetKeyDown(KeyCode.C))
        {
            for(int i = 0; i < burgers.Length; i++)
            {
                Debug.Log(i);
                if(burgers[i][0] != null)
                {
                    if (FindTop(burgers[i]) == -1)
                    {
                        //return; //The burger is at max height
                        continue;
                    }
                    else
                    {
                        //NEED TO ADD CODE TO MOVE INGREDIENT POSITIONS
                        burgers[i][FindTop(burgers[i])] = Instantiate(cheese, bunPositions[i], Quaternion.identity);
                        break;
                    }
                }
                
            }
        }
        //Place down bacon
        if (Input.GetKeyDown(KeyCode.V))
        {

        }
        //Place down salad
        if (Input.GetKeyDown(KeyCode.S))
        {

        }
        //Place down sauce
        if (Input.GetKeyDown(KeyCode.E))
        {

        }
        //Serve burger
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
        }



        //Camera movements
        if (Input.GetKeyDown(KeyCode.D))
        {
            currentView = "stacking";
            MoveCamera("right");
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            currentView = "grills";
            MoveCamera("left");
        }
    }

    //Function for changing the position of the camera
    void MoveCamera(string dir)
    {
        if (dir.Equals("right"))
        {
            camera.transform.position = stackingView;
        }
        else if (dir.Equals("left"))
        {
            camera.transform.position = grillsView;
        }
    }

    //Function for finding the top of a burger array
    int FindTop(GameObject[] burger)
    {
        for(int i = 0; i < burger.Length; i++)
        {
            if(burger[i] == null)
            {
                return i;
            }
        }
        return -1;
    }
}


