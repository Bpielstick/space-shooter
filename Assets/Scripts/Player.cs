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
    [SerializeField] private GameObject _speedParticle;
    [SerializeField] AnimationClip Explode;
    [SerializeField] AudioClip _explodeAudio;
    [SerializeField] AudioClip _laserAudio;
    [SerializeField] AudioClip _powerupAudio;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        //set the player to 0,0,0
        transform.position = new Vector3(0, 0, 0);
        _lastFired = Time.time;

        _canvas = GameObject.Find("Canvas");
        _UIManager = _canvas.GetComponent<UIManager>();
        _UIManager.UpdateHealth(_health);

        audioSource = GetComponent<AudioSource>();
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
            audioSource.clip = _laserAudio;
            audioSource.Play();
        }
        else if (_powerupTripleShot)
        {
            Instantiate(_tripleShot, new Vector3(0, 0.0f, 0) + transform.position, Quaternion.identity);
            audioSource.clip = _laserAudio;
            audioSource.PlayOneShot(audioSource.clip);
            audioSource.PlayDelayed(0.1f);
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
            _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

            if (_spawnManager == null)
            {
                Debug.LogError("spawn manager not found");
            }
            _spawnManager.OnPlayerDeath();
            Destroy(gameObject, Explode.length);
        }

        Animator _anim = this.GetComponent<Animator>();
        if (_anim == null) { Debug.Log("error anim is null"); }
        _anim.SetTrigger("Damaged");

        audioSource.clip = _explodeAudio;
        audioSource.Play();
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
                audioSource.clip = _powerupAudio;
                audioSource.Play();
                break;
            case "Speed":
                Destroy(other.gameObject);
                _powerupTripleShot = false;
                _powerupSpeed = true;
                _currentSpeed = _speedBoosted;
                StartCoroutine(PowerupTimerRoutine());
                StartCoroutine(SpeedParticleRoutine());
                audioSource.clip = _powerupAudio;
                audioSource.Play();
                break;
            case "ShieldPowerup":
                Destroy(other.gameObject);
                GameObject ActiveShield = Instantiate(_shield, this.transform.position, Quaternion.identity, this.transform);
                ActiveShield.transform.SetParent(this.transform);
                audioSource.clip = _powerupAudio;
                audioSource.Play();
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

    private IEnumerator SpeedParticleRoutine()
    {
        while (_powerupSpeed)
        {
            Instantiate(_speedParticle, new Vector3(transform.position.x + UnityEngine.Random.Range(0.2f,-0.2f), transform.position.y-0.5f, 0), Quaternion.identity);
            yield return new WaitForSeconds(0.05f);
        }

    }

}
