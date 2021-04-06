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

    [SerializeField]
    private Transform shootPoint;

    public UnityEvent OnMoveEvent;
    public UnityEvent OnShootEvent;
    public UnityEvent EndOfPath;
    
    [SerializeField] private GameObject prefabBullet;
    [SerializeField] private float timeBetweenShots = 0.3333f; // 3 shots per second
    [SerializeField] private float timestamp;
    [SerializeField] private int bulletSpeed = 8;

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

        if (distanceWithTarget() <= 12)
        {
            Debug.Log("In range");
            
            if (CanHitPlayer())
            {
                Debug.Log("Able to hit player");
                if (Time.time >= timestamp)
                {
                    Shoot();
                    OnShootEvent?.Invoke();
                }
            }
        }

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
    
    private float distanceWithTarget()
    {
        return Vector2.Distance(rb.position, target.position);
    }

    private void Shoot()
    {
        Debug.Log("Shooting");
        GameObject bullet =
            Instantiate(prefabBullet, shootPoint.position, Quaternion.identity) as GameObject;
        timestamp = Time.time + timeBetweenShots;
                    
        bullet.GetComponent<Rigidbody2D>().velocity = (shootPoint.position - rb.transform.position).normalized * bulletSpeed;
    }

    private bool CanHitPlayer()
    {
        Vector2 hitDirection = (Vector2) shootPoint.position - rb.position;

        GetComponent<Collider2D>().enabled = false;
        RaycastHit2D hit = Physics2D.Raycast(shootPoint.position, hitDirection, 100f);
        GetComponent<Collider2D>().enabled = true;

        if (hit.collider == null)
        {
            return false;
        }
        
        if (hit.collider.gameObject.tag.Equals(Values.PlayerTag))
        {
            return true;
        }

        return false;
    }
}
