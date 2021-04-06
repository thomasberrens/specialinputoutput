using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFinder : MonoBehaviour
{
    private ArrayList enemyObjects;
    
    [SerializeField] private Transform shootPoint;
    // Start is called before the first frame update
    void Start()
    {
        this.enemyObjects = new ArrayList();
        AddObjectsToList();
        shootPoint = gameObject.transform.Find(Values.ShootPoint);
    }
    
    public GameObject GetNearestTarget()
    {
        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach(GameObject potentialTarget in enemyObjects)
        {
              if (!CanHitEnemy(potentialTarget))
             {
                  Debug.Log("Found a near enemy, but it is impossible to shoot him");
                  continue;
              }
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if(dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }

    private void AddObjectsToList()
    {
        foreach (GameObject enemyObject in GameObject.FindGameObjectsWithTag(Values.EnemyTag))
        {
            this.enemyObjects.Add(enemyObject);
        }
    }
    
    private bool CanHitEnemy(GameObject target)
    {
        Vector2 hitDirection = (Vector2) target.transform.position - (Vector2) shootPoint.position;

        GetComponent<Collider2D>().enabled = false;
        RaycastHit2D hit = Physics2D.Raycast(shootPoint.position, hitDirection, 100f);
        GetComponent<Collider2D>().enabled = true;

        if (hit.collider == null)
        {
            return false;
        }
        
        if (hit.collider.gameObject.tag.Equals(Values.EnemyTag))
        {
            return true;
        }

        return false;
    }
}
