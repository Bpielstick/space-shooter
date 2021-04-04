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
    private int _waveOneSize = 10;
    private int _waveTwoSize = 20;
    private int _waveThreeSize = 30;
    private int _waveFourSize = 40;
    private int _waveFiveSize = 50;
    private GameObject _canvas;
    private UIManager _UIManager;
    //explosion powerup

    // Start is called before the first frame update
    void Start()
    {
        _canvas = GameObject.Find("Canvas");
        _UIManager = _canvas.GetComponent<UIManager>();
        

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
            if (Random.Range(0,2) < 1)
            {
                GameObject PowerupToSpawn = powerups[Random.Range(0, powerups.Length)];
                SpawnPowerup(PowerupToSpawn);
            }
            else
            {
                GameObject PowerupToSpawn = powerups[Random.Range(0, powerups.Length-1)];
                SpawnPowerup(PowerupToSpawn);
            }          
            
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
        yield return new WaitForSeconds(0.5f);
        _UIManager.UpdateWave(1);
        yield return new WaitForSeconds(5);
        StartCoroutine(WaveOneRoutine());
        yield break;
    }

    private IEnumerator WaveOneRoutine()
    {
        int spawncount =0;
        while (!_stopSpawning)
        {
            GameObject newObject = Instantiate(_enemy, new Vector3(Random.Range(-9, 10), 7, 0), Quaternion.identity);
            newObject.transform.parent = _container.transform;
            yield return new WaitForSeconds(0.5f);
            spawncount += 1;
            if (spawncount >= _waveOneSize)
            {
                _UIManager.UpdateWave(2);
                yield return new WaitForSeconds(5);
                StartCoroutine(WaveTwoRoutine());
                yield break;
            }
        }
    }

    private IEnumerator WaveTwoRoutine()
    {
        int spawncount = 0;
        while (!_stopSpawning)
        {
            GameObject newObject = Instantiate(_enemy, new Vector3(Random.Range(-9, 10), 7, 0), Quaternion.identity);
            newObject.transform.parent = _container.transform;
            yield return new WaitForSeconds(0.4f);
            spawncount += 1;
            if (spawncount >= _waveTwoSize)
            {
                _UIManager.UpdateWave(3);
                yield return new WaitForSeconds(5);
                StartCoroutine(WaveThreeRoutine());
                yield break;
            }
        }
    }

    private IEnumerator WaveThreeRoutine()
    {
        int spawncount = 0;
        while (!_stopSpawning)
        {
            GameObject newObject = Instantiate(_enemy, new Vector3(Random.Range(-9, 10), 7, 0), Quaternion.identity);
            newObject.transform.parent = _container.transform;
            yield return new WaitForSeconds(0.3f);
            spawncount += 1;
            if (spawncount >= _waveThreeSize)
            {
                _UIManager.UpdateWave(4);
                yield return new WaitForSeconds(5);
                StartCoroutine(WaveFourRoutine());
                yield break;
            }
        }
    }

    private IEnumerator WaveFourRoutine()
    {
        int spawncount = 0;
        while (!_stopSpawning)
        {
            GameObject newObject = Instantiate(_enemy, new Vector3(Random.Range(-9, 10), 7, 0), Quaternion.identity);
            newObject.transform.parent = _container.transform;
            yield return new WaitForSeconds(0.2f);
            spawncount += 1;
            if (spawncount >= _waveFourSize)
            {
                _UIManager.UpdateWave(5);
                yield return new WaitForSeconds(5);
                StartCoroutine(WaveFiveRoutine());
                yield break;
            }
        }
    }

    private IEnumerator WaveFiveRoutine()
    {
        int spawncount = 0;
        while (!_stopSpawning)
        {
            GameObject newObject = Instantiate(_enemy, new Vector3(Random.Range(-9, 10), 7, 0), Quaternion.identity);
            newObject.transform.parent = _container.transform;
            yield return new WaitForSeconds(0.1f);
            spawncount += 1;
            if (spawncount >= _waveFiveSize)
            {
                //do something here
            }
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
