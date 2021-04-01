using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private ArduinoInput ai;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Light1: " + ai.arduinoLightValues.L1);
        Debug.Log("Light2: " + ai.arduinoLightValues.L2);
    }
}
