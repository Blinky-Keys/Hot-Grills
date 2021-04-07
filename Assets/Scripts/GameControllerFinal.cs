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

    //Cooked patties
    int cookedPatties = 0;

    //Burgers being assembled
    Vector2[] bunPos;
    GameObject[][] burgers;

    //Position offset for stacking burger ingredients
    float offset = 0.07f;

    //Ingredient sorting order
    int sortingOrder = 10;

    //Import assets that are going to be created whena burger is being made
    public GameObject patty;
    public GameObject cookedPatty;
    public GameObject bun;
    public GameObject patty_side;
    public GameObject salad;
    public GameObject cheese;
    public GameObject sauce;
    public GameObject topBun;
    public GameObject bacon;

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

        bunPos = new Vector2[]
        {
            new Vector2(16f, -4.6f),
            new Vector2(19f, -4.6f),
            new Vector2(22f, -4.6f),
            new Vector2(25f, -4.6f),
            new Vector2(28f, -4.6f),
            new Vector2(31f, -4.6f),
            new Vector2(34f, -4.6f)
        };

        pattiesArr = new GameObject[9];
        cooked = new bool[9];

        burgers = new GameObject[7][];
        for(int i = 0; i < burgers.Length; i++)
        {
            burgers[i] = new GameObject[7];
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Add functions for each of the controls
        if(Input.GetKeyDown(KeyCode.P))
        {
            if(currentView == "grills")
            {
                if (grillPatties >= 9)
                {
                    return;
                }
                pattiesArr[grillPatties] = Instantiate(patty, pattyPos[grillPatties], Quaternion.identity);
                grillPatties++;
            }
            else if(currentView == "stacking")
            {
                for (int i = 0; i < burgers.Length; i++)
                {
                    //Check that there is a bottom bun
                    if (burgers[i][0] != null && burgers[i][5] == null)
                    {
                        PlaceIngredient(burgers[i], patty_side);
                        break;
                    }
                }
            }  
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            if(currentView == "grills")
            {
                //Flip uncooked patty
                if (grillPatties > 0)
                {

                    Destroy(pattiesArr[grillPatties - 1]);
                    pattiesArr[grillPatties - 1] = Instantiate(cookedPatty, pattyPos[grillPatties - 1], Quaternion.identity);
                    cooked[grillPatties - 1] = true;
                }
            }
            else if(currentView == "stacking")
            {
                for(int i = 0; i < burgers.Length; i++)
                {
                    if(burgers[i][0] != null && burgers[i][5] == null)
                    {
                        PlaceIngredient(burgers[i], sauce);
                        break;
                    }
                }
            }
            
        }

        if(Input.GetKeyDown(KeyCode.M))
        {
            if(grillPatties > 0 && cooked[grillPatties-1])
            {
                //Move patty off the grill
                Destroy(pattiesArr[grillPatties - 1]);
                cooked[grillPatties - 1] = false;
                grillPatties--;
                cookedPatties++;
            }
        }

        if(Input.GetKeyDown(KeyCode.B))
        {
            //Place down bottom bun
            for(int i = 0; i < burgers.Length; i++)
            {
                if(burgers[i][0] == null)
                {
                    burgers[i][0] = Instantiate(bun, bunPos[i], Quaternion.identity);
                    break;
                }
            }
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

        if(Input.GetKeyDown(KeyCode.S))
        {
            if(currentView == "stacking")
            {
                //Place down a salad on burger
                for (int i = 0; i < burgers.Length; i++)
                {
                    //Check that there is a bottom bun
                    if (burgers[i][0] != null && burgers[i][5] == null)
                    {
                        PlaceIngredient(burgers[i], salad);
                        break;
                    }
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            if(currentView == "stacking")
            {
                //Place cheese down on burger
                for(int i = 0; i < burgers.Length; i++)
                {
                    if (burgers[i][0] != null && burgers[i][5] == null)
                    {
                        PlaceIngredient(burgers[i], cheese);
                        break;
                    }
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            if(currentView == "stacking")
            {
                //Place bacon down on burger
                for(int i = 0; i < burgers.Length; i++)
                {
                    if(burgers[i][0] != null && burgers[i][5] == null)
                    {
                        PlaceIngredient(burgers[i], bacon);
                        break;
                    }
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            //Clear all ingredients on the chopping board
            for(int i = 0; i < burgers.Length; i++)
            {
                for(int j = 0; j < burgers[i].Length; j++)
                {
                    Destroy(burgers[i][j]);
                    burgers[i][j] = null;
                }
            }
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

    void PlaceIngredient(GameObject[] burgerArr, GameObject ingredient)
    {
        for(int i = 0; i < burgerArr.Length; i++)
        {
            if(burgerArr[i] == null)
            {
                burgerArr[i] = Instantiate(ingredient, new Vector2(burgerArr[0].transform.position.x, burgerArr[i-1].transform.position.y + (offset*i)), Quaternion.identity);
                burgerArr[i].GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
                sortingOrder++;
                return;
            }
        }
    }
}
