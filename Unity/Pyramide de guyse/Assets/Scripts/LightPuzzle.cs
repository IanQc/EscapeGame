using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;

public class LightPuzzle : MonoBehaviour
{
    // This is just an exemple of how extOSC works. Will change for future use when the arduino code is okie dokie
    [SerializeField] private OSCReceiver receiver;

    private void Start()
    {
        receiver.Bind("/yourAddress", OnReceiveMessage);
    }

    private void OnReceiveMessage(OSCMessage message)
    {
        Debug.Log("Received OSC message: " + message.ToString());
    }
}
