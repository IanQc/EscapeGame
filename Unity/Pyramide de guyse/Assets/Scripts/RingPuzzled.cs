using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingPuzzled : MonoBehaviour
{
    public GameObject[] ring;
    private int ringSuccess = 0;
    private float currentRotation = 0;

    // Update is called once per frame
    void Update()
    {
        currentRotation = ring[ringSuccess].transform.rotation.eulerAngles.y;
        if (currentRotation <= -135 && currentRotation >= -155 && ringSuccess == 0)
        {
            ringSuccess++;
        }
        else if (currentRotation <= -100 && currentRotation >= 0 && ringSuccess == 1)
        {
            ringSuccess++;

        }


    }
}
