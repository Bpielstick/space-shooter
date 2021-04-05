using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private int _speed = 3;
    [SerializeField] private AudioClip _explodeAudio;
    [SerializeField] AnimationClip Explode;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, 1, 0) * -_speed * Time.deltaTime, Space.World);
        if (transform.position.y < -8)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyLaser" || other.tag == "BeamLaser")
        {
            GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine(PowerupExplode());
        }
    }

    private IEnumerator PowerupExplode()
    {
        Animator anim = gameObject.GetComponent<Animator>();
        anim.SetTrigger("Explode");

        AudioSource AudioSource = GetComponent<AudioSource>();
        AudioSource.clip = _explodeAudio;
        AudioSource.Play();

        Destroy(gameObject, Explode.length);
        yield break;
    }
}
