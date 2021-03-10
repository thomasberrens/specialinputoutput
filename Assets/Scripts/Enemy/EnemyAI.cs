using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{

    public Transform target;

    public float speed = 200f;

    public float nextWaypointDistance = 3f;

    public Transform enemyGFX;

    private Path path;
    private int currentWaypoint = 0;
    private bool endOfPath;

    private Seeker seeker;
    private Rigidbody2D rb;

    public LayerMask layerMask;

    public Transform shootPoint;
    
    [SerializeField] private GameObject prefabBullet;
    [SerializeField] private float timeBetweenShots = 0.3333f; // 3 shots per second
    [SerializeField] private float timestamp;
    [SerializeField] private int bulletSpeed = 8;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, .5f);
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null) return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            endOfPath = true;
            return;
        } else
        {
            endOfPath = false;
        }

        Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);
        
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
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete); 
        }
    }

    void OnPathComplete(Path p)
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
        
        if (hit.collider.gameObject.name.Equals("Player"))
        {
            return true;
        }

        return false;
    }
}
