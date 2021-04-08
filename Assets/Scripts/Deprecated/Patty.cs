using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patty : MonoBehaviour
{
    public GameObject cookedPatty;
    private bool flipped;

    private void Start()
    {
        flipped = false;
    }

    void flip()
    {
        flipped = true;
        Instantiate(cookedPatty, this.transform.position, Quaternion.identity);
        Destroy(this);
    }

    public bool isFlipped()
    {
        if(flipped)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
