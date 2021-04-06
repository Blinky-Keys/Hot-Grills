using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderGenerator : MonoBehaviour
{
    public string difficulty = "easy";

    //Array containing all available ingredients
    private string[] ingredients;

    // Start is called before the first frame update
    void Start()
    {
        ingredients = new string[]
        {
            "patty",
            "cheese",
            "bacon",
            "salad",
            "sauce",
            "stop"
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string[] GenerateOrder()
    {
        //Max order size is 10 for now
        //Need to make orders generate without gaps in ingredients
        string[] order = new string[10];

        for(int i = 0; i < order.Length; i++)
        {
            int randNum = Random.Range(1, ingredients.Length);
            if(ingredients[randNum].Equals("stop"))
            {
                order[i] = ingredients[randNum];
                break;
            }
            order[i] = ingredients[randNum];
        }
        return order;
    }

    
}
