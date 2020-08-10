using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MovingObject
{
    public int WallDamage = 1;
    public int PointsPerFood = 10;
    public int PointsPerSoda = 20;
    public int PointsPerBerry = 50;
    public float RestartLevelDelay = 1f;
    public Text FoodText;
    public AudioClip MoveSound1;
    public AudioClip MoveSound2;
    public AudioClip EatSound1;
    public AudioClip EatSound2;
    public AudioClip DrinkSound1;
    public AudioClip DrinkSound2;
    public AudioClip GameOverSound;

    private Animator _animator;
    private int _food;

    // Start is called before the first frame update
    protected override void Start()
    {
        _animator = GetComponent<Animator>();
        _food = GameManager.instance.PlayerFoodPoints;

        FoodText.text = "Food " + _food;

        base.Start();
    }

    private void OnDisable()
    {
        GameManager.instance.PlayerFoodPoints = _food;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.PlayersTurn)
        {
            return;
        }

        int horizontal;
        int vertical;

        horizontal = (int) Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
        {
            vertical = 0;
        }

        if (horizontal !=0 || vertical != 0)
        {
            AttemptMove<Wall>(horizontal, vertical);
        }

        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            gameObject.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        _food--;
        FoodText.text = "Food " + _food;

        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit;
        if(Move (xDir, yDir, out hit))
        {
            SoundManager.Instance.RanomizeSfx(MoveSound1, MoveSound2);
        }

        CheckIfGameOver();

        GameManager.instance.PlayersTurn = false;
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "Exit")
        {
            Invoke("Restart", RestartLevelDelay);
            enabled = false;
        }
        else if (other.tag == "Food")
        {
            _food += PointsPerFood;
            FoodText.text = "+" + PointsPerFood + "Food: " + _food;
            SoundManager.Instance.RanomizeSfx(EatSound1, EatSound2);
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Soda")
        {
            _food += PointsPerSoda;
            FoodText.text = "+" + PointsPerSoda + "Food: " + _food;
            SoundManager.Instance.RanomizeSfx(DrinkSound1, DrinkSound2);
            other.gameObject.SetActive(false);
        }

        else if (other.tag == "Berry")
        {
            _food += PointsPerBerry;
            FoodText.text = "+" + PointsPerBerry + "Food: " + _food;
            SoundManager.Instance.RanomizeSfx(EatSound1, EatSound2);
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Ghost")
        {
            LoseFood(50);
        }
    }

    protected override void OnCantMove<T>(T component)
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(WallDamage);
        _animator.SetTrigger("PlayerChop");
    }

    private void Restart()
    {
        SceneManager.LoadScene (0);
    }

    public void LoseFood (int loss)
    {
        _animator.SetTrigger("PlayerHit");
        _food -= loss;
        FoodText.text = "-" + loss + "Food " + _food;
        CheckIfGameOver();
    }

    private void CheckIfGameOver()
    {
        if (_food <= 0)
        {
            SoundManager.Instance.PlaySingle(GameOverSound);
            SoundManager.Instance.MusicSource.Stop();
            GameManager.instance.GameOver();
        }

    }


    protected override void EnemyHitWall<T>(T component)
    {

    }
}
