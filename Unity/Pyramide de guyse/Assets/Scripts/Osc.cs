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
    public GameObject Joueur;

    private void Start()
    {
        // Mettre cette ligne dans la m�thode start()
        oscReceiver.Bind("/Key", TraiterMessageOSC);
        oscReceiver.Bind("/Light", TraiterMessageOSC);
    }

    public static float ScaleValue(float value, float inputMin, float inputMax, float outputMin, float outputMax)
    {
        return Mathf.Clamp(((value - inputMin) / (inputMax - inputMin) * (outputMax - outputMin) + outputMin), outputMin, outputMax);
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

        /*// Changer l'�chelle de la valeur pour l'appliquer � la rotation :
        float rotation = ScaleValue(value, 0, 4095, 45, 315);
        // Appliquer la rotation au GameObject cibl� :
        Joueur.transform.eulerAngles = new Vector3(0, 0, rotation);*/
    }
}
