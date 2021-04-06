using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    private EnemyFinder _enemyFinder;
    private Health _playerHealth;
    private PlayerMovement _playerMovement;
    private PlayerShoot _playerShoot;
    private Animator _animator;

    private Rigidbody2D rb;

    public UnityEvent OnHurtEvent;
    public UnityEvent OnPlayerDeathEvent;
    void Start()
    {
        _enemyFinder = GetComponent<EnemyFinder>();
        if (_enemyFinder == null)
        {
            gameObject.AddComponent<EnemyFinder>();
            _enemyFinder = GetComponent<EnemyFinder>();
        }

        _playerHealth = GetComponent<Health>();
        if (_playerHealth == null)
        {
            gameObject.AddComponent<Health>();
            _playerHealth = GetComponent<Health>();
        }
        
        _playerShoot = GetComponent<PlayerShoot>();
        if (_playerShoot == null)
        {
            gameObject.AddComponent<PlayerShoot>();
            _playerShoot = GetComponent<PlayerShoot>();
        }


        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();
        
        _playerShoot.wantsToShoot.AddListener(SetTargetForShooting);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject test = _enemyFinder.GetNearestTarget();
        if (test != null)
        {
            Debug.Log(test.gameObject.name);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag.Equals(Values.Bullet))
        {
            if (collision.gameObject.layer.Equals(9))
            {
                return;
            }
            OnHurtEvent?.Invoke();
           _playerHealth.SubtractHealth(5);
            
            if (_playerHealth.getHealth() <= 0)
            {
                OnPlayerDeathEvent?.Invoke();
                _animator.SetTrigger(Values.DeathAnimation);
            }
            else
            {
                StartCoroutine(handleHurtAnimation());
            }
            
            Destroy(collision.gameObject);
        }
    }

    private void SetTargetForShooting()
    {
        _playerShoot.SetTarget(_enemyFinder.GetNearestTarget());
    }
    
    private IEnumerator handleHurtAnimation()
    {
        Debug.Log("Handling hurt");
        _animator.SetBool(Values.HurtAnimation, true);

        yield return new WaitForSeconds(0.3f);
        
        _animator.SetBool(Values.HurtAnimation, false);
        Debug.Log("End of hurt animation");
    }
}
