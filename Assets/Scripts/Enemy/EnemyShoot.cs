using System;
using UnityEngine;
using UnityEngine.Events;

public class EnemyShoot : MonoBehaviour
{

   [SerializeField] private GameObject target;
   [SerializeField] private GameObject prefabBullet;
   [SerializeField] private float timeBetweenShots = 0.3333f; // 3 shots per second
   [SerializeField] private float timestamp;
   [SerializeField] private int bulletSpeed = 8;
   [SerializeField] private Transform shootPoint;
   [SerializeField] private int maxDistanceWithTarget;
   
   public UnityEvent OnShootEvent;
    private void Start()
    {
        target = GameObject.FindWithTag(Values.PlayerTag);
    }

    private void Update()
    {
        if (target == null) return;
        if (distanceWithTarget() <= maxDistanceWithTarget) { 
            if (Time.time >= timestamp) {
                if (CanHitPlayer()) {
                    Shoot();
                    OnShootEvent?.Invoke();
                }
            }
        }
    }
    
    private float distanceWithTarget()
    {
        return Vector2.Distance(transform.position, target.transform.position);
    }
    
    private void Shoot()
    {
        GameObject bullet =
            Instantiate(prefabBullet, shootPoint.position, Quaternion.identity) as GameObject;
        timestamp = Time.time + timeBetweenShots;
                    
        bullet.GetComponent<Rigidbody2D>().velocity = (shootPoint.position - transform.position).normalized * bulletSpeed;
    }

    private bool CanHitPlayer()
    {
        Vector2 hitDirection = (Vector2) shootPoint.position - (Vector2) transform.position;

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
