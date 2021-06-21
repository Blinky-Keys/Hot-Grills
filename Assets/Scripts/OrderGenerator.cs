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

        //Arbitrarily set array size to 50 because who's gonna have 50 orders waiting lol (game will end before that happens anyway)
        orderTickets = new GameObject[50];
        pendingOrders = 0;

        orderContents = new string[50];

        //Generate order every 10 seconds
        //InvokeRepeating("GenerateOrder", 0f, 10f);

        //Start generating orders with 10 seconds betwen them
        StartCoroutine(TimeDecayGenerateOrder(10f));
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Generate an order and then decrease the time it takes for another one to be generated
    IEnumerator TimeDecayGenerateOrder(float time)
    {
        GenerateOrder();

        yield return new WaitForSeconds(time);
        while(true)
        {
            GenerateOrder();
            if(time != 1)
            {
                if(time > 5)
                {
                    time -= 0.25f;
                }
                else
                {
                    time -= 0.15f;
                }
            }
            Debug.Log("Current interval: " + time);
            yield return new WaitForSeconds(time);
        }
    }

    public void GenerateOrder()
    {
        pendingOrders++;

        //Max order size is 5 for now
        string order = "";

        for(int i = 0; i < ingredients.Length; i++)
        {
            order += ingredients[Random.Range(0, ingredients.Length)] + " ";
        }

        orderContents[numItemsInArray(orderContents)] = order;

        //Spawn a new order ticket
        orderTickets[numItemsInArray(orderContents)] = Instantiate(ticket, new Vector3((-pendingOrders*2.5f)+36.5f, 3.45f, 0), Quaternion.identity);

        //Place the correct order text on the ticket
        orderTickets[numItemsInArray(orderContents)].GetComponent<OrderController>().UpdateText(order);


        //DEBUGGING OUTPUT
        Debug.Log(orderContents[0]);
        //Debug.Log(pendingOrders);

    }

    public void UpdateOrders()
    {
        Destroy(orderTickets[1]);

        for(int i = 1; i < orderTickets.Length; i++)
        {
            if (orderTickets[i] == null) break;

            Vector2 pos = orderTickets[i].transform.position;
            pos.x += 2.5f;
            orderTickets[i].transform.position = pos;
        }

        for(int i = 1; i < orderContents.Length; i++)
        {
            orderContents[i - 1] = orderContents[i];
            orderTickets[i - 1] = orderTickets[i];
        }
    }

    //When the player serves a burger, check that it has been made correctly according to the first ticket in the queue
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
        pendingOrders--;
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

    public int GetPendingOrders()
    {
        return pendingOrders;
    }
    
}
