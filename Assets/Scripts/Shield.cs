using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
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
}
