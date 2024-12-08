using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;
using static UnityEngine.GraphicsBuffer;
using Unity.VisualScripting;
using System;

public class Osc : MonoBehaviour
{
    public extOSC.OSCReceiver oscReceiver;
    public extOSC.OSCTransmitter oscTransmitter;

    // Related to the ring puzzle
    public GameObject[] ring;
    private int ringSuccess = 0;
    private float currentRotation = 0;
    private bool ringWin = false;

    // Related player
    public GameObject player;
    public GameObject UVlight;

    //Related to the keys values and light puzzle lord help me...
    private float whiteKey;
    private int whitePosition;
    private float redKey;
    private int redPosition;
    private float greenKey;
    private int greenPosition;
    private float blueKey;
    private int bluePosition;
    private float[] winsOrder = new float[4];
    private int[] winsOrderNumber = {0, 1, 2, 3};
    private int winsOrderSuccess = 0;
    private bool recentWin = false;


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


        Shuffle(winsOrderNumber);

        // Print the shuffled array
        foreach (var item in winsOrderNumber)
        {
            Debug.Log(item);
        }

        for (int i = 0; i < winsOrderNumber.Length; i++)
        {
            if (i == 0)
            {
                winsOrder[i] = whiteKey;
                whitePosition = i;
            } 
            else if (i == 1)
            {
                winsOrder[i] = redKey;
                redPosition = i;
            } 
            else if (i == 2)
            {
                winsOrder[i] = greenKey;
                greenPosition = i;
            } 
            else if(i == 3)
            {
                winsOrder[i] = blueKey;
                bluePosition = i;
            }
        }

    }

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
    }

    private void Update()
    {
        currentRotation = ring[ringSuccess].transform.rotation.eulerAngles.y;
    }

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
    }

    private void colorCode()
    {

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
        ring[ringSuccess].transform.eulerAngles = new Vector3(ring[ringSuccess].transform.rotation.eulerAngles.x, value, ring[ringSuccess].transform.rotation.eulerAngles.z);
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
            colorCode();
            Debug.Log("win");
        }
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

        winsOrder[whitePosition] = whiteKey;

        if(ringWin == true)
        {
            LightPuzzle(whiteKey);
        }
        
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

        winsOrder[redPosition] = redKey;

        if (ringWin == true)
        {
            LightPuzzle(redKey);
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

        winsOrder[greenPosition] = greenKey;

        if (ringWin == true)
        {
            LightPuzzle(greenKey);
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

        winsOrder[bluePosition] = blueKey;

        if (ringWin == true)
        {
            LightPuzzle(blueKey);
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

        UVlight.GetComponent<Light>().intensity = ScaleValue(value, 0, 4095, 45, 315);

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
