using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class OrderGenerator : MonoBehaviour
{
    public string difficulty = "easy";

    //Array containing all available ingredients
    private string[] ingredients;

    //Object used for showing orders to the player
    public GameObject ticket;

    //Array (possibly change to queue) of pending orders
    private GameObject[] orders;
    private int pendingOrders;

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

        //Arbitrarily set array size to 50 because who's gonna have 50 orders waiting lol
        orders = new GameObject[50];
        pendingOrders = 0;

        //Generate order every 10 seconds
        InvokeRepeating("GenerateOrder", 0f, 10f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateOrder()
    {
        pendingOrders++;

        //Max order size is 5 for now
        //Need to make orders generate without gaps in ingredients
        string order = "";

        for(int i = 0; i < ingredients.Length; i++)
        {
            order += ingredients[Random.Range(1, ingredients.Length)] + " ";
        }

        //Debug.Log(order);

        orders[pendingOrders] = Instantiate(ticket, new Vector3(2.64764357f, 2.86149859f, 0), Quaternion.identity);
        orders[pendingOrders].GetComponent<OrderController>().UpdateText(order);


        //DEBUGGING CODE ONLY
        string temp = "";
        for(int i = 0; i < order.Length; i++)
        {
            temp += " " + order[i];
        }

        //Debug.Log(temp);

    }

    
}
