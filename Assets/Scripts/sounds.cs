using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sounds : MonoBehaviour
{
    public GameObject clickSound;

    public void PlayClick()
    {
        clickSound.GetComponent<AudioSource>().Play();
    }

}
