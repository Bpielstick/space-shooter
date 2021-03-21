using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _speed = 15;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private float fireRate = 0.2f;
    private float lastFired;

    // Start is called before the first frame update
    void Start()
    {
        //set the player to 0,0,0
        transform.position = new Vector3(0, 0, 0);
        lastFired = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        FireButtonListener();
    }

    private void FireButtonListener()
    {
        //while fire button is pressed, fire lasers
        if (Input.GetButtonDown("Fire1") && Time.time - lastFired > fireRate) 
        {
            Instantiate(_prefab, new Vector3(0,0.8f,0) + transform.position, Quaternion.identity);
            lastFired = Time.time;
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);


        transform.Translate(direction* _speed * Time.deltaTime);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -9.3f, 9.3f), Mathf.Clamp(transform.position.y, -4f, 4), 0);
    }

}
