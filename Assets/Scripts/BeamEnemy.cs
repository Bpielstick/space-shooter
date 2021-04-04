using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BeamEnemy : MonoBehaviour
{
    //shoot bullets sideways
    [SerializeField] private int _speed;
    GameObject UIManager;
    [SerializeField] AnimationClip Explode;
    [SerializeField] AnimationClip Fire;
    [SerializeField] int ScoreValue;
    [SerializeField] private AudioClip _explodeAudio;
    [SerializeField] private AudioClip _missileExplodeAudio;
    private AudioSource _audioSource;
    [SerializeField] private GameObject _beamlaser;
    [SerializeField] private AudioClip _laserAudio;
    [SerializeField] private GameObject _missileExplosion;
    private bool _dying = false;
    GameObject _myBeam;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        StartCoroutine(ShootAndMoveRoutine());
    }

    // Update is called once per frame
    void Update()
    {

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
        
        //Debug.Log("collided with " + other.tag);
        PreDestroy();
    }

    private void PreDestroy()
    {
        _dying = true;
        if (_myBeam != null) { GameObject.Destroy(_myBeam); }
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


    private IEnumerator ShootAndMoveRoutine()
    {

        while (transform.position.y > UnityEngine.Random.Range(4,-6))
        {
            transform.Translate(new Vector3(0, 1, 0) * -_speed * Time.deltaTime, Space.World);
            if (transform.position.y < -8)
            {
                //transform.position = (new Vector3(Random.Range(-9, 9), 7, 0));
                Destroy(gameObject);
            }

            RaycastHit2D hitleft = Physics2D.Raycast(new Vector2(transform.position.x - 1, transform.position.y - 1), Vector2.left, 1f);
            //Debug.DrawRay(new Vector2(transform.position.x - 1, transform.position.y - 1), Vector2.left, Color.red, 0.1f, false);
            RaycastHit2D hitright = Physics2D.Raycast(new Vector2(transform.position.x + 1, transform.position.y - 1), Vector2.right, 1f);
            //Debug.DrawRay(new Vector2(transform.position.x + 1, transform.position.y - 1), Vector2.right, Color.red, 0.1f, false);

            if (hitleft)
            {
                transform.Translate(new Vector3(-0.5f, 0, 0) * -_speed * Time.deltaTime, Space.World);
            }
            else if (hitright)
            {
                transform.Translate(new Vector3(0.5f, 0, 0) * -_speed * Time.deltaTime, Space.World);
            }

            yield return null;
        }

        Animator _anim = this.GetComponent<Animator>();
        _anim.SetTrigger("Fire");
        yield return new WaitForSeconds(Fire.length);
        Shoot();
        yield return new WaitForSeconds(3f);
        _anim.SetTrigger("Idle");
        yield return new WaitForSeconds(1f);

        while (true)
        {
            transform.Translate(new Vector3(0, 1, 0) * -_speed * Time.deltaTime, Space.World);
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
        if (!_dying)
        { 
        _myBeam = Instantiate(_beamlaser, new Vector3(0, -8.4f, 0) + transform.position, Quaternion.identity);
        _audioSource.PlayOneShot(_laserAudio);
        }
    }                         
}
    