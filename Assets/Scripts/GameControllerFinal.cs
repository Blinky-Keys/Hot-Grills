using System.Collections;

using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.IO;

public class GameControllerFinal : MonoBehaviour
{
    //Camera and text objects
    public GameObject camera;
    public Text text;

    //Spatula
    public GameObject spatula;

    public TextMeshProUGUI scoreText;

    //Order generator
    public GameObject og;

    //Player score
    int score = 0;

    //View that the player is currently seeing
    string currentView = "grills";

    //Camera positions for the two different views
    Vector3 grills = new Vector3(0.06f, -12.75f, -10f);
    Vector3 stacking = new Vector3(25.24f, -0.52f, -10f);

    //Patties on the grill
    int grillPatties = 0;
    GameObject[] pattiesArr;
    bool[] cooked;

    //Patty positions
    Vector2[] pattyPos;

    //Cooked patties
    int cookedPatties = 0;

    //Variables used for calculating score multipliers
    float timeSinceServe;
    int multiplier;

    //Burgers being assembled
    Vector2[] bunPos;
    GameObject[][] burgers;

    //Position offset from previous ingredient (for stacking burger ingredients)
    //offset was 0.15f
    float offset = 0.15f;

    //Ingredient sorting order
    int sortingOrder = 10;

    //Import assets that are going to be created when a burger is being made
    public GameObject patty;
    public GameObject cookedPatty;
    public GameObject bun;
    public GameObject patty_side;
    public GameObject salad;
    public GameObject cheese;
    public GameObject sauce;
    public GameObject topBun;
    public GameObject bacon;

    KeyCode pattyKeyCode;
    KeyCode bunKeyCode;
    KeyCode baconKeyCode;
    KeyCode sauceKeyCode;
    KeyCode saladKeyCode;
    KeyCode cheeseKeyCode;
    KeyCode serveKeyCode;

    // Start is called before the first frame update
    void Start()
    {
        multiplier = 1;
        timeSinceServe = 0f;

        //Read the controls file to bind the controls to the players liking
        using(StreamReader sr = new StreamReader(Application.dataPath + "/controls.txt"))
        {
            pattyKeyCode = ReadKeyCode(sr);
            bunKeyCode = ReadKeyCode(sr);
            baconKeyCode = ReadKeyCode(sr);
            sauceKeyCode = ReadKeyCode(sr);
            saladKeyCode = ReadKeyCode(sr);
            cheeseKeyCode = ReadKeyCode(sr);
                                                                                                                  serveKeyCode = ReadKeyCode(sr);
        }

        //Remove the file to prevent tampering
        //File.Delete("Assets/controls.txt");


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

        //Quick fix (REWRITE POSITIONS IN ARRAY LATER)
        for(int i = 0; i < pattyPos.Length; i++)
        {
            pattyPos[i].y -= 12f;
        }

        //Positions in 2D space where buns will be rendered
        bunPos = new Vector2[]
        {
            new Vector2(16.5f, -4.6f),
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
        //Check whether or not the player has too many orders still pending
        if(og.GetComponent<OrderGenerator>().GetPendingOrders() > 10)
        {
            //Play some sort of sound

            //Show gameover screen (after some time?)
            StartCoroutine(WaitForGameOver(2f));

        }

        //Player presses P key
        if (Input.GetKeyDown(pattyKeyCode))
        {
            if(currentView == "grills")
            {
                if (grillPatties >= 9)
                {
                    return;
                }
                for(int i = 0; i < pattiesArr.Length; i++)
                {
                    if(pattiesArr[i] == null)
                    {
                        pattiesArr[i] = Instantiate(patty, pattyPos[i], Quaternion.identity);
                        break;
                    }
                }
                //pattiesArr[grillPatties] = Instantiate(patty, pattyPos[grillPatties], Quaternion.identity);
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
        if(Input.GetKeyDown(sauceKeyCode))
        {
            if(currentView == "grills")
            {

                //Loop through all patties on the grill and check if they are ready to flip
                for(int i = 0; i < pattiesArr.Length; i++)
                {
                    //Flip the oldest patty that is ready to flip
                    if(pattiesArr[i] != null && pattiesArr[i].tag == "ready2flip")
                    {

                        //Spawn spatula
                        var spat = Instantiate(spatula, new Vector3(pattyPos[i].x + 1.3f, pattyPos[i].y - 15.5f, 20f), Quaternion.identity);

                        //Wait a second and then destroy spatula game object to prevent memory leak by accumulation of unused objects


                        //Remove uncooked patty game object
                        Destroy(pattiesArr[i]);

                        //Spawn cooked patty game object
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
        if(Input.GetKeyDown(bunKeyCode))
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
        if(Input.GetKeyDown(saladKeyCode))
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
        if(Input.GetKeyDown(cheeseKeyCode))
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
        if(Input.GetKeyDown(baconKeyCode))
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
        if(Input.GetKeyDown(serveKeyCode))
        {
            for(int i = 0; i < burgers.Length; i++)
            {
                //Place a top bun on the burger
                if (burgers[i][0] != null && burgers[i][5] != null && burgers[i][6] == null)
                {
                    PlaceIngredient(burgers[i], topBun);

                    if (og.GetComponent<OrderGenerator>().CheckOrder(burgers[i]))
                    {
                        ServeBurger(burgers[i]);
                        og.GetComponent<OrderGenerator>().UpdateOrders();


                        //If a burger has been served before, check how long it has been since then
                        if (timeSinceServe == 0)
                        {
                            timeSinceServe = Time.time;
                        }

                        if(timeSinceServe > 0)
                        {
                            float timeTemp = Time.time;
                            if(timeTemp - timeSinceServe < 2)
                            {
                                multiplier++;
                            }
                            else
                            {
                                multiplier = 1;
                                timeSinceServe = Time.time;
                            }
                        }


                        //Increase player score
                        score += 100 * multiplier;

                        //Play increase score sound

                    }
                    else
                    {
                        //Decrease player score
                        score -= 50;

                        //Reset score multiplier
                        multiplier = 1;

                        //Play penalty sound

                    }
                    
                    break;
                }
            }
        }

        Debug.Log("Current multiplier: " + multiplier);

        //Update the score UI
        UpdateScore();
    }

    //Calculate score multiplier
    int calculateMulti()
    {
        return 0;
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
        for(int i = 0; i < burgerArr.Length; i++)
        {
            if(burgerArr[i] == null)
            {
                //Change the offset to place ingredient at the correct height
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
        //(Add 4.6 to each objects Y component)
        for(int i = 0; i < burger.Length; i++)
        {
            Vector2 temp = burger[i].transform.position;
            temp.y += 4.6f;
            burger[i].transform.position = temp;
        }

        //After delay, destroy all game objects to simulate burger being taken (using a coroutine)
        for(int i = 0; i < burger.Length; i++)
        {
            StartCoroutine(ExecuteAfterTime(2, burger[i]));
        }

    }

    IEnumerator ExecuteAfterTime(float time, GameObject go)
    {
        yield return new WaitForSeconds(time);

        //Destroy gameobjects making up the burger
        Destroy(go);

        //Play bell sound to signify order being taken

    }

    IEnumerator WaitForGameOver(float time)
    {
        yield return new WaitForSeconds(time);

        //Write the players final score to a file
        using (StreamWriter sw = new StreamWriter(Application.dataPath + "/score.txt"))
        {
            sw.WriteLine(score);
        }

        //Load the game over screen
        SceneManager.LoadScene("GameOver");
    }

    void UpdateText()
    {
        //Change the UI text to show the number of cooked patties that are available
        text.text = "Cooked Patties: " + cookedPatties;
    }

    void UpdateScore()
    {
        //Change the UI to reflect playeres current score
        scoreText.text = "Score: " + score;
    }

    KeyCode ReadKeyCode(StreamReader sr)
    {
        return (KeyCode)System.Enum.Parse(typeof(KeyCode), sr.ReadLine());
    }


    //Adjust the vertial offset depending on which ingredient is being placed and which ingredient came before
    void UpdateOffset(GameObject ingredient, GameObject previousIngredient)
    {
        //Debug.Log("Ingredient: " + ingredient.name + " Prev Ingredient: " + previousIngredient.name);

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
                        offset = 0f;
                        break;
                    case "cheese(Clone)":
                        //Adjust offset
                        offset = -0.1f;
                        break;
                    case "bun(Clone)":
                        //Adjust offset
                        offset = 0.1f;
                        break;
                }
                break;
            case "salad":
                switch (previousIngredient.name)
                {
                    case "patty-side-cropped(Clone)":
                        //Adjust offset
                        offset = 0.1f;
                        break;
                    case "salad(Clone)":
                        //Adjust offset
                        offset = 0.2f;
                        break;
                    case "sauce(Clone)":
                        //Adjust offset
                        offset = 0.1f;                                                                                                                                                      
                        break;
                    case "bacon(Clone)":
                        //Adjust offset
                        offset = -0.1f;
                        break;
                    case "top_bun(Clone)":
                        //Adjust offset
                        offset = 0.01f;
                        break;
                    case "cheese(Clone)":
                        //Adjust offset
                        offset = -0.2f;
                        break;
                    case "bun(Clone)":
                        //Adjust offset
                        offset = 0.1f;
                        break;
                }
                break;
            case "sauce":
                switch (previousIngredient.name)
                {
                    case "patty-side-cropped(Clone)":
                        //Adjust offset
                        offset = 0.2f;
                        break;
                    case "salad(Clone)":
                        //Adjust offset
                        offset = 0.15f;
                        break;
                    case "sauce(Clone)":
                        //Adjust offset
                        offset = 0.01f;
                        break;
                    case "bacon(Clone)":
                        //Adjust offset
                        offset = -0.1f;
                        break;
                    case "top_bun(Clone)":
                        //Adjust offset
                        offset = 0;
                        break;
                    case "cheese(Clone)":
                        //Adjust offset
                        offset = 0.05f;
                        break;
                    case "bun(Clone)":
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
                        offset = 0.2f;
                        break;
                    case "sauce(Clone)":
                        //Adjust offset
                        offset = 0.2f;
                        break;
                    case "bacon(Clone)":
                        //Adjust offset
                        offset = 0.1f;
                        break;
                    case "top_bun(Clone)":
                        //Adjust offset
                        offset = 0;
                        break;
                    case "cheese(Clone)":
                        //Adjust offset
                        offset = 0.05f;
                        break;
                    case "bun(Clone)":
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
                        offset = 0.4f;
                        break;
                    case "sauce(Clone)":
                        //Adjust offset
                        offset = 0.3f;
                        break;
                    case "bacon(Clone)":
                        //Adjust offset
                        offset = 0.2f;
                        break;
                    case "top_bun(Clone)":
                        //Adjust offset
                        offset = 0;
                        break;
                    case "cheese(Clone)":
                        //Adjust offset
                        offset = 0.05f;
                        break;
                    case "bun(Clone)":
                        //Adjust offset

                        break;
                }
                break;
            case "top_bun":
                switch (previousIngredient.name)
                {
                    case "patty-side-cropped(Clone)":
                        //Adjust offset
                        offset = 0.2f;
                        break;
                    case "salad(Clone)":
                        //Adjust offset
                        offset = 0.3f;
                        break;
                    case "sauce(Clone)":
                        //Adjust offset
                        offset = 0.1f;
                        break;
                    case "bacon(Clone)":
                        //Adjust offset
                        offset = 0;
                        break;
                    case "top_bun(Clone)":
                        //Adjust offset
                        offset = 0;
                        break;
                    case "cheese(Clone)":
                        //Adjust offset
                        offset = -0.15f;
                        break;
                    case "bun(Clone)":
                        //Adjust offset
                        offset = 0.1f;
                        break;
                }
                break;
        }
    }

}
