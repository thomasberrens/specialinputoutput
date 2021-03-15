using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private float health = 100;
    
    private Animator animator;

    private Rigidbody2D rb;

    public UnityEvent OnHurtEvent;
    public UnityEvent OnPlayerDeathEvent;

    private Collider2D _collider2D;

    // Update is called once per frame
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        _collider2D = gameObject.GetComponent<BoxCollider2D>();

    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag.Equals("Bullet"))
        {
            OnHurtEvent?.Invoke();
            health -= 5;
            
            if (health <= 0)
            {
                OnPlayerDeathEvent?.Invoke();
                animator.SetTrigger(Values.DeathAnimation);
            }
            else
            {
                StartCoroutine(handleHurtAnimation());
            }
        }
    }

    IEnumerator handleHurtAnimation()
    {
        Debug.Log("Handling hurt");
        animator.SetBool(Values.HurtAnimation, true);

        yield return new WaitForSeconds(0.3f);
        
        animator.SetBool(Values.HurtAnimation, false);
        Debug.Log("End of hurt animation");
    }
}



public static class Values
{
    public const string
        HurtAnimation = "isHurt",
        DeathAnimation = "IsDead";
}