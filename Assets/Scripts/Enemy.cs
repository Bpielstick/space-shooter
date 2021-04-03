using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    //shoot bullets sideways
    [SerializeField] private int _speed;
    GameObject UIManager;
    [SerializeField] AnimationClip Explode;
    [SerializeField] int ScoreValue;
    [SerializeField] private AudioClip _explodeAudio;
    [SerializeField] private AudioClip _missileExplodeAudio;
    private AudioSource _audioSource;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _leftlaser;
    [SerializeField] private GameObject _rightlaser;
    [SerializeField] private AudioClip _laserAudio;
    [SerializeField] private GameObject _missileExplosion;
    private bool _hasfired = false;
    private bool _dying = false;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, 1, 0) * -_speed * Time.deltaTime, Space.World);
        if (transform.position.y < -8)
        {
            //transform.position = (new Vector3(Random.Range(-9, 9), 7, 0));
            Destroy(gameObject);
        }

        if (_player != null)
        {
            if (Math.Abs(transform.position.y - _player.transform.position.y) < 0.5 && !_hasfired && !_dying && UnityEngine.Random.Range(1, 3) > 1)
            {
                if (transform.position.x > _player.transform.position.x)
                {
                    Instantiate(_leftlaser, new Vector3(-0.5f, -0.6f, 0) + transform.position, Quaternion.identity);
                    _audioSource.PlayOneShot(_laserAudio);
                    _hasfired = true;
                }
                else if (transform.position.x < _player.transform.position.x)
                {
                    Instantiate(_rightlaser, new Vector3(0.5f, -0.6f, 0) + transform.position, Quaternion.identity);
                    _audioSource.PlayOneShot(_laserAudio);
                    _hasfired = true;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
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

        PreDestroy();
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

}
