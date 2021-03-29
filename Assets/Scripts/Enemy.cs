using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //shoot bullets sideways
    [SerializeField] private int _speed = 4;
    GameObject UIManager;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, 1, 0) * -_speed * Time.deltaTime, Space.World);
        if (transform.position.y < -8)
        {
            //transform.position = (new Vector3(Random.Range(-9, 9), 7, 0));
            Destroy(gameObject);
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
        UIManager = GameObject.Find("Canvas");
        UIManager uiManagerComponent = UIManager.transform.GetComponent<UIManager>();
        uiManagerComponent.AddScore(100);
        Destroy(gameObject);
    }
}
