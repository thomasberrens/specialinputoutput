using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float health = 100;

    public float getHealth()
    {
        return this.health;
    }

    public void setHealth(float newHealth)
    {
        this.health = newHealth;
    }

    public void SubtractHealth(float subtractValue)
    {
        this.health -= subtractValue;
    }
}



