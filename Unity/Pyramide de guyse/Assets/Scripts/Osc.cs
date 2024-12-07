using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;
using static UnityEngine.GraphicsBuffer;
using Unity.VisualScripting;

public class Osc : MonoBehaviour
{
    public extOSC.OSCReceiver oscReceiver;
    public extOSC.OSCTransmitter oscTransmitter;

    // Related to the ring puzzle
    public GameObject[] ring;
    private int ringSuccess = 0;
    private float currentRotation = 0;

    // Related player
    public GameObject player;
    public GameObject UVlight;

    //Related to the keys values and light puzzle
    private float whiteKey;
    private float redKey;
    private float greenKey;
    private float blueKey;

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

    }

    private void Update()
    {
        currentRotation = ring[ringSuccess].transform.rotation.eulerAngles.y;

        LightPuzzle();
    }

    private void LightPuzzle()
    {
        if (whiteKey == 1)
        {

        }
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
            // Si la valeur n'est ni un foat ou int, on quitte la méthode :
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
            // Si la valeur n'est ni un foat ou int, on quitte la méthode :
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
            // Si la valeur n'est ni un foat ou int, on quitte la méthode :
            return;
        }

        //Debug.Log(value);

        whiteKey = value;
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
            // Si la valeur n'est ni un foat ou int, on quitte la méthode :
            return;
        }

        Debug.Log(value);

        redKey = value;
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
            // Si la valeur n'est ni un foat ou int, on quitte la méthode :
            return;
        }

        Debug.Log(value);

        greenKey = value;
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
            // Si la valeur n'est ni un foat ou int, on quitte la méthode :
            return;
        }

        Debug.Log(value);

        blueKey = value;
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
            // Si la valeur n'est ni un foat ou int, on quitte la méthode :
            return;
        }

        Debug.Log(value);

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
            // Si la valeur n'est ni un foat ou int, on quitte la méthode :
            return;
        }

        Debug.Log(value);

        // Changer l'échelle de la valeur pour l'appliquer à la rotation :
        float rotation = ScaleValue(value, 0, 4095, 45, 315);
        // Appliquer la rotation au GameObject ciblé :
        //Joueur.transform.eulerAngles = new Vector3(0, rotation, 0);
    }
}
