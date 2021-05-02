using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class OrderGenerator : MonoBehaviour
{
    public string difficulty = "easy";

    public Canvas c;

    //Array containing all available ingredients
    private string[] ingredients;

    // Start is called before the first frame update
    void Start()
    {

        //Init array of ingredients
        ingredients = new string[]
        {
            "patty",
            "cheese",
            "bacon",
            "salad",
            "sauce"
        };

        //Generate order every 10 seconds
        InvokeRepeating("GenerateOrder", 0f, 10f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public string[] GenerateOrder()
    {
        //Max order size is 5 for now
        //Need to make orders generate without gaps in ingredients
        string[] order = new string[5];

        for(int i = 0; i < order.Length; i++)
        {
            order[i] = ingredients[Random.Range(1, ingredients.Length)];
        }

        string temp = "";
        
        for(int i = 0; i < order.Length; i++)
        {
            temp += " " + order[i];
        }

        Debug.Log(temp);

        //VERY TEMPORARY
        GameObject newGo = new GameObject("newOrder");
        newGo.transform.SetParent(c.transform);

        Text newText = newGo.AddComponent<Text>();
        newText.text = temp;

        return order;
    }

    
}
