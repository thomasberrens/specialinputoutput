using System;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{
    private Health _enemyHealth;
    public UnityEvent OnEnemyDeathEvent;
    [SerializeField] private Animator _animator;
    private void Start()
    {
        _enemyHealth = GetComponent<Health>();
        if (_enemyHealth == null)
        {
            gameObject.AddComponent<Health>();
            _enemyHealth = GetComponent<Health>();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag.Equals(Values.Bullet))
        {
            _enemyHealth.SubtractHealth(5);
            
            if (_enemyHealth.getHealth() <= 0)
            {
                OnEnemyDeathEvent?.Invoke();
                _animator.SetTrigger(Values.DeathAnimation);
                
            }
            
            Destroy(collision.gameObject);
        }
    }
}