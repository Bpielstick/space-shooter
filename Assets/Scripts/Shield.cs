using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private int _health=3;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShieldTimerRoutine());
    }

    IEnumerator ShieldTimerRoutine()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "EnemyLaser")
        {
            Destroy(other.gameObject);
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        if (_health > 1)
        {
            Animator _anim = this.GetComponent<Animator>();
            if (_anim == null) { Debug.Log("error anim is null"); }
            _anim.SetTrigger("Damaged");
            _health -= 1;
        }
        else if (_health == 1)
        {
            Destroy(this.gameObject);
        }
    }    

}
