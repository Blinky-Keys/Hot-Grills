using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class patty : MonoBehaviour
{
    private float initTime;
    private float timeSinceInit;

    // Start is called before the first frame update
    void Start()
    {
        initTime = Time.timeSinceLevelLoad;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceInit = Time.timeSinceLevelLoad - initTime;
        //Debug.Log(timeSinceInit);

        //Once 10 seconds has passed, change the object tag to show patty is ready to flip
        //VALUE ADJUSTED TO 1 FOR DEBUGGING PURPOSES
        if(timeSinceInit > 1)
        {
            this.tag = "ready2flip";
        }
    }
}
