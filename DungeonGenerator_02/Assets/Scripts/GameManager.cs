using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public BoardManager boardScript;
    public int playerFoodPoints = 100;
    public bool playersTurn = true;
    public float turnDelay = 0.1f;
    public float levelStartDelay = 2f;

    private Text levelText;
    private GameObject levelImage;
    private int level = 0;
    private List<Enemy> enemies;
    private bool enemiesMoving;
    private bool doingSetup;

	private void Awake () {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        enemies = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();
        InitGame();
	}

    private void OnLevelWasLoaded(int level) {
        Debug.Log("Level was loaded");
        InitGame();
    }

    private void Update() {
        print(level);
        if (playersTurn || enemiesMoving || doingSetup) {
            return;
        }
        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy enemy) {
        enemies.Add(enemy);
    }

    private void InitGame() {
        doingSetup = true;
        level++;
        levelImage = GameObject.Find("Level Image");
        levelText = GameObject.Find("Level Text").GetComponent<Text>();
        levelText.text = "Day " + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);
        enemies.Clear();
        boardScript.SetupScene(level);
    }

    private void HideLevelImage() {
        levelImage.SetActive(false);
        doingSetup = false;

    }

    public void GameOver() {
        enabled = false;
        levelText.text = "You survived for " + level + " days.";
        levelImage.SetActive(false);
    }

    private IEnumerator MoveEnemies() {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if (enemies.Count == 0) {
            yield return new WaitForSeconds(turnDelay);
        }
        for (int i = 0; i < enemies.Count; i++) {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }

        playersTurn = true;
        enemiesMoving = false;
    }
}
