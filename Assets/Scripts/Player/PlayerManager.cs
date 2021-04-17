using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    private EnemyFinder _enemyFinder;
    private Health _playerHealth;
    private PlayerMovement _playerMovement;
    private PlayerShoot _playerShoot;
    private Animator _animator;
    private ArduinoInput _arduinoInput;

    private Rigidbody2D rb;

    private int lightValue1;
    private int lightValue2;

    public UnityEvent OnHurtEvent;
    public UnityEvent OnPlayerDeathEvent;
    public UnityEvent lightValue1Update;
    public UnityEvent lightValue2Update;
    
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
        _arduinoInput = GameObject.FindWithTag(Values.ArduinoInputManager).GetComponent<ArduinoInput>();
        
        _playerShoot.wantsToShoot.AddListener(SetTargetForShooting);
        OnPlayerDeathEvent.AddListener(OnDeath);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject test = _enemyFinder.GetNearestTarget();
        if (test != null)
        {
            Debug.Log(test.gameObject.name);
        }

        setLightValue1();
        
        
        setLightValue2();
    }

    // TODO try to refactor this code when i have enough time
    private void setLightValue1()
    {
        lightValue1Update?.Invoke();
        lightValue1 = _arduinoInput.arduinoLightValues.L1;
    }
    
    private void setLightValue2()
    {
        lightValue2Update?.Invoke();
        lightValue2 = _arduinoInput.arduinoLightValues.L2;
    }


    public int GetLightValue1()
    {
        return lightValue1;
    }
    public int GetLightValue2()
    {
        return lightValue2;
    }

    public void OnDeath()
    {
        SceneManager.LoadScene(Values.DeathScene);
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
        Debug.Log("Setting target");
        Debug.Log("Target = " + _enemyFinder.GetNearestTarget());
        _playerShoot.SetTarget(_enemyFinder.GetNearestTarget());
    }
    
    private IEnumerator handleHurtAnimation()
    {
        
        _animator.SetBool(Values.HurtAnimation, true);

        yield return new WaitForSeconds(0.3f);
        
        _animator.SetBool(Values.HurtAnimation, false);
    }
}
