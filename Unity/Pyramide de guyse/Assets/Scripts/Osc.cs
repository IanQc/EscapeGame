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

    public GameObject[] ring;
    private int ringSuccess = 0;
    private float currentRotation = 0;

    private void Start()
    {
        // Mettre cette ligne dans la m�thode start()
        //oscReceiver.Bind("/Key", TraiterMessageOSC);
        //oscReceiver.Bind("/Light", TraiterMessageOSC);
        oscReceiver.Bind("/rotation", TraiterRotationOSC);
        oscReceiver.Bind("/button", TraiterConfirmOSC);

    }

    private void Update()
    {
        currentRotation = ring[ringSuccess].transform.rotation.eulerAngles.y;

        //Debug.Log(currentRotation);
    }

    public static float ScaleValue(float value, float inputMin, float inputMax, float outputMin, float outputMax)
    {
        return Mathf.Clamp(((value - inputMin) / (inputMax - inputMin) * (outputMax - outputMin) + outputMin), outputMin, outputMax);
    }

    void TraiterRotationOSC(OSCMessage oscMessage)
    {
        // R�cup�rer une valeur num�rique en tant que float
        // m�me si elle est de type float ou int :
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
            // Si la valeur n'est ni un foat ou int, on quitte la m�thode :
            return;
        }

        //Debug.Log(value);

        // Changer l'�chelle de la valeur pour l'appliquer � la rotation :
        // Appliquer la rotation au GameObject cibl� :
        ring[ringSuccess].transform.eulerAngles = new Vector3(ring[ringSuccess].transform.rotation.eulerAngles.x, value, ring[ringSuccess].transform.rotation.eulerAngles.z);
    }

    void TraiterConfirmOSC(OSCMessage oscMessage)
    {
        // R�cup�rer une valeur num�rique en tant que float
        // m�me si elle est de type float ou int :
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
            // Si la valeur n'est ni un foat ou int, on quitte la m�thode :
            return;
        }

        Debug.Log(value);

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

    void TraiterMessageOSC(OSCMessage oscMessage)
    {
        // R�cup�rer une valeur num�rique en tant que float
        // m�me si elle est de type float ou int :
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
            // Si la valeur n'est ni un foat ou int, on quitte la m�thode :
            return;
        }

        Debug.Log(value);

        // Changer l'�chelle de la valeur pour l'appliquer � la rotation :
        float rotation = ScaleValue(value, 0, 4095, 45, 315);
        // Appliquer la rotation au GameObject cibl� :
        //Joueur.transform.eulerAngles = new Vector3(0, rotation, 0);
    }
}
