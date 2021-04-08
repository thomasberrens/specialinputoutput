using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
   [SerializeField] private Slider _slider;
   [SerializeField] private PlayerManager _playerManager;
   [SerializeField] private Health _playerHealth;
   [SerializeField] private GameObject _playerObject;
   
   [SerializeField] private Gradient gradient;
   [SerializeField] private Image fill;

   private void Start()
   {
      _playerObject = GameObject.FindWithTag(Values.PlayerTag);

      _playerManager = _playerObject.GetComponent<PlayerManager>();
      _playerHealth = _playerObject.GetComponent<Health>();
      
      _playerManager.OnHurtEvent.AddListener(SetHealth);
      
      _slider.maxValue = _playerHealth.getMaxHealth();
      _slider.value = _playerHealth.getMaxHealth();

      fill.color = gradient.Evaluate(1f);
      
   }

   public void SetMaxHealth()
   {
      
   }
   
   public void SetHealth()
   {
      _slider.value = _playerHealth.getHealth();
      fill.color = gradient.Evaluate(_slider.normalizedValue);
   }
}
