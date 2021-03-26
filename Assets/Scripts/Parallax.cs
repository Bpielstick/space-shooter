using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    private float _ySize;
    private float _yLocation;
    [SerializeField] private float _speed;

    // Start is called before the first frame update
    void Start()
    {
        _yLocation = transform.position.y;
        _ySize = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, 1, 0) * -_speed * Time.deltaTime, Space.World);
        if (transform.position.y < -14.65f)
        {
            transform.position = (new Vector3(0, 14.65f, 0));

        }
    }
}
