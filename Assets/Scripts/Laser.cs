using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Laser : MonoBehaviour
{
    [SerializeField] private int _speed = 12;
    [SerializeField] private float _duration = 4;
    private float _lifetime = 0;

    [SerializeField] private int _x;
    [SerializeField] private int _y;
    [SerializeField] private bool _homing = false;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(_x, _y, 0) * _speed * Time.deltaTime, Space.World);

        if (_homing)
        {
            GameObject nearestEnemy = null;
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject nextEnemy in enemies)
            {
                if (nextEnemy != null)
                {
                    if (nearestEnemy == null)
                    {
                        nearestEnemy = nextEnemy;
                    }
                    else if (Vector3.Distance(transform.position, nextEnemy.transform.position) <
                             Vector3.Distance(transform.position, nearestEnemy.transform.position))
                    {
                        nearestEnemy = nextEnemy;
                    }
                } 
            }

            if (nearestEnemy != null && Math.Abs(nearestEnemy.gameObject.transform.position.x - transform.position.x) > 0.3)
            {
                if (nearestEnemy.transform.position.x > transform.position.x)
                {
                    transform.Translate(new Vector3(1, 0, 0) * _speed * Time.deltaTime, Space.World);
                }
                else if (nearestEnemy.transform.position.x < transform.position.x)
                {
                    transform.Translate(new Vector3(-1, 0, 0) * _speed * Time.deltaTime, Space.World);
                }
            }
        }              

        if (transform.position.y > 10 || transform.position.x < -10 || transform.position.x > 10)
        {
            Destroy(gameObject);
        }

        _lifetime += Time.deltaTime;
        if (_lifetime > _duration)
        {
            Destroy(gameObject);
        }
    }
}
