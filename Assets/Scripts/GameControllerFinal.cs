using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public class GameControllerFinal : MonoBehaviour
{
    //Import assets that are going to be created whena burger is being made
    public GameObject patty;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Add functions for each of the controls
        if(Input.GetKeyDown(KeyCode.P))
        {
            //Spawn in a new uncooked patty
            Instantiate(patty, new Vector3(2, 0), Quaternion.identity);
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            //Flip uncooked patty
        }
        if(Input.GetKeyDown(KeyCode.M))
        {
            //Move patty off the patty
        }
        if(Input.GetKeyDown(KeyCode.B))
        {
            //Place down bottom bun
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            //Place down cooked patty
        }

    }
}
