using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;
using static UnityEngine.GraphicsBuffer;
using Unity.VisualScripting;
using System;
using UnityEngine.SceneManagement;

public class Osc : MonoBehaviour
{
    public extOSC.OSCReceiver oscReceiver;
    public extOSC.OSCTransmitter oscTransmitter;

    // sons 
    public AudioClip wheel;
    public AudioClip ball;
    public AudioSource effetsSonores;

    // Related to the ring puzzle
    public GameObject bigRing;
    public GameObject[] ring;
    private int ringSuccess = 0;
    private float currentRotation = 0;
    private bool ringWin = false;
    private bool isLooking = false;

    // Related player
    public GameObject player;
    public GameObject UVlight;

    //Related to the keys values and light puzzle lord help me...
    private float whiteKey;
    //private int whitePosition;

    private float redKey;
    //private int redPosition;

    private float greenKey;
    //private int greenPosition;

    private float blueKey;
    //private int bluePosition;

    private float[] winsOrder = new float[4];
    private int[] winsOrderNumber = {0, 1, 2, 3};

    private float winsOrderSuccess = 0;
    private bool recentWin = false;

    public GameObject ColoredBall;

    private void Start()
    {
        // Mettre cette ligne dans la méthode start()
        oscReceiver.Bind("/KeyWhite", TraiterWhiteOSC);
        oscReceiver.Bind("/KeyRed", TraiterRedOSC);
        oscReceiver.Bind("/KeyGreen", TraiterGreenOSC);
        oscReceiver.Bind("/KeyBlue", TraiterBlueOSC);
        oscReceiver.Bind("/Light", TraiterLightOSC);
        oscReceiver.Bind("/rotation", TraiterRotationOSC);
        oscReceiver.Bind("/button", TraiterConfirmOSC);

        Debug.Log("code is : blue, red, white, green");
        effetsSonores = GetComponent<AudioSource>();
        /*
        Shuffle(winsOrderNumber);

        // Print the shuffled array
        foreach (var item in winsOrderNumber)
        {
            Debug.Log(item);
        }

        for (int i = 0; i < winsOrderNumber.Length; i++)
        {
            if (winsOrderNumber[i] == 0)
            {
                whitePosition = winsOrderNumber[i];
                winsOrder[whitePosition] = whiteKey;
                Debug.Log(whitePosition);
            } 
            else if (winsOrderNumber[i] == 1)
            {
                redPosition = winsOrderNumber[i];
                winsOrder[redPosition] = redKey;
                Debug.Log(redPosition);
            } 
            else if (winsOrderNumber[i] == 2)
            {
                greenPosition = winsOrderNumber[i];
                winsOrder[greenPosition] = greenKey;
                Debug.Log(greenPosition);
            } 
            else if(winsOrderNumber[i] == 3)
            {
                bluePosition = winsOrderNumber[i];
                winsOrder[bluePosition] = blueKey;
                Debug.Log(bluePosition);
            }
        }



        colorSequence();*/
    }

    private void Update()
    {
        currentRotation = ring[ringSuccess].transform.rotation.eulerAngles.y;
    }
    /*
    void Shuffle(int[] array)
    {
        System.Random rand = new System.Random();
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = rand.Next(i + 1); // Random index between 0 and i
            // Swap elements at i and j
            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }*/
    /*
    private void colorSequence() // NOT FINISHED
    {
        string white = winsOrderNumber[0].ToString();
        string red = winsOrderNumber[1].ToString();
        string green = winsOrderNumber[2].ToString();
        string blue = winsOrderNumber[3].ToString();

        Debug.Log(white + red + green + blue);

        for (int i = 0; i < winsOrderNumber.Length; i++)
        {
            if (winsOrderNumber[i] == 0)
            {
                //Debug.Log("white");
                // play color animation
                //ColoredBall.GetComponent<Animator>().Play("ballNothingWhite");

            }
            else if (winsOrderNumber[i] == 1)
            {
                //Debug.Log("red");
                // play color animation
            }
            else if (winsOrderNumber[i] == 2)
            {
                //Debug.Log("green");
                // play color animation
            }
            else if (winsOrderNumber[i] == 3)
            {
                //Debug.Log("blue");
                // play color animation
            }
        }
    }*/
    /*
    private void LightPuzzle(float value)
    {
        recentWin = false;

        if (winsOrder[winsOrderSuccess] == 1 && winsOrderSuccess <= 2)
        {
            Debug.Log("doing well");
            winsOrderSuccess++;
            recentWin = true;
            return;
        }
        else if (winsOrderSuccess == 3 && winsOrder[3] == 1)
        {
            Debug.Log("win");
            ringWin = false;
        }
        else if (value != 0 && recentWin != true)
        {
            Debug.Log("fail");
            winsOrderSuccess = 0;
        }
    }*/

    private void LightPuzzle(float value)
    {
        recentWin = false;

        if (value == winsOrderSuccess && winsOrderSuccess <= 2)
        {
            Debug.Log("doing well");
            winsOrderSuccess++;
            recentWin = true;
            return;
        }
        else if (winsOrderSuccess == 3 && value == winsOrderSuccess)
        {
            Debug.Log("win");
            ringWin = false;
            StartCoroutine(trophy());
        }
        else if (value != winsOrderSuccess && recentWin != true)
        {
            Debug.Log("fail");
            winsOrderSuccess = 0;
            StartCoroutine(colorSequence());
        }
    }

    private IEnumerator colorSequence()
    {
        effetsSonores.PlayOneShot(ball, 2f);
        ColoredBall.GetComponent<Animator>().Play("ballNothingBlue");
        yield return new WaitForSeconds(2);
        effetsSonores.PlayOneShot(ball, 2f);
        ColoredBall.GetComponent<Animator>().Play("ballBlueToRed");
        yield return new WaitForSeconds(2);
        effetsSonores.PlayOneShot(ball, 2f);
        ColoredBall.GetComponent<Animator>().Play("ballRedToWhite");
        yield return new WaitForSeconds(2);
        effetsSonores.PlayOneShot(ball, 2f);
        ColoredBall.GetComponent<Animator>().Play("ballWhiteToGreen");
        yield return new WaitForSeconds(2);
        effetsSonores.PlayOneShot(ball, 2f);
        ColoredBall.GetComponent<Animator>().Play("ballGreenToNothing");
        yield break;
    }

    private IEnumerator trophy()
    {

        ColoredBall.GetComponent<Animator>().Play("ballGameFinished");
        yield return new WaitForSeconds(10);
        SceneManager.LoadScene("PyramidOfGyatt");
        yield break;
    }

    public static float ScaleValue(float value, float inputMin, float inputMax, float outputMin, float outputMax)
    {
        return Mathf.Clamp(((value - inputMin) / (inputMax - inputMin) * (outputMax - outputMin) + outputMin), outputMin, outputMax);
    }

    void TraiterRotationOSC(OSCMessage oscMessage)
    {
        // Récupérer une valeur numérique en tant que float
        // même si elle est de type float ou int :
        float value;
        if (oscMessage.Values[0].Type == OSCValueType.Int)
        {
            value = oscMessage.Values[0].IntValue;
        }
        else if (oscMessage.Values[0].Type == OSCValueType.Float)
        {
            value = oscMessage.Values[0].FloatValue;
        }
        else
        {
            // Si la valeur n'est ni un float ou int, on quitte la méthode :
            return;
        }

        //Debug.Log(value);

        // Appliquer la rotation au GameObject ciblé :
        if (ringWin == false)
        {
            ring[ringSuccess].transform.eulerAngles = new Vector3(ring[ringSuccess].transform.rotation.eulerAngles.x, value, ring[ringSuccess].transform.rotation.eulerAngles.z);
        }
        
    }

    void TraiterConfirmOSC(OSCMessage oscMessage)
    {
        // Récupérer une valeur numérique en tant que float
        // même si elle est de type float ou int :
        float value;
        if (oscMessage.Values[0].Type == OSCValueType.Int)
        {
            value = oscMessage.Values[0].IntValue;
        }
        else if (oscMessage.Values[0].Type == OSCValueType.Float)
        {
            value = oscMessage.Values[0].FloatValue;
        }
        else
        {
            // Si la valeur n'est ni un float ou int, on quitte la méthode :
            return;
        }

        //Debug.Log(value);

        if (ringWin == false)
        {
            if (currentRotation >= 195 && currentRotation <= 215 && ringSuccess == 0 && value == 0)
            {
                ringSuccess++;
            }
            else if (currentRotation >= 110 && currentRotation <= 140 && ringSuccess == 1 && value == 0)
            {
                ringSuccess++;

            }
            else if (currentRotation >= 191 && currentRotation <= 211 && ringSuccess == 2 && value == 0)
            {
                ringWin = true;
                //colorSequence();
                Debug.Log("win");
                StartCoroutine(won());
            } 
            else if (value == 0)
            {
                ringSuccess = 0;
                StartCoroutine(wrong());
            }
        }


    }
    
    private IEnumerator won()
    {
        effetsSonores.PlayOneShot(wheel, 1f);
        bigRing.GetComponent<Animator>().enabled = true;
        bigRing.GetComponent<Animator>().Play("wheelSuccess");
        yield return new WaitForSeconds(2);
        ColoredBall.GetComponent<Animator>().Play("ballRise");
        yield return new WaitForSeconds(2);
        bigRing.GetComponent<Animator>().enabled = false;
        ColoredBall.GetComponent<Animator>().Play("ballIdle");
        StartCoroutine (colorSequence());
        yield break;
    }

    private IEnumerator wrong()
    {
        bigRing.GetComponent<Animator>().enabled = true;
        bigRing.GetComponent<Animator>().Play("wheelFail");
        yield return new WaitForSeconds(1);
        bigRing.GetComponent<Animator>().Play("idleWheel");
        yield return new WaitForSeconds(1);
        bigRing.GetComponent<Animator>().enabled = false;
        yield break;
    }

    void TraiterWhiteOSC(OSCMessage oscMessage)
    {
        // Récupérer une valeur numérique en tant que float
        // même si elle est de type float ou int :
        float value;
        if (oscMessage.Values[0].Type == OSCValueType.Int)
        {
            value = oscMessage.Values[0].IntValue;
        }
        else if (oscMessage.Values[0].Type == OSCValueType.Float)
        {
            value = oscMessage.Values[0].FloatValue;
        }
        else
        {
            // Si la valeur n'est ni un float ou int, on quitte la méthode :
            return;
        }

        //Debug.Log(value);

        whiteKey = value;


        if (ringWin == false && value == 0 && isLooking == false)
        {
            ringSuccess = 0;
            StartCoroutine(looking());
        }

        //winsOrder[whitePosition] = whiteKey;

        if(ringWin == true && value == 0)
        {
            LightPuzzle(2);
        }
        
    }

    private IEnumerator looking()
    {
        isLooking = true;
        player.GetComponent<Animator>().Play("shineLight");
        yield return new WaitForSeconds(10);
        player.GetComponent<Animator>().Play("shineOff");
        isLooking = false;
        yield break;
    }

    void TraiterRedOSC(OSCMessage oscMessage)
    {
        // Récupérer une valeur numérique en tant que float
        // même si elle est de type float ou int :
        float value;
        if (oscMessage.Values[0].Type == OSCValueType.Int)
        {
            value = oscMessage.Values[0].IntValue;
        }
        else if (oscMessage.Values[0].Type == OSCValueType.Float)
        {
            value = oscMessage.Values[0].FloatValue;
        }
        else
        {
            // Si la valeur n'est ni un float ou int, on quitte la méthode :
            return;
        }

        //Debug.Log(value);

        redKey = value;

        //winsOrder[redPosition] = redKey;

        if (ringWin == true && value == 0)
        {
            LightPuzzle(1);
        }
    }

    void TraiterGreenOSC(OSCMessage oscMessage)
    {
        // Récupérer une valeur numérique en tant que float
        // même si elle est de type float ou int :
        float value;
        if (oscMessage.Values[0].Type == OSCValueType.Int)
        {
            value = oscMessage.Values[0].IntValue;
        }
        else if (oscMessage.Values[0].Type == OSCValueType.Float)
        {
            value = oscMessage.Values[0].FloatValue;
        }
        else
        {
            // Si la valeur n'est ni un float ou int, on quitte la méthode :
            return;
        }

        //Debug.Log(value);

        greenKey = value;

        //winsOrder[greenPosition] = greenKey;

        if (ringWin == true && value == 0)
        {
            LightPuzzle(3);
        }


    }

    void TraiterBlueOSC(OSCMessage oscMessage)
    {
        // Récupérer une valeur numérique en tant que float
        // même si elle est de type float ou int :
        float value;
        if (oscMessage.Values[0].Type == OSCValueType.Int)
        {
            value = oscMessage.Values[0].IntValue;
        }
        else if (oscMessage.Values[0].Type == OSCValueType.Float)
        {
            value = oscMessage.Values[0].FloatValue;
        }
        else
        {
            // Si la valeur n'est ni un float ou int, on quitte la méthode :
            return;
        }

        //Debug.Log(value);

        blueKey = value;

        //winsOrder[bluePosition] = blueKey;

        if (ringWin == true && value == 0)
        {
            LightPuzzle(0);
        }


    }

    void TraiterLightOSC(OSCMessage oscMessage)
    {
        // Récupérer une valeur numérique en tant que float
        // même si elle est de type float ou int :
        float value;
        if (oscMessage.Values[0].Type == OSCValueType.Int)
        {
            value = oscMessage.Values[0].IntValue;
        }
        else if (oscMessage.Values[0].Type == OSCValueType.Float)
        {
            value = oscMessage.Values[0].FloatValue;
        }
        else
        {
            // Si la valeur n'est ni un float ou int, on quitte la méthode :
            return;
        }

        //Debug.Log(value);

        UVlight.GetComponent<Light>().intensity = ScaleValue(value, 4095, 0, 0, 3);

    }

    void TraiterMessageOSC(OSCMessage oscMessage)
    {
        // Récupérer une valeur numérique en tant que float
        // même si elle est de type float ou int :
        float value;
        if (oscMessage.Values[0].Type == OSCValueType.Int)
        {
            value = oscMessage.Values[0].IntValue;
        }
        else if (oscMessage.Values[0].Type == OSCValueType.Float)
        {
            value = oscMessage.Values[0].FloatValue;
        }
        else
        {
            // Si la valeur n'est ni un float ou int, on quitte la méthode :
            return;
        }

        //Debug.Log(value);

        // Changer l'échelle de la valeur pour l'appliquer à la rotation :
        float rotation = ScaleValue(value, 0, 4095, 45, 315);
        // Appliquer la rotation au GameObject ciblé :
        //Joueur.transform.eulerAngles = new Vector3(0, rotation, 0);
    }
}
