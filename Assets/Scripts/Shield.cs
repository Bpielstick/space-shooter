using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private int _health=3;
    [SerializeField] private bool _timelimited = true;
    [SerializeField] private bool _friendly = true;

    // Start is called before the first frame update
    void Start()
    {
        if (_timelimited)
        {
            StartCoroutine(ShieldTimerRoutine());
        }
    }

    IEnumerator ShieldTimerRoutine()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_friendly)
        {
            if (other.gameObject.tag == "EnemyLaser")
            {
                Destroy(other.gameObject);
                TakeDamage();
            }
        }
        else if (!_friendly)
        {
            switch (other.gameObject.tag)
            {
                case "Laser":
                    Destroy(other.gameObject);
                    TakeDamage();
                    break;

                case "Player":
                    Player player = other.transform.GetComponent<Player>();
                    if (player != null) { player.TakeDamage(); }
                    break;
            }
        }
    }

    public void TakeDamage()
    {
        if (_health > 1)
        {
            Animator _anim = this.GetComponent<Animator>();
            if (_anim == null) { Debug.Log("error anim is null"); }
            else { _anim.SetTrigger("Damaged"); }
                
            _health -= 1;
        }
        else if (_health == 1)
        {
            Destroy(this.gameObject);
        }
    }    

}
