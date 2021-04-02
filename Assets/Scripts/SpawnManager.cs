using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemy;
    [SerializeField] GameObject[] powerups;
    [SerializeField] GameObject[] resourcePowerups;
    [SerializeField] private GameObject _container;
    private bool _stopSpawning = false;
    [SerializeField] private float _spawnRate = 1.0f;
    [SerializeField] private float _powerupRate = 6.0f;
    //explosion powerup

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRoutine());
        StartCoroutine(IncreaseSpawnRate());
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnResourcePowerupRoutine());
        this.enabled = true;
    }

    private IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(10);
        while (!_stopSpawning)
        {            
            GameObject PowerupToSpawn = powerups[Random.Range(0, powerups.Length)];

            SpawnPowerup(PowerupToSpawn);

            yield return new WaitForSeconds(_powerupRate + Random.Range(_powerupRate*0.5f,_powerupRate*1.5f));
        }
    }

    private IEnumerator SpawnResourcePowerupRoutine()
    {
        yield return new WaitForSeconds(5);
        while (!_stopSpawning)
        {
            GameObject PowerupToSpawn = resourcePowerups[Random.Range(0, resourcePowerups.Length)];

            SpawnPowerup(PowerupToSpawn);

            yield return new WaitForSeconds(_powerupRate + Random.Range(_powerupRate * 0.25f, _powerupRate * 0.5f));
        }
    }

    private void SpawnPowerup(GameObject PowerupToSpawn)
    {
        if (PowerupToSpawn != null)
        {
            GameObject newObject = Instantiate(PowerupToSpawn, new Vector3(Random.Range(-9, 10), 7, 0), Quaternion.identity);
            newObject.transform.parent = _container.transform;
        }
    }

    private IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(2);
        while (!_stopSpawning)
        {
            GameObject newObject = Instantiate(_enemy, new Vector3(Random.Range(-9, 10), 7, 0), Quaternion.identity);
            newObject.transform.parent = _container.transform;
            yield return new WaitForSeconds(_spawnRate);
        }    
    }

    private IEnumerator IncreaseSpawnRate()
    {
        while (_spawnRate > 0.1f)
        {
            _spawnRate -= 0.1f;
            yield return new WaitForSeconds(10);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
