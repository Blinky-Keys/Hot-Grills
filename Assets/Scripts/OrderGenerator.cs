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
        string order = "";

        for(int i = 0; i < ingredients.Length; i++)
        {
            order += ingredients[Random.Range(1, ingredients.Length)] + " ";
        }

        orderContents[numItemsInArray(orderContents)] = order;

        //Debug.Log(order);

        orderTickets[numItemsInArray(orderContents)] = Instantiate(ticket, new Vector3((-pendingOrders*2.5f)+10f, 3f, 0), Quaternion.identity);
        orderTickets[numItemsInArray(orderContents)].GetComponent<OrderController>().UpdateText(order);


        Debug.Log(orderContents[0]);

    }

    public void UpdateOrders()
    {
        Destroy(orderTickets[1]);

        for(int i = 0; i < orderTickets.Length; i++)
        {
            if (orderTickets[i] == null) break;

            Vector2 pos = orderTickets[i].transform.position;
            pos.x += 0.5f;
            orderTickets[i].transform.position = pos;
        }

        for(int i = 1; i < orderContents.Length; i++)
        {
            orderContents[i - 1] = orderContents[i];
            orderTickets[i - 1] = orderTickets[i];
        }
    }

    public bool CheckOrder(GameObject[] burgerArr)
    {
        string[] orderCheck = orderContents[0].Split(' ');

        for(int i = 1; i < burgerArr.Length-2; i++)
        {
            if(!burgerArr[i].name.ToLower().Contains(orderCheck[i-1]))
            {
                Debug.Log("burger UNCLEAN");
                return false;
            }
        }

        Debug.Log("burger CLEAN");
        return true;
    }

    //Counts the number of items in array. Type of object in array doesn't matter as function is generic
    int numItemsInArray<T>(T[] arr)
    {
        int count = 0;
        for(int i = 0; i < arr.Length; i++)
        {
            if(arr[i] != null)
            {
                count++;
            }
        }
        return count;
    }
    
}
