using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameController : MonoBehaviour
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

    private GameObject[] patties;
    private GameObject[][] burgers;

    //Array of positions where patties will be placed on the grill
    private Vector3[] beefPositions;

    //Array of positions where buns will be placed on the stacking line
    private Vector3[] bunPositions;

    //Arrays for detecting placed ingredients
    private bool[] occupied;
    private bool[] flipped;
    private bool[] stacks;
    private bool[] stacked_patties;
    private bool[] stacked_cheese;
    private bool[] stacked_bacon;
    private bool[] stacked_salad;
    private bool[] stacked_sauce;

    private bool[] ready;
    
    //Camera positions for 
    private Vector3 grillsView;
    private Vector3 stackingView;

    //Order generator object
    public OrderGenerator og;

    private int cookedNum;

    private string currentView;

    // Start is called before the first frame update
    void Start()
    {
        cookedNum = 0;
        currentView = "grills";

        //Adjust these to cleaner values later
        grillsView = new Vector3(0f, -0.52f, -10f);
        stackingView = new Vector3(25.24f, -0.52f, -10f);

        //Arrays for detecting occupied patty positions, whether or not they have been flipped, and the actual GameObjects themselves
        occupied = new bool[6];
        flipped = new bool[12];
        patties = new GameObject[12];

        //Array for detecting occupied bun positions
        stacks = new bool[7];

        //Arrays for detecting occupied ingredient spaces in the stacking view
        stacked_patties = new bool[7];
        stacked_cheese = new bool[7];
        stacked_bacon = new bool[7];
        stacked_salad = new bool[7];
        stacked_sauce = new bool[7];
        ready = new bool[7];

        //Array for containing all parts of the burger
        burgers = new GameObject[7][];

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

        //Initialise each array in burgers array
        for(int i = 0; i < burgers.Length; i++)
        {
            burgers[i] = new GameObject[7];
        }
            
        //Set all arrays to intially only contain false
        EmptyArray(occupied);
        EmptyArray(flipped);
        EmptyArray(stacks);
        EmptyArray(stacked_patties);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentView.Equals("grills"))
        {
            //Key bindings for placing, flipping and moving patties
            if (Input.GetKeyDown(KeyCode.P))
            {
                //Instantiate(patty, new Vector3(1.8f, 0.8f, 0f), Quaternion.identity);
                for (int i = 0; i < occupied.Length; i++)
                {
                    if (!occupied[i])
                    {
                        patties[i] = Instantiate(patty, beefPositions[i], Quaternion.identity);
                        occupied[i] = true;
                        break;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                //Instantiate(cooked_patty, new Vector3(1.8f, 0.8f, 0f), Quaternion.identity);
                for (int i = 0; i < occupied.Length; i++)
                {
                    if (occupied[i] && !flipped[i])
                    {
                        Destroy(patties[i]);
                        patties[i] = Instantiate(cooked_patty, beefPositions[i], Quaternion.identity);
                        flipped[i] = true;
                        break;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                for (int i = 0; i < occupied.Length; i++)
                {
                    //If there is a patty and it has been flipped
                    if (occupied[i] && flipped[i])
                    {
                        Destroy(patties[i]);
                        patties[i] = null;
                        occupied[i] = false;
                        flipped[i] = false;
                        cookedNum++;
                        text.text = "Cooked Patties: " + cookedNum;
                        break;
                    }
                }
            }
        }
        if (currentView.Equals("stacking"))
        {
            //Place bun down
            if (Input.GetKeyDown(KeyCode.B))
            {
                //Check for an available space on the stacker line
                for(int i = 0; i < stacks.Length; i++)
                {
                    //Place a new bun if there is a space available
                    if(!stacks[i])
                    {
                        burgers[i][0] = Instantiate(bun, bunPositions[i], Quaternion.identity);
                        stacks[i] = true;
                        ready[i] = true;
                        break;
                    }
                }
            }
            //Place patty down
            if (Input.GetKeyDown(KeyCode.P))
            {
                //If there are cooked patties available
                if(cookedNum > 0)
                {
                    //Check for an available bun
                    for (int i = 0; i < stacks.Length; i++)
                    {
                        //If there is a bun placed down and it does not have a patty on it
                        if (stacks[i] && !stacked_patties[i])
                        {
                            //Decrease the number of cooked patties available
                            cookedNum--;
                            text.text = "Cooked patties: " + cookedNum;

                            Vector3 pos = bunPositions[i];
                            pos.y += 0.2f;
                            burgers[i][1] = Instantiate(patty_side, pos, Quaternion.identity);
                            stacked_patties[i] = true;
                            //FOR TESTING PURPOSES ONLY
                            //ready[i] = true;
                            break;
                        }
                    }
                }
            }
            //Place cheese down
            if (Input.GetKeyDown(KeyCode.C))
            {
                for(int i = 0; i < stacks.Length; i++)
                {
                    //If there is a bun placed down, with a patty on it and no cheese
                    if(stacks[i] && stacked_patties[i] && !stacked_cheese[i])
                    {
                        Vector3 pos = bunPositions[i];
                        pos.y += 0.4f;
                        burgers[i][2] = Instantiate(cheese, pos, Quaternion.identity);
                        stacked_cheese[i] = true;
                        break;
                    }
                }
            }
            //Place bacon down
            if(Input.GetKeyDown(KeyCode.V))
            {
                for(int i = 0; i < stacks.Length; i++)
                {
                    if(stacks[i] && stacked_patties[i] && stacked_cheese[i] && !stacked_bacon[i])
                    {
                        Vector3 pos = bunPositions[i];
                        pos.y += 0.4f;
                        burgers[i][3] = Instantiate(bacon, pos, Quaternion.identity);
                        stacked_bacon[i] = true;
                        //FOR TESTING PURPOSES ONLY
                        //ready[i] = true;
                        break;
                    }
                }
            }
            //Place salad down
            if(Input.GetKeyDown(KeyCode.S))
            {
                for(int i = 0; i < stacks.Length; i++)
                {
                    if(stacks[i] && stacked_patties[i] && stacked_cheese[i] && stacked_bacon[i] && !stacked_salad[i])
                    {
                        Vector3 pos = bunPositions[i];
                        pos.y += 0.3f;
                        burgers[i][4] = Instantiate(salad, pos, Quaternion.identity);
                        stacked_salad[i] = true;
                        break;
                    }
                }
            }
            //Place sauce down
            if(Input.GetKeyDown(KeyCode.E))
            {
                for(int i = 0; i < stacks.Length; i++)
                {
                    if(stacks[i] && stacked_patties[i] && stacked_cheese[i] && stacked_bacon[i] && stacked_salad[i] && !stacked_sauce[i])
                    {
                        Vector3 pos = bunPositions[i];
                        pos.y += 0.5f;
                        burgers[i][5] = Instantiate(sauce, pos, Quaternion.identity);
                        stacked_sauce[i] = true;
                        break;
                    }
                }
            }
            //Serve the burger
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Check to find burger that is ready to serve
                for (int i = 0; i < ready.Length; i++)
                {
                    if (ready[i])
                    {
                        //Place the top bun on the burger
                        Vector3 pos = bunPositions[i];
                        pos.y += 0.5f;
                        burgers[i][6] = Instantiate(top_bun, pos, Quaternion.identity);

                        foreach (GameObject obj in burgers[i])
                        {
                            //Shift the position of each burger component to the order window
                            Transform t = obj.transform; //This line throws a nullreferenceexception, must look into later
                            obj.transform.position = new Vector3(t.position.x, t.position.y + 4.5f, t.position.z);
                            ready[i] = false;
                            stacks[i] = false;
                        }
                        break;
                    }
                }
            }
            //Trash all the burgers on the stacking line
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                for(int i = 0; i < burgers.Length; i++)
                {
                    for(int j = 0; j < burgers[j].Length; j++)
                    {
                        //Debug.Log("i: " + i + "\nj: " + j);
                        if(burgers[i][j] != null)
                        {
                            Destroy(burgers[i][j]);
                            RestartArrays();
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }
        

        //Camera movements
        if(Input.GetKeyDown(KeyCode.D))
        {
            currentView = "stacking";
            MoveCamera("right");
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            currentView = "grills";
            MoveCamera("left");
        }

        //TESTING ORDER GENERATOR
        //if(Input.GetKeyDown(KeyCode.G))
        //{
        //    string[] order = og.GenerateOrder();
        //    string output = "";
        //    foreach(string s in order)
        //    {
        //        output += s + " ";
        //    }
        //    Debug.Log(output);
        //}
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

    //Take an array of bools and make them all equal to false
    void EmptyArray(bool[] arr)
    {
        for(int i = 0; i < arr.Length; i++)
        {
            arr[i] = false;
        }
    }

    //Empty every array afer hitting backspace
    void RestartArrays()
    {
        EmptyArray(stacks);
        EmptyArray(stacked_patties);
        EmptyArray(stacked_cheese);
        EmptyArray(stacked_bacon);
        EmptyArray(stacked_salad);
        EmptyArray(stacked_sauce);
    }
}
