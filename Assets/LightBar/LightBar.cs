using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LightBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private ArduinoInput _lightValues;
    [SerializeField] private GameObject _playerObject;
   
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;
    [SerializeField] private int LightNumber;
    [SerializeField] private Text txt;

    private void Start()
    {
        _playerObject = GameObject.FindWithTag(Values.PlayerTag);

        _playerManager = _playerObject.GetComponent<PlayerManager>();
        _lightValues = GameObject.FindWithTag(Values.ArduinoInputManager).GetComponent<ArduinoInput>();

        if (LightNumber == 1)
        {
            _playerManager.lightValue1Update.AddListener(SetBrightness);
        }

        if (LightNumber == 2)
        {
            _playerManager.lightValue2Update.AddListener(SetBrightness);
        }

        _slider.maxValue = 100f;

        fill.color = gradient.Evaluate(1f);
        txt.text = _slider.maxValue.ToString() + "%";

    }

    public void SetBrightness() {
        if (LightNumber == 1)
        {
            _slider.value = _playerManager.GetLightValue1();
            fill.color = gradient.Evaluate(_slider.normalizedValue);
        }
        else if (LightNumber == 2)
        {
            _slider.value = _playerManager.GetLightValue2();
            fill.color = gradient.Evaluate(_slider.normalizedValue);  
        }

        txt.text = _slider.value.ToString() + "%";
    }
}
