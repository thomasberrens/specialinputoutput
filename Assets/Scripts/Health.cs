using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float currenthealth;

    private void Start()
    {
        currenthealth = maxHealth;
    }

    public float getHealth()
    {
        return this.currenthealth;
    }
    
    public float getMaxHealth()
    {
        return this.maxHealth;
    }

    public void setHealth(float newHealth)
    {
        this.currenthealth = newHealth;
    }

    public void SubtractHealth(float subtractValue)
    {
        this.currenthealth -= subtractValue;
    }
}



