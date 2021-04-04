using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
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
    private float _xDirection;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _xDirection = Random.Range(-1f * _speed, 1f * _speed);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(_xDirection, -1, 0) * Time.deltaTime, Space.World);
        if (transform.position.y < -8)
        {
            //transform.position = (new Vector3(Random.Range(-9, 9), 7, 0));
            Destroy(gameObject);
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
