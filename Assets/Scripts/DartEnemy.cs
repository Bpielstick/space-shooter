using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DartEnemy : MonoBehaviour
{
    [SerializeField] private int _speed;
    GameObject UIManager;
    [SerializeField] AnimationClip Explode;
    [SerializeField] int ScoreValue;
    [SerializeField] private AudioClip _explodeAudio;
    [SerializeField] private AudioClip _missileExplodeAudio;
    private AudioSource _audioSource;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _missileExplosion;
    private bool _dying = false;
    [SerializeField] private GameObject _enemyShield;
    [SerializeField] private GameObject _laser;
    [SerializeField] private AudioClip _laserAudio;
    private bool _hasfired = false;

    [SerializeField] private GameObject _collisionAvoidanceTrigger;
    private Vector3 _collisionLocation;
    private bool _avoidCollision = false;
    private bool _waitToMove = false;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _player = GameObject.FindGameObjectWithTag("Player");

        if (UnityEngine.Random.Range(1, 5) < 2)
        {
            GameObject ActiveShield = Instantiate(_enemyShield, this.transform.position + new Vector3(0, -0.7f, 0), Quaternion.identity, this.transform);
            ActiveShield.transform.SetParent(this.transform);
        }

        Instantiate(_collisionAvoidanceTrigger, this.transform.position + new Vector3(0, -0.6f, 0), Quaternion.identity, this.transform);
        StartCoroutine(MoveRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser" || other.tag == "EnemyLaser")
        {
            Destroy(other.gameObject);

        }
        else if (other.tag == "Shield")
        {
            other.GetComponent<Shield>().TakeDamage();
        }
        else if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null) { player.TakeDamage(); }
        }
        else if (other.tag == "Speed" || other.tag == "TripleShot" || other.tag == "ShieldPowerup" || other.tag == "Enemy" || other.tag == "MissileExplosion")
        {

        }
        else if (other.tag == "Missile")
        {
            Destroy(other.gameObject);
            Instantiate(_missileExplosion, other.transform.position, Quaternion.identity);
            _audioSource.clip = _missileExplodeAudio;
            _audioSource.Play();
        }

        if (other.tag != "Trigger")
        {
            PreDestroy();
        }
    }

    private void PreDestroy()
    {
        _dying = true;

        UIManager = GameObject.Find("Canvas");
        UIManager uiManagerComponent = UIManager.transform.GetComponent<UIManager>();
        uiManagerComponent.AddScore(ScoreValue);

        _audioSource.clip = _explodeAudio;
        _audioSource.PlayOneShot(_explodeAudio);

        BoxCollider2D Collider = this.GetComponent<BoxCollider2D>();
        Collider.enabled = false;

        Animator _anim = this.GetComponent<Animator>();
        if (_anim == null) { Debug.Log("error anim is null"); }
        _anim.SetTrigger("Explode");
        Destroy(gameObject, Explode.length);
    }

    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            if (_waitToMove)
            {
                //yield return new WaitForSeconds(0.5f);
                _waitToMove = false;
            }

            if (_avoidCollision)
            {
                if (_collisionLocation.x < transform.position.x)
                {
                    transform.Translate(new Vector3(1, 0, 0) * _speed * Time.deltaTime, Space.World);
                }
                if (_collisionLocation.x > transform.position.x)
                {
                    transform.Translate(new Vector3(-1, 0, 0) * _speed * Time.deltaTime, Space.World);
                }
                if (_collisionLocation.y < transform.position.y)
                {
                    transform.Translate(new Vector3(0, 1, 0) * _speed * Time.deltaTime, Space.World);
                }
                if (_collisionLocation.y > transform.position.y)
                {
                    transform.Translate(new Vector3(0, -1, 0) * _speed * Time.deltaTime, Space.World);
                }
            }
            else
            {
                if (_player != null)
                {
                    if (transform.position.x > _player.transform.position.x)
                    {
                        transform.Translate(new Vector3(-1f, 0, 0) * _speed * 0.4f * Time.deltaTime, Space.World);
                    }
                    else if (transform.position.x < _player.transform.position.x)
                    {
                        transform.Translate(new Vector3(1f, 0, 0) * _speed * 0.4f * Time.deltaTime, Space.World);
                    }

                    transform.Translate(new Vector3(0, 1, 0) * -_speed * Time.deltaTime, Space.World);
                }
            }

            if (transform.position.y < -8)
            {
                //transform.position = (new Vector3(Random.Range(-9, 9), 7, 0));
                Destroy(gameObject);
            }

            yield return null;
        }
    }

    private void Shoot()
    {
        if (_player != null)
        {
            Powerup[] powerups = GameObject.FindObjectsOfType(typeof(Powerup)) as Powerup[];
            foreach (Powerup powerup in powerups)
            {
                if (Math.Abs(powerup.gameObject.transform.position.x - transform.position.x) < 0.1 && !_hasfired && !_dying)
                {
                    if (transform.position.y < powerup.gameObject.transform.position.y)
                    {
                        Instantiate(_laser, new Vector3(0, 0.9f, 0) + transform.position, Quaternion.identity);
                        _audioSource.PlayOneShot(_laserAudio);
                        _hasfired = true;
                    }                
                }
            }

            if (Math.Abs(transform.position.x - _player.transform.position.x) < 0.1 && !_hasfired && !_dying)
            {
                if (transform.position.y < _player.transform.position.y)
                {
                    Instantiate(_laser, new Vector3(0, 0.9f, 0) + transform.position, Quaternion.identity);
                    _audioSource.PlayOneShot(_laserAudio);
                    _hasfired = true;
                }

            }
        }
    }

    public void AvoidCollision(Vector3 location)
    {
        _collisionLocation = location;
        _avoidCollision = true;
    }

    public void ClearCollision()
    {
        _waitToMove = true;
        _avoidCollision = false;
    }
}