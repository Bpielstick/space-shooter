using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _speed = 15;
    [SerializeField] private GameObject _laser;
    [SerializeField] private float _fireRate = 0.2f;
    private float _lastFired;
    [SerializeField] private int _health = 3;
    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        //set the player to 0,0,0
        transform.position = new Vector3(0, 0, 0);
        _lastFired = Time.time;
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("spawn manager not found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        FireButtonListener();
    }

    private void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -9.3f, 9.3f), Mathf.Clamp(transform.position.y, -4f, 4), 0);
    }

    private void FireButtonListener()
    {
        if (Input.GetButtonDown("Fire1") && Time.time - _lastFired > _fireRate)
        {
            _lastFired = Time.time;
            FireWeapon();
        }
    }

    private void FireWeapon()
    {
        Instantiate(_laser, new Vector3(0, 0.8f, 0) + transform.position, Quaternion.identity);
    }

    public void TakeDamage()
    {
            if (_health > 1) { _health--; }
            else 
            { 
                Destroy(gameObject);
                _spawnManager.OnPlayerDeath();
            }
     }

}
