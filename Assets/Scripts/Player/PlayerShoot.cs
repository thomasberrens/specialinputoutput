﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject prefabBullet;
    [SerializeField] private Transform shootPoint;
    
    [SerializeField] private float timeBetweenShots = 0.3333f; // 3 shots per second
    [SerializeField] private float timestamp;
    [SerializeField] private int bulletSpeed = 10;
    
    [SerializeField] private GameObject currentTarget;
    
    [SerializeField] private ArduinoInput ai;
    private int LightValue2;
    
    private ArrayList shootMap = new ArrayList();

    public UnityEvent wantsToShoot;
    // Start is called before the first frame update
    void Start()
    {
        shootPoint = gameObject.transform.Find(Values.ShootPoint);
        ai = GetComponent<ArduinoInput>();
        
        for (int i = 50; i < 70; i++)
        {
            shootMap.Add(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        LightValue2 = ai.arduinoLightValues.L2;

        if (Time.time >= timestamp)
        {
            if (isInIntRange(LightValue2, shootMap))
            {
                wantsToShoot?.Invoke();
                if (currentTarget == null)
                {
                    Debug.Log("Target is null");
                    return;
                }

                Shoot();
            }
        }
    }

    public void SetTarget(GameObject target)
    {
        this.currentTarget = target;
    }

    public GameObject GetTarget()
    {
        return currentTarget;
    }
    
    private bool isInIntRange(int lightValue, ArrayList list)
    {
        foreach (int mapValue in list)
        {
            if (mapValue.Equals(lightValue)) return true;

        }
        return false;
    }
    
    
    private void Shoot()
    {
        Debug.Log("Target = " + currentTarget.name);
        GameObject bullet =
            Instantiate(prefabBullet, shootPoint.position, Quaternion.identity) as GameObject;
        timestamp = Time.time + timeBetweenShots;
                    
        bullet.GetComponent<Rigidbody2D>().velocity = (currentTarget.transform.position - shootPoint.position).normalized * bulletSpeed;
    }
}
