using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _baseSpeed = 8;
    [SerializeField] private int _currentSpeed = 8;
    [SerializeField] private int _speedBoosted = 15;
    [SerializeField] private GameObject _laser;
    [SerializeField] private GameObject _tripleShot;
    [SerializeField] private GameObject _shield;
    [SerializeField] private float _fireRate = 0.2f;
    private float _lastFired;
    [SerializeField] private int _health = 3;
    private SpawnManager _spawnManager;
    private bool _powerupTripleShot = false;
    private bool _powerupSpeed = false;
    private GameObject _canvas;
    private UIManager _UIManager;

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

        _canvas = GameObject.Find("Canvas");
        _UIManager = _canvas.GetComponent<UIManager>();
        _UIManager.UpdateHealth(_health);
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

        transform.Translate(direction * _currentSpeed * Time.deltaTime);
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
        if (!_powerupTripleShot)
        {
            Instantiate(_laser, new Vector3(0, 0.8f, 0) + transform.position, Quaternion.identity);
        }
        else if (_powerupTripleShot)
        {
            Instantiate(_tripleShot, new Vector3(0, 0.0f, 0) + transform.position, Quaternion.identity);
        }
    }

    public void TakeDamage()
    {
        if (_health > 1) 
        { 
            _health--;
            _UIManager.UpdateHealth(_health);
        }
        else
        {
            _health--;
            _UIManager.UpdateHealth(_health);
            _UIManager.GameOver(true);
            Destroy(gameObject);
            _spawnManager.OnPlayerDeath();
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch(other.tag)
        {
            case "TripleShot":
                Destroy(other.gameObject);
                _powerupSpeed = false;
                _powerupTripleShot = true;
                StartCoroutine(PowerupTimerRoutine());
                break;
            case "Speed":
                Destroy(other.gameObject);
                _powerupTripleShot = false;
                _powerupSpeed = true;
                _currentSpeed = _speedBoosted;
                StartCoroutine(PowerupTimerRoutine());
                break;
            case "ShieldPowerup":
                Destroy(other.gameObject);
                GameObject ActiveShield = Instantiate(_shield, this.transform.position, Quaternion.identity, this.transform);
                ActiveShield.transform.SetParent(this.transform);
                break;
        }
    }

    private IEnumerator PowerupTimerRoutine()
    {
        while (_powerupTripleShot || _powerupSpeed)
        {
            yield return new WaitForSeconds(5);
            _powerupTripleShot = false;
            _powerupSpeed = false;
            _currentSpeed = _baseSpeed;
        }
        

    }
}
