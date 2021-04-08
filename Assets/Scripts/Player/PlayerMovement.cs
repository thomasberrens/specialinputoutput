using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController2D controller;
    
    private Animator animator;

    private float horizontalmove = 0f;

    [SerializeField]
    private float runSpeed = 40f;
   [SerializeField] private float maxDistanceForJump = 1.5f;
    
    [SerializeField]
    private Transform jumpPoint;
    private Transform shootPoint;
    [SerializeField]
    private Transform crouchPoint;

    private Rigidbody2D rb;

    private bool jump;
    private bool crouch;

    public bool MayMove = true;
    
    [SerializeField] private ArduinoInput ai;
    private ArrayList leftMap = new ArrayList();
    private ArrayList rightMap = new ArrayList();
    private ArrayList crouchMap = new ArrayList();
    private ArrayList idleMap = new ArrayList();

    private int LightValue1;
    private int LightValue2;

    public UnityEvent OnMoveLeft;
    public UnityEvent OnMoveRight;
    public UnityEvent OnIdle;
    public UnityEvent OnCrouch;
    public UnityEvent OnJump;

    private void Start()
    {

        addNumbersInRangeToList(0, 49, leftMap);
        addNumbersInRangeToList(51, 120, rightMap);
        addNumbersInRangeToList(0, 29, crouchMap);
        addNumbersInRangeToList(45, 55, idleMap);

        jumpPoint = gameObject.transform.Find(Values.JumpPoint);
        shootPoint = gameObject.transform.Find(Values.ShootPoint);
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<CharacterController2D>();
        animator = GetComponent<Animator>();
        ai = GetComponent<ArduinoInput>();
    }

    void addNumbersInRangeToList(int lowestNumber, int HighestNumber, ArrayList list)
    {
        for (int i = lowestNumber; i <= HighestNumber; i++)
        {
            list.Add(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        LightValue1 = ai.arduinoLightValues.L1;
        LightValue2 = ai.arduinoLightValues.L2;

        if (!MayMove) return;
        
        if (isInIntRange(LightValue1, rightMap))
        {
            horizontalmove = 1 * runSpeed;
            OnMoveRight?.Invoke();
            
        }
        else if (isInIntRange(LightValue1, leftMap))
        {
            horizontalmove = -1 * runSpeed;
            OnMoveLeft?.Invoke();
        }


        //    horizontalmove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Math.Abs(horizontalmove));
        
        if (isInIntRange(LightValue2, crouchMap))
        {
            crouch = true;
            OnCrouch?.Invoke();
        } else if (crouch)
        {
            crouch = false;
        }


        if (needToJump() && !crouch)
        {
            if (jump) return;
            jump = true;
            animator.SetBool("isJumping", true);
            OnJump?.Invoke();
        }

    }

    private void FixedUpdate()
    {
        controller.Move(horizontalmove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

    public void onLand()
    {
        animator.SetBool("isJumping", false);
        jump = false;
    }

    public void onCrouching(bool isCrouching)
    {
        animator.SetBool("isCrouching", isCrouching);
    }

    private bool needToJump()
    {
        Vector2 hitDirection = (Vector2) jumpPoint.position - rb.position;

        GetComponent<Collider2D>().enabled = false;
        RaycastHit2D hit = Physics2D.Raycast(jumpPoint.position, hitDirection, maxDistanceForJump);
        GetComponent<Collider2D>().enabled = true;

        if (hit.collider == null)
        {
            return false;
        }
        
        if (hit.collider.gameObject.tag.Equals(Values.ObstacleTag))
        {
            return true;
        }

        return false;
    }

    private bool isInIntRange(int lightValue, ArrayList list)
    {
        foreach (int mapValue in list)
        {
            if (mapValue.Equals(lightValue)) return true;

        }
        return false;
    }
}