using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MovingObject {

    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;
    public Text foodText;

    private Animator animator;
    private int food;

	// Use this for initialization
	protected override void Start () {
        animator = GetComponent<Animator>();

        food = GameManager.instance.playerFoodPoints;
        foodText.text = "Food: " + food;

        base.Start();
	}

    private void OnDisable() {
        GameManager.instance.playerFoodPoints = food;
    }

    // Update is called once per frame
    private void Update () {
        if (!GameManager.instance.playersTurn) return;

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0) {
            vertical = 0;
        }

        if (horizontal != 0 || vertical != 0) {
            AttemptMove<Wall>(horizontal, vertical);
        }
	}

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Exit") {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (collision.tag == "Food") {
            food += pointsPerFood;
            foodText.text = "+" + pointsPerFood + " Food: " + food;
            collision.gameObject.SetActive(false);
        }
        else if (collision.tag == "Soda") {
            food += pointsPerSoda;
            foodText.text = "+" + pointsPerSoda + " Food: " + food;
            collision.gameObject.SetActive(false);
        }
    }

    public void Restart() {
        SceneManager.LoadScene("Main");
    }

    public void LoseFood (int loss) {
        animator.SetTrigger("playerHit");
        food -= loss;
        foodText.text = "-" + loss + " Food: " + food;
        CheckIfGameOver();
    }

    void CheckIfGameOver() {
        if (food <= 0) {
            GameManager.instance.GameOver();
        }
    }

    protected override void AttemptMove<T>(int xDir, int yDir) {
        food--;
        foodText.text = "Food: " + food;

        base.AttemptMove<T>(xDir, yDir);
        RaycastHit2D hit;
        CheckIfGameOver();
        GameManager.instance.playersTurn = false;
    }

    protected override void OnCantMove<T>(T component) {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);
        animator.SetTrigger("playerChop");
    }
}
