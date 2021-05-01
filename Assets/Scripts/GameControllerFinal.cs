using System.Collections;

using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameControllerFinal : MonoBehaviour
{
    //Camera and text objects
    public GameObject camera;
    public Text text;

    //View that the player is currently seeing
    string currentView = "grills";

    //Camera positions for the two different views
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

    //Position offset from previous ingredient (for stacking burger ingredients)
    //offset was 0.15f
    float offset = 0.15f;

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
        //Positions in 2D space where patties will be created
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

        //Positions in 2D space where buns will be rendered
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

        //Initialising arrays to be entirely empty
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

        //Player presses P key
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
                    //Check that there is a bottom bun, no top bun, and there are cooked patties available
                    if (burgers[i][0] != null && burgers[i][5] == null && cookedPatties > 0)
                    {
                        PlaceIngredient(burgers[i], patty_side);
                        cookedPatties--;
                        UpdateText();
                        break;
                    }
                }
            }  
        }

        //Player presses F key
        if(Input.GetKeyDown(KeyCode.F))
        {
            if(currentView == "grills")
            {
                //CHANGE BELOW
                //Check if there are any patties on the grill
                //Loop through and check which ones are tagged as ready to flip
                //Flip first patty that is ready to be flipped
                //Change patty tag from ready2flip to flipped (to avoid repeated flipping)


                //Check if there are any patties on the grill and whether the latest one is ready to be flipped
                //if (grillPatties > 0 && pattiesArr[grillPatties-1].tag == "ready2flip")
                //{
                    //Destroy(pattiesArr[grillPatties - 1]);
                    //pattiesArr[grillPatties - 1] = Instantiate(cookedPatty, pattyPos[grillPatties - 1], Quaternion.identity);
                    //cooked[grillPatties - 1] = true;
                //}

                //Loop through all patties on the grill and check if they are ready to flip
                for(int i = 0; i < grillPatties; i++)
                {
                    //Flip the oldest patty that is ready to flip
                    if(pattiesArr[i].tag == "ready2flip" && pattiesArr[i] != null)
                    {
                        Destroy(pattiesArr[i]);
                        pattiesArr[i] = Instantiate(cookedPatty, pattyPos[i], Quaternion.identity);
                        cooked[i] = true;
                        break;
                    }
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

        //Player presses M key
        if(Input.GetKeyDown(KeyCode.M))
        {
            //if(grillPatties > 0 && cooked[grillPatties-1])
            //{
                //Move patty off the grill
                //Destroy(pattiesArr[grillPatties - 1]);
                //cooked[grillPatties - 1] = false;
                //grillPatties--;
                //cookedPatties++;
            //}

            for(int i = 0; i < cooked.Length; i++)
            {
                if(cooked[i])
                {
                    Destroy(pattiesArr[i]);
                    cooked[i] = false;
                    grillPatties--;
                    cookedPatties++;
                    UpdateText();
                    break;
                }
            }
        }

        //Player presses B key
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

        //Player presses A key
        if(Input.GetKeyDown(KeyCode.A))
        {
            //Move the camera back to the grills view
            changeCamera(currentView);
        }

        //Player presses D key
        if(Input.GetKeyDown(KeyCode.D))
        {
            //Move the camera to the stacking view
            changeCamera(currentView);
        }

        //Player presses S key
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

        //Player presses C key
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

        //Player presses R key
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

        //Player presses backspace key
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

        //Player presses spacebar
        if(Input.GetKeyDown(KeyCode.Space))
        {
            for(int i = 0; i < burgers.Length; i++)
            {
                //Place a top bun on the burger
                if (burgers[i][0] != null && burgers[i][5] != null && burgers[i][6] == null)
                {
                    PlaceIngredient(burgers[i], topBun);
                    ServeBurger(burgers[i]);
                    break;
                }
            }
        }
    }

    //Function for changing the position of the camera
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

    //General purpose function for placing ingredient on burger in the stacking view
    void PlaceIngredient(GameObject[] burgerArr, GameObject ingredient)
    {
        //Change the offset to place ingredient at the correct height

        for(int i = 0; i < burgerArr.Length; i++)
        {
            if(burgerArr[i] == null)
            {
                UpdateOffset(ingredient, burgerArr[i - 1]);
                burgerArr[i] = Instantiate(ingredient, new Vector2(burgerArr[0].transform.position.x, burgerArr[i-1].transform.position.y + offset), Quaternion.identity);
                burgerArr[i].GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
                sortingOrder++;
                //Debug.Log("X: " + burgerArr[0].transform.position.x + " Y: " + burgerArr[i-1].transform.position.y + (offset*i));
                return;
            }
        }
    }

    void ServeBurger(GameObject[] burger)
    {
        //Move all of the game objects upwards by a set amount to place the burger on the order window
        //(Add 4.6 to each objects Y component
        for(int i = 0; i < burger.Length; i++)
        {
            Vector2 temp = burger[i].transform.position;
            temp.y += 4.6f;
            burger[i].transform.position = temp;
        }

        //After delay, destroy all game objects to simulate burger being taken (possibly use a coroutine?)
        
    }

    void UpdateText()
    {
        //Change the UI text to show the number of cooked patties that are available
        text.text = "Cooked Patties: " + cookedPatties;
    }


    //Adjust the vertial offset depending on which ingredient is being placed and which ingredient came before
    void UpdateOffset(GameObject ingredient, GameObject previousIngredient)
    {
        Debug.Log("Ingredient: " + ingredient.name + " Prev Ingredient: " + previousIngredient.name);

        switch(ingredient.name)
        {
            case "patty-side-cropped":
                switch (previousIngredient.name)
                {
                    case "patty-side-cropped(Clone)":
                        //Adjust offset
                        offset = 0.15f;
                        break;
                    case "salad(Clone)":
                        //Adjust offset
                        offset = 0.2f;
                        break;
                    case "sauce(Clone)":
                        //Adjust offset
                        offset = 0.15f;
                        break;
                    case "bacon(Clone)":
                        //Adjust offset
                        offset = 0.015f;
                        break;
                    case "top_bun(Clone)":
                        //Adjust offset
                        offset = 0.15f;
                        break;
                    case "cheese(Clone)":
                        //Adjust offset
                        offset = -0.1f;
                        break;
                }
                break;
            case "salad":
                switch (previousIngredient.name)
                {
                    case "patty-side-cropped(Clone)":
                        //Adjust offset

                        break;
                    case "salad(Clone)":
                        //Adjust offset

                        break;
                    case "sauce(Clone)":
                        //Adjust offset

                        break;
                    case "bacon(Clone)":
                        //Adjust offset

                        break;
                    case "top_bun(Clone)":
                        //Adjust offset

                        break;
                    case "cheese(Clone)":
                        //Adjust offset

                        break;
                }
                break;
            case "sauce":
                switch (previousIngredient.name)
                {
                    case "patty-side-cropped(Clone)":
                        //Adjust offset

                        break;
                    case "salad(Clone)":
                        //Adjust offset

                        break;
                    case "sauce(Clone)":
                        //Adjust offset

                        break;
                    case "bacon(Clone)":
                        //Adjust offset

                        break;
                    case "top_bun(Clone)":
                        //Adjust offset

                        break;
                    case "cheese(Clone)":
                        //Adjust offset

                        break;
                }
                break;
            case "bacon":
                switch (previousIngredient.name)
                {
                    case "patty-side-cropped(Clone)":
                        //Adjust offset
                        offset = 0.3f;
                        break;
                    case "salad(Clone)":
                        //Adjust offset

                        break;
                    case "sauce(Clone)":
                        //Adjust offset

                        break;
                    case "bacon(Clone)":
                        //Adjust offset

                        break;
                    case "top_bun(Clone)":
                        //Adjust offset

                        break;
                    case "cheese(Clone)":
                        //Adjust offset

                        break;
                }
                break;
            case "cheese":
                switch (previousIngredient.name)
                {
                    case "patty-side-cropped(Clone)":
                        //Adjust offset
                        offset = 0.3f;
                        break;
                    case "salad(Clone)":
                        //Adjust offset

                        break;
                    case "sauce(Clone)":
                        //Adjust offset

                        break;
                    case "bacon(Clone)":
                        //Adjust offset

                        break;
                    case "top_bun(Clone)":
                        //Adjust offset

                        break;
                    case "cheese(Clone)":
                        //Adjust offset
                        offset = 0.05f;
                        break;
                }
                break;
            case "top_bun":
                switch (previousIngredient.name)
                {
                    case "patty-side-cropped(Clone)":
                        //Adjust offset
                        offset = 0.15f;
                        break;
                    case "salad(Clone)":
                        //Adjust offset

                        break;
                    case "sauce(Clone)":
                        //Adjust offset

                        break;
                    case "bacon(Clone)":
                        //Adjust offset

                        break;
                    case "top_bun(Clone)":
                        //Adjust offset

                        break;
                    case "cheese(Clone)":
                        //Adjust offset
                        offset = -0.1f;
                        break;
                }
                break;
        }
    }

}
