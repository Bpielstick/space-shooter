using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0,1,0) * 12 * Time.deltaTime, Space.World);
        if (transform.position.y > 10)
        {
            Destroy(gameObject);
        }
    }
}
