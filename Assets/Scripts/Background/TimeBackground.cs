using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBackground : MonoBehaviour
{
    private ArduinoInput _input;

    [SerializeField] private string _timeOfDay;
    
    public SpriteRenderer spriteRenderer;
    public Sprite dayBackGround;
    public Sprite nightBackGround;
    
    // Start is called before the first frame update
    void Start()
    {
        _input = GameObject.FindWithTag(Values.ArduinoInputManager).GetComponent<ArduinoInput>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        _timeOfDay = _input.arduinoLightValues.timeOfDay;
        if (_timeOfDay.Equals("bright"))
        {
            if (spriteRenderer.sprite.Equals(dayBackGround)) return;
            spriteRenderer.sprite = dayBackGround;
        }
        else if (_timeOfDay.Equals("dark"))
        { 
            if (spriteRenderer.sprite.Equals(nightBackGround)) return;
          spriteRenderer.sprite = nightBackGround;  
        }
    }
}
