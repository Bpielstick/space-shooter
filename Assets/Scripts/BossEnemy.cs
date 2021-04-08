using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    [SerializeField] private GameObject _laser;
    [SerializeField] private AudioClip _laserAudio;
    [SerializeField] private GameObject _missileExplosion;
    [SerializeField] private AudioClip _missileExplodeAudio;
    [SerializeField] private AudioClip _explodeAudio;
    [SerializeField] AnimationClip Explode;
    private AudioSource _audioSource;
    private Animator _anim;
    GameObject UIManager;
    int _xVector = 0;
    [SerializeField] private int _speed = 4;
    public int health = 30;
    bool _dying = false;
    [SerializeField] int ScoreValue;

    Vector3[] gunPositions = { new Vector3(-2, -2.2f, 0),
                               new Vector3(-0.2f, -1.6f, 0),
                               new Vector3(0.2f, -1.6f, 0),
                               new Vector3(2, -2.2f, 0) };

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
        StartCoroutine(MainEncounterRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator MainEncounterRoutine()
    {
        while (transform.position.y > 6)
        {
            transform.Translate(new Vector3(0, -1, 0) * _speed * Time.deltaTime, Space.World);
            yield return null;
        }

        StartCoroutine(ShootRoutine());

        while (true)
        {
            if (_xVector == 0)
            {
                int newxVector = Random.Range(0, 2) * 2 - 1;
                _xVector = newxVector;
            }

            if (transform.position.x >= 6)
            {
                _xVector = -1;
            }
            else if (transform.position.x <= -6)
            {
                _xVector = 1;
            }

            transform.Translate(new Vector3(_xVector, 0, 0) * _speed * Time.deltaTime, Space.World);

            yield return null;
        }
    }

    private IEnumerator ShootRoutine()
    {
        while (true)
        {
            foreach (Vector3 gun in gunPositions)
            {
                yield return new WaitForSeconds(Random.Range(0.1f, 0.75f));
                Instantiate(_laser, gun + transform.position, Quaternion.identity);
                _audioSource.PlayOneShot(_laserAudio);
            }

            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser" || other.tag == "EnemyLaser")
        {
            Destroy(other.gameObject);

        }
        else if (other.tag == "Shield")
        {
            Destroy(other.gameObject);
        }
        else if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null) { player.TakeDamage(); player.TakeDamage(); player.TakeDamage(); }
        }
        else if (other.tag == "Speed" || other.tag == "TripleShot" || other.tag == "ShieldPowerup" || other.tag == "Enemy" || other.tag == "MissileExplosion")
        {
            Destroy(other.gameObject);
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
            TakeDamage();
            if (health <= 0)
            {
                PreDestroy();
            }
        }
    }

    public void TakeDamage()
    {
        if (health > 0)
        {
            health--;
            _anim.SetTrigger("Flash");
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
        
        if (_anim == null) { Debug.Log("error anim is null"); }
        _anim.SetTrigger("Explode");
        Destroy(gameObject, Explode.length);
    }

}
