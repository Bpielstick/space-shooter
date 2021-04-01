using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private int _score = 0;
    [SerializeField] private Image _heartDisplay;
    [SerializeField] private Sprite[] _heartSprites;
    [SerializeField] private GameObject _gameOverText;
    [SerializeField] private GameObject _restartText;

    [SerializeField] private GameObject _livesDisplay;
    [SerializeField] private GameObject _scoreText;
    [SerializeField] private GameObject _spawnManager;
    [SerializeField] private GameObject _player;

    [SerializeField] private GameObject _titleText;
    [SerializeField] private GameObject _instructionText;
    [SerializeField] private GameObject _startText;

    [SerializeField] private GameObject _Asteroid;

    private bool _gameover = false;
    private bool _gamestarted = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown("r") && _gameover)
        {
            SceneManager.LoadScene("Game");
        }

        if (Input.GetKeyDown("space") && !_gamestarted)
        {
            _gamestarted = true;
            _titleText.SetActive(false);
            _instructionText.SetActive(false);
            _startText.SetActive(false);

            _livesDisplay.SetActive(true);
            _scoreText.SetActive(true);
            _player.SetActive(true);
        }

        if (_Asteroid == null) 
        {
            _spawnManager.SetActive(true);
        }
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
        _heartDisplay.sprite = _heartSprites[CurrentHealth];
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

}
