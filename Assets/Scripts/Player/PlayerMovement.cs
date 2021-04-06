using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController2D controller;
    
    private Animator animator;

    private float horizontalmove = 0f;

    [SerializeField]
    private float runSpeed = 40f;
    
    [SerializeField]
    private Transform jumpPoint;
    private Transform shootPoint;
    [SerializeField]
    private Transform crouchPoint;

    private Rigidbody2D rb;

    private bool jump;
    private bool crouch;
    
    [SerializeField] private ArduinoInput ai;
    private ArrayList leftMap = new ArrayList();
    private ArrayList rightMap = new ArrayList();
    private ArrayList crouchMap = new ArrayList();

    private int LightValue1;
    private int LightValue2;

    private void Start()
    {
        
        for (int i = -20; i < 50; i++)
        {
            leftMap.Add(i);
        }
        
        for (int i = 50; i < 120; i++)
        {
            rightMap.Add(i);
        }
        
        for (int i = 0; i < 45; i++)
        {
            crouchMap.Add(i);
        }
        
        jumpPoint = gameObject.transform.Find(Values.JumpPoint);
        shootPoint = gameObject.transform.Find(Values.ShootPoint);
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<CharacterController2D>();
        animator = GetComponent<Animator>();
        ai = GetComponent<ArduinoInput>();
    }

    // Update is called once per frame
    void Update()
    {
        LightValue1 = ai.arduinoLightValues.L1;
        LightValue2 = ai.arduinoLightValues.L2;

        if (isInIntRange(LightValue1, rightMap))
        {
            horizontalmove = 1 * runSpeed;
            
            shootPoint.Rotate(new Vector2(180, 0));
        }
        else if (isInIntRange(LightValue1, leftMap))
        {
            horizontalmove = -1 * runSpeed;
            shootPoint.Rotate(new Vector2(180, 0));
        }
        else
        {
            horizontalmove = 0;
        }
        //    horizontalmove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Math.Abs(horizontalmove));
        
        if (isInIntRange(LightValue2, crouchMap))
        {
            crouch = true;
        } else if (crouch)
        {
            crouch = false;
        }


        if (needToJump() && !crouch)
        {
            Debug.Log("Needs to jump");
            jump = true;
            animator.SetBool("isJumping", true);
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
    }

    public void onCrouching(bool isCrouching)
    {
        animator.SetBool("isCrouching", isCrouching);
    }

    private bool needToJump()
    {
        Vector2 hitDirection = (Vector2) jumpPoint.position - rb.position;

        GetComponent<Collider2D>().enabled = false;
        RaycastHit2D hit = Physics2D.Raycast(jumpPoint.position, hitDirection, 2f);
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