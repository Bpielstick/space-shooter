using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileExplosion : MonoBehaviour
{
    [SerializeField] private AnimationClip _AnimationClip;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Cleanup());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Cleanup() 
    {
        yield return new WaitForSeconds(_AnimationClip.length);
        Destroy(gameObject);
    }

}
