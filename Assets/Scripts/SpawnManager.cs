using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemy;
    [SerializeField] private GameObject _beamEnemy;
    [SerializeField] private GameObject _dartEnemy;
    [SerializeField] private GameObject _boss;
    [SerializeField] private GameObject _asteroid;
    [SerializeField] GameObject[] powerups;
    [SerializeField] GameObject[] resourcePowerups;
    [SerializeField] private GameObject _container;
    private bool _stopSpawning = false;
    [SerializeField] private float _powerupRate = 5.0f;
    private int _waveOneSize = 15;
    private int _waveTwoSize = 15;
    private int _waveThreeSize = 15;
    private int _waveFourSize = 15;
    private int _waveFiveSize = 30;
    private GameObject _canvas;
    private UIManager _UIManager;
    private int _roundCount = 1;
    //explosion powerup

    // Start is called before the first frame update
    void Start()
    {
        _canvas = GameObject.Find("Canvas");
        _UIManager = _canvas.GetComponent<UIManager>();
        

        StartCoroutine(SpawnRoutine());
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
        yield return new WaitForSeconds(8);
        while (!_stopSpawning)
        {
            GameObject PowerupToSpawn = resourcePowerups[Random.Range(0, resourcePowerups.Length)];

            SpawnPowerup(PowerupToSpawn);

            yield return new WaitForSeconds(_powerupRate + Random.Range(_powerupRate * 0.1f, _powerupRate * 0.25f));
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

            GameObject newObject = Instantiate(_dartEnemy, new Vector3(Random.Range(-9, 10), 7, 0), Quaternion.identity);
            newObject.transform.parent = _container.transform;

            yield return new WaitForSeconds(0.5f);
            spawncount += 1;
            if (spawncount >= _waveOneSize * _roundCount)
            {
                yield return new WaitForSeconds(1);
                StartCoroutine(SpawnAsteroids());
                yield return new WaitForSeconds(2);
                _UIManager.UpdateWave(2 + (_roundCount - 1) * 5);
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
            GameObject newObject = Instantiate(_beamEnemy, new Vector3(Random.Range(-9, 10), 7, 0), Quaternion.identity);
            newObject.transform.parent = _container.transform;

            yield return new WaitForSeconds(0.7f);
            spawncount += 1;
            if (spawncount >= _waveTwoSize * _roundCount)
            {
                yield return new WaitForSeconds(1);
                StartCoroutine(SpawnAsteroids());
                yield return new WaitForSeconds(2);
                _UIManager.UpdateWave(3 + (_roundCount - 1) * 5);
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
            int spawnchance = Random.Range(1, 10);
            if (spawnchance < 3)
            {
                GameObject newObject = Instantiate(_beamEnemy, new Vector3(Random.Range(-9, 10), 7, 0), Quaternion.identity);
                newObject.transform.parent = _container.transform;
            }
            else
            {
                GameObject newObject = Instantiate(_dartEnemy, new Vector3(Random.Range(-9, 10), 7, 0), Quaternion.identity);
                newObject.transform.parent = _container.transform;
            }
            yield return new WaitForSeconds(0.7f);
            spawncount += 1;
            if (spawncount >= _waveThreeSize * _roundCount)
            {
                yield return new WaitForSeconds(1);
                StartCoroutine(SpawnAsteroids());
                yield return new WaitForSeconds(2);
                _UIManager.UpdateWave(4 + (_roundCount - 1) * 5);
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

            yield return new WaitForSeconds(0.7f);
            spawncount += 1;
            if (spawncount >= _waveFourSize * _roundCount)
            {
                yield return new WaitForSeconds(1);
                StartCoroutine(SpawnAsteroids());
                yield return new WaitForSeconds(2);
                _UIManager.UpdateWave(5 + (_roundCount - 1) * 5);
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
            int spawnchance = Random.Range(1, 10);
            if (spawnchance < 2)
            {
                GameObject newObject = Instantiate(_beamEnemy, new Vector3(Random.Range(-9, 10), 7, 0), Quaternion.identity);
                newObject.transform.parent = _container.transform;
            }
            else if  (spawnchance < 5)
            {
                GameObject newObject = Instantiate(_dartEnemy, new Vector3(Random.Range(-9, 10), 7, 0), Quaternion.identity);
                newObject.transform.parent = _container.transform;
            } else
            {
                GameObject newObject = Instantiate(_enemy, new Vector3(Random.Range(-9, 10), 7, 0), Quaternion.identity);
                newObject.transform.parent = _container.transform;
            }
            yield return new WaitForSeconds(0.7f);
            spawncount += 1;
            if (spawncount >= _waveFiveSize)
            {
                StartCoroutine(BossRoutine());
                yield break;
            }
        }
    }

    private IEnumerator BossRoutine()
    {
        GameObject boss = Instantiate(_boss, new Vector3(0, 10, 0), Quaternion.identity);
        while (boss != null)
        {
            if (boss.GetComponent<BossEnemy>().health <= 0)
            {
                _roundCount++;
                yield return new WaitForSeconds(1);
                StartCoroutine(SpawnAsteroids());
                yield return new WaitForSeconds(2);
                _UIManager.UpdateWave(1 + (_roundCount - 1) * 5);
                yield return new WaitForSeconds(5);
                StartCoroutine(WaveOneRoutine());
                yield break;
            }
            yield return null;
        }        
    }

    private IEnumerator SpawnAsteroids()
    {
        int i = 0;
        while (i < 5)
        {
            i++;
            GameObject newObject = Instantiate(_asteroid, new Vector3(Random.Range(-5, 6), 7, 0), Quaternion.identity);
            newObject.transform.parent = _container.transform;
            yield return new WaitForSeconds(0.7f);            
        }
        yield break;
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
