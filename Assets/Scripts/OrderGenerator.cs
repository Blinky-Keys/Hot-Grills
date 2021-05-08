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
    private GameObject[] orderTickets;
    private string[] orderContents;
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
        orderTickets = new GameObject[50];
        pendingOrders = 0;

        orderContents = new string[50];

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

        orderContents[pendingOrders] = order;

        //Debug.Log(order);

        orderTickets[pendingOrders] = Instantiate(ticket, new Vector3((-pendingOrders*2.5f)+10f, 3f, 0), Quaternion.identity);
        orderTickets[pendingOrders].GetComponent<OrderController>().UpdateText(order);


        //DEBUGGING CODE ONLY
        string temp = "";
        for(int i = 0; i < order.Length; i++)
        {
            temp += " " + order[i];
        }

        //Debug.Log(temp);

    }

    //Removes order from queue of pending orders
    void RemoveOrder()
    {
        Destroy(orderTickets[0]);
        orderContents[0] = null;
    }

    //Moves the pending order tickets along the screen after one has been completed
    void MoveOrders()
    {
        //Move all tickets in the game world
        for(int i = 1; i < orderTickets.Length; i++)
        {
            Vector3 pos = orderTickets[i].transform.position;
            pos.x += 5f;
            orderTickets[i].transform.position = pos;
        }

        //Move all tickets up one space in the array
        for(int i = 1; i < orderTickets.Length; i++)
        {
            orderTickets[i - 1] = orderTickets[i];
        }
    }

    public bool CheckOrder(GameObject[] burgerArr)
    {
        string[] order = orderContents[1].Split(' ');

        for(int i = 0; i < order.Length; i++)
        {
            //Debug.Log("Order arr: " + order[i] + " Burger Arr: " + burgerArr[i].name);
        }

        for(int i = 0; i < order.Length; i++)
        {
            if(!burgerArr[i+1].name.ToLower().Contains(order[i]))
            {
                Debug.Log("burger is UNCLEAN");
                return false;
            }
        }
        Debug.Log("buruger is CLEAN");
        return true;
    }

    
}
