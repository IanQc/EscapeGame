using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;

public class LightPuzzle : MonoBehaviour
{
    // This is just an exemple of how extOSC works. Will change for future use when the arduino code is okie dokie
    [SerializeField] private OSCReceiver receiver;

    private float fuckup;
    private int keyPress;

    private void Start()
    {
        receiver.Bind("/lightMap", MessageReceived);
        receiver.Bind("/keyUnit", MessageReceived);
    }

    private void Update()
    {
        Debug.Log(fuckup);
    }

    protected void MessageReceived(OSCMessage message)
    {
        Debug.Log(message);
        if (message.Address == "/keyUnit")
        {
            
        }




        if (message.ToInt(out var value))
        {
            Debug.Log(value);
            fuckup = value;
        }
    }

    public GameObject playa;

    public void LateUpdate()
    {

    }
}
