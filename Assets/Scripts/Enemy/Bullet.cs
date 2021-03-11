using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.tag.Equals("Player"))
         {
             Debug.Log("Hitted player");
             gameObject.active = false;
             return;
         }
        else
        {
            gameObject.active = false;
        }
    }
}
