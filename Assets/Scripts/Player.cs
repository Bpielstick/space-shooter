using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _speed;

    // Start is called before the first frame update
    void Start()
    {
        //set the player to 0,0,0
        transform.position = new Vector3(0, 0, 0);
           
    }

    // Update is called once per frame
    void Update()
    {
        calculateMovement();
    }

    void calculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);


        transform.Translate(direction* _speed * Time.deltaTime);

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -9.3f, 9.3f), Mathf.Clamp(transform.position.y, -4f, 4), 0);

        /*
        if (transform.position.x > 4)
        {
            transform.position = new Vector3(transform.position.x, 4, 0);
        }
        else if (transform.position.y < -4)
        {
            transform.position = new Vector3(transform.position.x, -4, 0);
        }

        if (transform.position.y > 9.3f)
        {
            transform.position = new Vector3(9.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -9.3f)
        {
            transform.position = new Vector3(-9.3f, transform.position.y, 0);
        }
        */
    }

}
