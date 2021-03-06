using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject _playerObject;
    [SerializeField] private GameObject _dialogueManagerObject;
    [SerializeField] private GameObject _enemyObject;
    [SerializeField] private float timeBetweenDialogue = 30f; // 3 seconds
    private float timestamp;

    private PlayerMovement _playerMovement;
    private DialogueManager _dialogueManager;
    private PlayerShoot _playerShoot;

    private bool didGoLeft;
    private bool didGoRight;
    private bool didCrouch;
    private bool didJump;
    private bool didShoot;

    private bool startTutorial;
    // Start is called before the first frame update
    void Start()
    {
        _playerObject = GameObject.FindWithTag(Values.PlayerTag);
        _dialogueManagerObject = GameObject.FindWithTag(Values.DialogueManagerTag);
        
        _playerMovement = _playerObject.GetComponent<PlayerMovement>();
        _playerShoot = _playerObject.GetComponent<PlayerShoot>();
        
        _dialogueManager = _dialogueManagerObject.GetComponent<DialogueManager>();
        
        _playerMovement.OnMoveLeft.AddListener(activateLeft);
        _playerMovement.OnMoveRight.AddListener(activateRight);
        
        _playerMovement.OnCrouch.AddListener(activateCrouch);
        _playerMovement.OnJump.AddListener(activateJump);
        
        _playerShoot.PlayerShootEvent?.AddListener(shootForFirstTime);
        
        _dialogueManager.startPlayableTutorial.AddListener(StartTutorial);
        _dialogueManager.spawnEnemy.AddListener(spawnEnemy);

        _playerMovement.MayMove = false;
        _enemyObject.active = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (_enemyObject == null)
        {
            // switch scene
            SceneManager.LoadScene(Values.FinishedTutorialScene);
        }

    }
    
    private void activateRight()
    {
        if (Time.time <= timestamp) return;
        if (_dialogueManager.isTyping) return;
        
        timestamp = Time.time + timeBetweenDialogue;
        didGoRight = true;

        Debug.Log("First time right");
        _playerMovement.OnMoveRight.RemoveListener(activateRight);
        _dialogueManager.DisplayNextSentence();
    }
    
    private void activateLeft()
    {
        if (!didGoRight) return;
        
        if (Time.time <= timestamp) return;
        if (_dialogueManager.isTyping) return;
        
        timestamp = Time.time + timeBetweenDialogue;
        didGoLeft = true;
        
        Debug.Log("First time left");
        _playerMovement.OnMoveLeft.RemoveListener(activateLeft);
        _dialogueManager.DisplayNextSentence();

    }

    private void activateCrouch()
    {
        if (!didGoLeft) return;
        if (Time.time <= timestamp) return;
        if (_dialogueManager.isTyping) return;
        
        timestamp = Time.time + timeBetweenDialogue;
        didCrouch = true;
        Debug.Log("First time crouch");
        _playerMovement.OnCrouch.RemoveListener(activateCrouch);
        _dialogueManager.DisplayNextSentence();
        
    }

    private void activateJump()
    {
        if (!didCrouch) return;
        if (Time.time <= timestamp) return;
        if (_dialogueManager.isTyping) return;
        
        timestamp = Time.time + timeBetweenDialogue;
        didJump = true;
        Debug.Log("First time jump");
        _playerMovement.OnJump.RemoveListener(activateJump);
        
        _dialogueManager.DisplayNextSentence();
    }

    private void spawnEnemy()
    {
        if (Time.time <= timestamp) return;
        _enemyObject.active = true;
    }
    
    private void shootForFirstTime()
    {
        if (Time.time <= timestamp) return;
        if (_dialogueManager.isTyping) return;
        
        timestamp = Time.time + timeBetweenDialogue;
        didShoot = true;
        Debug.Log("First time shoot");
        _dialogueManager.DisplayNextSentence();
        _playerShoot.PlayerShootEvent.RemoveListener(shootForFirstTime);
    }

    private void StartTutorial()
    {
        startTutorial = true;
        _playerMovement.MayMove = true;
        timestamp = Time.time + timeBetweenDialogue;
    }
    
    
}
