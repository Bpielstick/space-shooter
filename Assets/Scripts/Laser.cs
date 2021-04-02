using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private int _speed = 12;
    [SerializeField] private int _x;
    [SerializeField] private int _y;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(_x, _y, 0) * _speed * Time.deltaTime, Space.World);
        if (transform.position.y > 10 || transform.position.x < -10 || transform.position.x > 10)
        {
            if (transform.parent != null) { Destroy(transform.parent.gameObject); }
            Destroy(gameObject);
        }
    }
}
