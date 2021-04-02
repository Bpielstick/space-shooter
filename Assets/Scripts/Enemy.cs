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
    private AudioSource audioSource;
    [SerializeField] GameObject _player;
    [SerializeField] GameObject _leftlaser;
    [SerializeField] GameObject _rightlaser;
    [SerializeField] AudioClip _laserAudio;
    private bool _hasfired = false;
    private bool _dying = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
            if (Math.Round(transform.position.y, 1) - 0.5 == Math.Round(_player.transform.position.y, 1) && !_hasfired && !_dying)
            {
                if (transform.position.x > _player.transform.position.x)
                {
                    Instantiate(_leftlaser, new Vector3(-0.5f, -0.6f, 0) + transform.position, Quaternion.identity);
                    audioSource.PlayOneShot(_laserAudio);
                    _hasfired = true;
                }
                else if (transform.position.x < _player.transform.position.x)
                {
                    Instantiate(_rightlaser, new Vector3(0.5f, -0.6f, 0) + transform.position, Quaternion.identity);
                    audioSource.PlayOneShot(_laserAudio);
                    _hasfired = true;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser" || other.tag == "Shield")
        {
            Destroy(other.gameObject);

        }
        else if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null) { player.TakeDamage(); }
        }
        else if (other.tag == "Speed" || other.tag == "TripleShot" || other.tag == "ShieldPowerup" || other.tag == "Enemy")
        {

        } 

        PreDestroy();
    }

    private void PreDestroy()
    {
        _dying = true;

        UIManager = GameObject.Find("Canvas");
        UIManager uiManagerComponent = UIManager.transform.GetComponent<UIManager>();
        uiManagerComponent.AddScore(ScoreValue);

        audioSource.clip = _explodeAudio;
        audioSource.PlayOneShot(_explodeAudio);

        BoxCollider2D Collider = this.GetComponent<BoxCollider2D>();
        Collider.enabled = false;

        Animator _anim = this.GetComponent<Animator>();
        if (_anim == null) { Debug.Log("error anim is null"); }
        _anim.SetTrigger("Explode");
        Destroy(gameObject, Explode.length);
    }

}
