using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [Header("Player")]
    public float speed = 5.0f;
    private Rigidbody2D playerRigidBody;
    public int jumpForce = 7;
    public bool canJump = true;
    public Animator playerAnim;
    public SpriteRenderer playersprite;
    bool isFacingRight;

    [Header("Shaking")]
    public GameObject popUp;
    public BoxCollider2D[] promptTrigger;
    public float minimumShakeAmount = 3;
    public bool stopShaking = false;
    public bool canShakeTree = false;

    [Header("Waves")]
    public TextMeshProUGUI wavesText;
    public int wave;
    public bool canSpawn = false;
    [SerializeField] TextMeshProUGUI wavesTotalText;
    int wavesTotal = 3;

    [Header("Score")]
    public TextMeshProUGUI scoreText;
    public int score;
    [SerializeField] TextMeshProUGUI scoreTotalText;
    int scoreTotal = 33;

    [Header("Health")]
    public TextMeshProUGUI healthText;
    public int health = 3;
    [SerializeField] TextMeshProUGUI healthTotalText;
    int healthTotal = 3;

    [Header("Outcome")]
    public TextMeshProUGUI outcomeText;
    public GameObject outcome;
    public GameObject grapes;
    public bool gameOver = false;
    public bool finishedGame = false;

    private TreeEvents tree;

    // Start is called before the first frame update
    void Start()
    {
        isFacingRight = false;

        playerRigidBody = GetComponent<Rigidbody2D>();
        
        //Find tree
        GameObject gTree = GameObject.FindGameObjectWithTag("Tree");

        if (gTree != null)
        {
            tree = gTree.GetComponent<TreeEvents>();
        }

        tree.leafMovement.SetBool("stop", true);

        scoreTotalText.text = scoreTotal.ToString();
        healthTotalText.text = healthTotal.ToString();
        wavesTotalText.text = wavesTotal.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = score.ToString();
        healthText.text = health.ToString();
        wavesText.text = wave.ToString();

        //Shake tree will occur
        if (canShakeTree == true)
        {
            ShakeTree(minimumShakeAmount);
        }

        //The tree will drop grapes
        if (stopShaking == true)
        {
            tree.leafMovement.SetBool("stop", true);
            TreeRumbled();
        }


        //Player lose condition
        if (score < 33 && finishedGame == true || health == 0)
        {
            ChangeMoveType(1);
            speed = 0;
            outcomeText.text = "Got all 33 grapes? because I only see fewer than 33! You'll get your money when get me all 33 grapes!";
            gameOver = true;
            outcome.SetActive(true);
            playersprite.color = new Color32(239, 98, 98, 255);
        }

        //Player win condition
        else if (score == 33)
        {
            ChangeMoveType(1);
            speed = 0;
            outcomeText.text = "My precious 33 grapes are all mine! Thank you kind sir. This shall be shared with my fellow Primordial Gang Gang!";
            gameOver = true;
            outcome.SetActive(true);
            grapes.SetActive(true);
        }

        if (gameOver == false)
        {

            if (canJump == true || playerRigidBody.velocity.y == 0)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    canJump = false;
                    playerRigidBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                    ChangeMoveType(3);
                }
            }

            //Move with A and D in game
            if (Input.GetKey(KeyCode.A))
            {
                transform.position += -transform.right * Time.deltaTime * speed;
                if (isFacingRight)
                {
                    ChangeDirection(1);
                }
                if (canJump)
                    ChangeMoveType(2);
            }

            else if (Input.GetKey(KeyCode.D))
            {
                transform.position += transform.right * Time.deltaTime * speed;
                if (!isFacingRight)
                {
                    ChangeDirection(2);
                }
                if (canJump)
                    ChangeMoveType(2);
            }

            else if(canJump)
            {
                ChangeMoveType(1);
            }

        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    //Only allow the player to jump once with the spacebar
    private void OnCollisionEnter2D(Collision2D ground)
    {
        if (ground.collider.CompareTag("Ground") || ground.collider.CompareTag("Obstacle"))
        {
            canJump = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D action)
    {
        //Allow player to shake tree
        if (action.CompareTag("Tree"))
        {
            canShakeTree = true;
            popUp.SetActive(false);
        }

        //Prompt popup message
        if (action.CompareTag("Prompt"))
        {
            popUp.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D exit)
    {
        //Stop shaking tree
        if (exit.CompareTag("Tree"))
        {
            popUp.SetActive(false);
            tree.leafMovement.SetBool("stop", true);
            canShakeTree = false;
        }

        //Remove popup message
        if (exit.CompareTag("Prompt"))
        {
            popUp.SetActive(false);
        }
    }

    //Allow player to shake tree
    public void ShakeTree(float shake)
    {
        shake = minimumShakeAmount;

        //Change shaking speeds

        if (tree.thirdWave == true)
        {
            tree.leafMovement.speed = 3.5f;
        }

        else if (tree.secondWave == true)
        {
            tree.leafMovement.speed = 3;
        }

        else if (tree.firstWave == true)
        {
            tree.leafMovement.speed = 1;
        }
        

        //Shake tree
        if (shake > 1)
        {
            tree.leafMovement.SetBool("stop", false);
            minimumShakeAmount -= Time.deltaTime;
        }

        //Stop shaking tree
        else if (shake <= 1)
        {
            canShakeTree = false;

            tree.trigger.enabled = false;
            promptTrigger[0].enabled = false;
            promptTrigger[1].enabled = false;
            stopShaking = true;

        }
    }

    //Tree will drop grapes
    public void TreeRumbled()
    {
        //Change tree to new colors
        tree.ChangeTree();

        //Spawn a wave of grapes after shaking tree
        if (tree.thirdWave == true)
        {
            wave = 3;
            canSpawn = true;
            tree.StartCoroutine("ThirdSpawnGrapes");
        }

        else if (tree.secondWave == true)
        {
            wave = 2;
            minimumShakeAmount = 8.0f;
            canSpawn = true;
            tree.StartCoroutine("SecondSpawnGrapes");
        }

        else if (tree.firstWave == true)
        {
            wave = 1;
            minimumShakeAmount = 6.0f;
            tree.StartCoroutine("FirstSpawnGrapes");
        }
        
    }

    //Different animation states of player
    enum MoveType
    {
        Idle = 1,
        Walking,
        Jumping
    }

    //Change direction of player
    enum Direction
    {
        FrontLeft = 1,
        FrontRight
    }

    //Change state of player
    public void ChangeMoveType(int moveType)
    {
        playerAnim.SetInteger("moveType", moveType);
    }

    //Change sprite orientation when facing left/right
    public void ChangeDirection(int direction)
    {
        if (direction == 2)
        {
            isFacingRight = true;
        }
        else
        {
            isFacingRight = false;
        }
        playerAnim.SetInteger("direction", direction);
    }
}
