using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(cleanup());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator cleanup()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
        yield break;
    }
}
