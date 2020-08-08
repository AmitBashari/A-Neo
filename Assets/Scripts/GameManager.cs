using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float LevelStartDelay = 2f;
    public float TurnDelay = 0.2f;
    public static GameManager instance = null;
    public BoardManager BoardScript;
    public int PlayerFoodPoints = 100;
    [HideInInspector] public bool PlayersTurn = true;

    private Text _levelText;
    private GameObject _levelImage;
    private int _level = 1;
    private List<Enemy> _enemies;
    private bool _enemiesMoving;
    private bool _doingSetup;
    private bool _firstRun = true;

    private void Awake ()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        _enemies = new List<Enemy>();
        BoardScript = GetComponent<BoardManager>();
        InitGame();
    }

    private void OnlevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (_firstRun)
        {
            _firstRun = false;
            return;
        }
        _level++;
        InitGame();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnlevelFinishedLoading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnlevelFinishedLoading;
    }

    private void InitGame()
    {
        _doingSetup = true;
        _levelImage = GameObject.Find("LevelImage");
        _levelText = GameObject.Find("LevelText").GetComponent<Text>();
        _levelText.text = "Day " + _level;
        _levelImage.SetActive(true);
        Invoke("HideLevelImage", LevelStartDelay);

        _enemies.Clear();
        BoardScript.SetupScene(_level);
    }

    private void HideLevelImage()
    {
        _levelImage.SetActive(false);
        _doingSetup = false;
    }

    public void GameOver()
    {
        _levelText.text = "After " + _level + " days, you starved.";
        _levelImage.SetActive(true);
        enabled = false;
    }

    void Update()
    {
        if (PlayersTurn || _enemiesMoving || _doingSetup)
        {
            return;
        }

        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy script)
    {
        _enemies.Add(script);
    }

    IEnumerator MoveEnemies()
    {
        _enemiesMoving = true;

        yield return new WaitForSeconds(TurnDelay);

        if (_enemies.Count == 0)
        { 
            yield return new WaitForSeconds(TurnDelay);
        }

        for (int i = 0; i < _enemies.Count; i++)
        {
            _enemies[i].MoveEnemy();
            yield return new WaitForSeconds(_enemies[i].MoveTime);
        }

        PlayersTurn = true;
        _enemiesMoving = false;

    }
}
