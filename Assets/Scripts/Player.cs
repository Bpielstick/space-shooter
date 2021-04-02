using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _baseThrusterSpeed = 10;
    [SerializeField] private int _currentSpeed = 6;
    [SerializeField] private int _speedBoosted = 15;
    [SerializeField] private int _thrusterSpeed = 10;
    [SerializeField] private GameObject _laser;
    [SerializeField] private GameObject _tripleShot;
    [SerializeField] private GameObject _shield;
    [SerializeField] private float _fireRate = 0.2f;
    private float _lastFired;
    [SerializeField] private int _health = 3;
    [SerializeField] private int _maxHealth = 3;
    private SpawnManager _spawnManager;
    private bool _powerupTripleShot = false;
    private bool _powerupSpeed = false;
    private GameObject _canvas;
    private UIManager _UIManager;
    [SerializeField] private GameObject _speedParticle;
    [SerializeField] AnimationClip Explode;
    [SerializeField] AudioClip _explodeAudio;
    [SerializeField] AudioClip _laserAudio;
    [SerializeField] AudioClip _powerupAudio;
    [SerializeField] AudioClip _outOfAmmo;
    private AudioSource _audioSource;
    [SerializeField] int _maxAmmo = 15;
    [SerializeField] int _currentAmmo = 15;
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        //set the player to 0,0,0
        transform.position = new Vector3(0, 0, 0);
        _lastFired = Time.time;

        _canvas = GameObject.Find("Canvas");
        _UIManager = _canvas.GetComponent<UIManager>();
        _UIManager.UpdateHealth(_health);

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null) { Debug.Log("error audio is null"); }
        _animator = GetComponent<Animator>();
        if (_animator == null) { Debug.Log("error animator is null"); }
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

        if (Input.GetButton("Fire3"))
        {
            transform.Translate(direction * _thrusterSpeed * Time.deltaTime);
        }
        else 
        {
            transform.Translate(direction * _currentSpeed * Time.deltaTime);
        }

        if (Input.GetButtonDown("Fire3"))
        {
            StartCoroutine(SpeedParticleRoutine());
        }

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -9.3f, 9.3f), Mathf.Clamp(transform.position.y, -4f, 4), 0);

        if (horizontalInput < 0)
        {
            gameObject.transform.GetChild(2).gameObject.SetActive(true);
            gameObject.transform.GetChild(4).gameObject.SetActive(false);
        } 
        else if (horizontalInput > 0)
        {
            gameObject.transform.GetChild(2).gameObject.SetActive(false);
            gameObject.transform.GetChild(4).gameObject.SetActive(true);
        }
        else if (horizontalInput == 0)
        {
            gameObject.transform.GetChild(4).gameObject.SetActive(false);
            gameObject.transform.GetChild(2).gameObject.SetActive(false);
        }
            
        if (verticalInput > 0)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
            gameObject.transform.GetChild(3).gameObject.SetActive(false);
        }
        else if (verticalInput < 0)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
            gameObject.transform.GetChild(3).gameObject.SetActive(true);
        }
        else if (verticalInput == 0)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
            gameObject.transform.GetChild(3).gameObject.SetActive(false);
        }

    }

    private void FireButtonListener()
    {
        if (Input.GetButtonDown("Fire1") && Time.time - _lastFired > _fireRate && _UIManager.GetComponent<UIManager>().gamestarted)
        {
            if (_currentAmmo > 0)
            {
                _lastFired = Time.time;
                FireWeapon();
                _currentAmmo -= 1;
                _UIManager.UpdateAmmo(_currentAmmo);
            } 
            else if (_currentAmmo == 0)
            {
                _audioSource.clip = _outOfAmmo;
                _audioSource.Play();
                _UIManager.UpdateAmmo(_currentAmmo);
            }
        }
    }

    private void FireWeapon()
    {                
        if (!_powerupTripleShot)
        {
            Instantiate(_laser, new Vector3(0, 0.8f, 0) + transform.position, Quaternion.identity);
            _audioSource.clip = _laserAudio;
            _audioSource.Play();
        }
        else if (_powerupTripleShot)
        {
            Instantiate(_tripleShot, new Vector3(0, 0.0f, 0) + transform.position, Quaternion.identity);
            _audioSource.clip = _laserAudio;
            _audioSource.PlayOneShot(_audioSource.clip);
            _audioSource.PlayDelayed(0.1f);
        }
    }

    public void TakeDamage()
    {
        if (_health > 1) 
        { 
            _health--;
            _UIManager.UpdateHealth(_health);
        }
        else if (_health == 1)
        {
            _health--;
            _UIManager.UpdateHealth(_health);
            _UIManager.GameOver(true);           
            _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

            if (_spawnManager == null)
            {
                Debug.LogError("spawn manager not found");
            }
            _spawnManager.OnPlayerDeath();
            Destroy(gameObject, Explode.length);
        }
                
        _animator.SetTrigger("Damaged");

        _audioSource.clip = _explodeAudio;
        _audioSource.Play();
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
                _audioSource.clip = _powerupAudio;
                _audioSource.Play();
                break;
            case "Speed":
                Destroy(other.gameObject);
                _powerupTripleShot = false;
                _powerupSpeed = true;
                _thrusterSpeed = _speedBoosted;
                StartCoroutine(PowerupTimerRoutine());                
                _audioSource.clip = _powerupAudio;
                _audioSource.Play();
                break;
            case "ShieldPowerup":
                Destroy(other.gameObject);
                GameObject ActiveShield = Instantiate(_shield, this.transform.position, Quaternion.identity, this.transform);
                ActiveShield.transform.SetParent(this.transform);
                _audioSource.clip = _powerupAudio;
                _audioSource.Play();
                break;
            case "EnemyLaser":
                TakeDamage();
                break;
            case "Ammo":
                Destroy(other.gameObject);
                _currentAmmo = _maxAmmo;
                _UIManager.UpdateAmmo(_currentAmmo);
                _audioSource.clip = _powerupAudio;
                _audioSource.Play();
                break;
            case "Health":
                Destroy(other.gameObject);
                if (_health < _maxHealth) 
                { 
                    _health += 1;
                    _UIManager.UpdateHealth(_health);
                    _animator.SetTrigger("Healed");
                }                
                _audioSource.clip = _powerupAudio;
                _audioSource.Play();                
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
            _thrusterSpeed = _baseThrusterSpeed;
        }
    }

    private IEnumerator SpeedParticleRoutine()
    {
        while (Input.GetButton("Fire3"))
        {
            if (_powerupSpeed)
            {
                Instantiate(_speedParticle, new Vector3(transform.position.x + UnityEngine.Random.Range(0.2f, -0.2f), transform.position.y - UnityEngine.Random.Range(0.2f, 0.4f), 0), Quaternion.identity);
                yield return new WaitForSeconds(0.01f);
            }
            else
            {
                Instantiate(_speedParticle, new Vector3(transform.position.x + UnityEngine.Random.Range(0.2f, -0.2f), transform.position.y - UnityEngine.Random.Range(0.2f, 0.4f), 0), Quaternion.identity);
                yield return new WaitForSeconds(0.04f);
            }
            
        }

    }

}
