using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UIManager : MonoBehaviour
{
    private int _score = 0;
    [SerializeField] private Image _heartImage;
    [SerializeField] private Image _ammoImage;
    [SerializeField] private Sprite[] _heartSprites;
    [SerializeField] private Sprite[] _ammoSprites;
    [SerializeField] private GameObject _gameOverText;
    [SerializeField] private GameObject _restartText;
    [SerializeField] private GameObject _waveText;

    [SerializeField] private GameObject _livesDisplay;
    [SerializeField] private GameObject _ammoDisplay;
    [SerializeField] private GameObject _thrustBarDisplay;
    [SerializeField] private GameObject _scoreText;
    [SerializeField] private GameObject _spawnManager;
    [SerializeField] private GameObject _player;

    [SerializeField] private GameObject _titleText;
    [SerializeField] private GameObject _instructionText;
    [SerializeField] private GameObject _startText;

    [SerializeField] private GameObject _Asteroid;

    private bool _gameover = false;
    public bool gamestarted = false;
    [SerializeField] Slider _thrustBarSlider;
    [SerializeField] private float _thrust = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKey("r") && _gameover)
        {
            SceneManager.LoadScene("Game");
        }

        if (Input.GetKey("space") && !gamestarted)
        {
            _titleText.SetActive(false);
            _instructionText.SetActive(false);
            _startText.SetActive(false);
            _waveText.SetActive(false);

            _livesDisplay.SetActive(true);
            _ammoDisplay.SetActive(true);
            _thrustBarDisplay.SetActive(true);
            _scoreText.SetActive(true);
            _player.SetActive(true);            
            gamestarted = true;
            StartCoroutine(ThrustBarRoutine());

            _spawnManager.SetActive(true);
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    private IEnumerator ThrustBarRoutine()
    {
        while (true)
        {
            _thrustBarSlider.value = (float)Math.Round(_thrust,2);
            yield return new WaitForSeconds(0.3f);
        }
        
    }

    public void SetThrust (float NewThrust)
    {
        _thrust = NewThrust;
    }

    public void AddScore(int ScoreToAdd)
    {
        _score += ScoreToAdd;
        GameObject scoreText = GameObject.Find("Score_Text");

        Text textComponent = scoreText.transform.GetComponent<Text>();
        textComponent.text = "Score " + _score;
    }

    public void UpdateHealth(int CurrentHealth)
    {
        _heartImage.sprite = _heartSprites[CurrentHealth];
    }
    
    public void UpdateAmmo(int CurrentAmmo)
    {
        _ammoImage.sprite = _ammoSprites[CurrentAmmo];
    }

    public void GameOver(bool GameOver)
    {
        if (GameOver)
        {
            _gameover = true;
            StartCoroutine(GameOverBlink());
        }    
        else
        {
            _gameover = false;
        }
    }

    private IEnumerator GameOverBlink()
    {       
        while (_gameover) 
        {
            if (_gameOverText.activeInHierarchy)
            {
               _gameOverText.SetActive(false);
               _restartText.SetActive(false);
            } else
            {
               _gameOverText.SetActive(true);
               _restartText.SetActive(true);
            }            
            yield return new WaitForSeconds(0.5f);
        }
        yield break;
    }

    public void UpdateWave(int WaveNumber) 
    {
        _waveText.GetComponent<Text>().text = "WAVE " + WaveNumber;
        StartCoroutine(ShowWaveTextRoutine());
    }

    private IEnumerator ShowWaveTextRoutine()
    {
        _waveText.SetActive(true);
        yield return new WaitForSeconds(5);
        _waveText.SetActive(false);
        yield break;
    }

}
