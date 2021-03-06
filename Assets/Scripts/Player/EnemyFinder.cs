using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFinder : MonoBehaviour
{
    private ArrayList enemyObjects = new ArrayList();
    private ArrayList removalCache;
    
    [SerializeField] private Transform shootPoint;
    // Start is called before the first frame update
    void Start()
    {
        //   AddObjectsToList();
        shootPoint = gameObject.transform.Find(Values.ShootPoint);
    }

    public GameObject GetNearestTarget()
    {
        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach(GameObject potentialTarget in enemyObjects.ToArray())
        {
            if (potentialTarget == null)
            {
                this.enemyObjects.Remove(potentialTarget);
                continue;
            }
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

    private void Awake()
    {
        foreach (GameObject enemyObject in FindObjectsOfType<GameObject>())
        {
            if (enemyObject.tag.Equals(Values.EnemyTag))
            {
                Debug.Log("Found enemy with tag: " + enemyObject.name);
                this.enemyObjects.Add(enemyObject);
            }
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
