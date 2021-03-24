using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemy;
    [SerializeField] private GameObject _container;
    private bool _stopSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnRoutine()
    {
       while (!_stopSpawning)
        {
            GameObject newObject = Instantiate(_enemy, new Vector3(Random.Range(-9, 9), 7, 0), Quaternion.identity);
            newObject.transform.parent = _container.transform;
            yield return new WaitForSeconds(0.5f);
        }    
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
