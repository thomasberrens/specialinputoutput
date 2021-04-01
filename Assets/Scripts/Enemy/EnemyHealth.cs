using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private float health = 100;
    
    private Animator animator;

    private Rigidbody2D rb;
    
    public UnityEvent OnEnemyDeathEvent;

    // Update is called once per frame
    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag.Equals("Bullet"))
        {
            health -= 5;
            
            if (health <= 0)
            {
                OnEnemyDeathEvent?.Invoke();
                animator.SetTrigger(Values.DeathAnimation);
                
            }
        }
    }
}
