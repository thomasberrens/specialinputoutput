using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Events;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float speed = 200f;

    [SerializeField]
    private float nextWaypointDistance = 3f;

    [SerializeField]
    private Transform enemyGFX;

    public UnityEvent OnMoveEvent;
    public UnityEvent EndOfPath;

    private Path path;
    private int currentWaypoint = 0;
    private bool endOfPath;

    private Seeker seeker;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, .5f);
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (path == null) return;
        if (target == null) return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            endOfPath = true;
            EndOfPath?.Invoke();
            return;
        } else
        {
            endOfPath = false;
        }
        
        Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        
        rb.AddForce(force);
        OnMoveEvent?.Invoke();
        
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
        
        if (force.x >= 0.01f)
        {
            enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
        } else if (force.x <= -0.01f)
        {
            enemyGFX.localScale = new Vector3(1f, 1f, 1f);
        }

        if (force.y >= 0.5f)
        {
            Debug.Log("Going up");
        } else if (force.y <= -0.01f)
        {
            Debug.Log("Going down");
        }
        
    }

    private void UpdatePath()
    {
        if (seeker.IsDone())
        {
            if (target == null) return;
            seeker.StartPath(rb.position, target.position, OnPathComplete); 
        }
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
}
