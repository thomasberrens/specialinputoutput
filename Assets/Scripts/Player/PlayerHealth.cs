using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100;

    public Animator animator;
    
    [SerializeField] private float timeBetweenAnimating = 0.3333f;
    [SerializeField] private float timestamp;
    
    public UnityEvent OnDeathEvent;
    public UnityEvent OnHurtEvent;

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= timestamp)
        {
            animator.SetBool("isHurt", false);
        }

        if (health <= 0)
        {
            OnDeathEvent?.Invoke();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Bullet"))
        {
            OnHurtEvent?.Invoke();
            animator.SetBool("isHurt", true);
            timestamp = Time.time + timeBetweenAnimating;
            health -= 5;
        }
    }
}
