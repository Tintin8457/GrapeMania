using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [Header("Player")]
    public float speed = 5.0f;
    private Rigidbody2D playerRigidBody;
    public int jumpForce = 4;
    public bool canPressP = false;
    public Animator playerAnim;
    public SpriteRenderer sprite;

    [Header("Waves")]
    public TextMeshProUGUI wavesText;
    public int wave;
    public bool canSpawn;

    [Header("Score")]
    public TextMeshProUGUI scoreText;
    public int score;

    [Header("Health")]
    public TextMeshProUGUI healthText;
    public int health = 3;

    [Header("Outcome")]
    public TextMeshProUGUI outcomeText;
    public GameObject outcome;
    public GameObject grapes;
    public bool gameOver;

    private TreeEvents tree;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        
        //Find tree
        GameObject gTree = GameObject.FindGameObjectWithTag("Tree");

        if (gTree != null)
        {
            tree = gTree.GetComponent<TreeEvents>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Grapes: " + score.ToString() + "/" + "33";
        healthText.text = "Health: " + health.ToString() + "/" + "3";
        wavesText.text = "Wave: " + wave.ToString() + "/" + "3";

        //Allow player to shake tree
        ShakeTree();

        //Player lose condition
        if (tree.wave3Done && score < 33 || health == 0)
        {
            ChangeMoveType(1);
            speed = 0;
            outcomeText.text = "Got all 33 grapes? because I only see fewer than 33! You'll get your money when get me all 33 grapes!";
            gameOver = true;
            outcome.SetActive(true);
            sprite.color = new Color32(239, 98, 98, 255);
        }

        //Player win condition
        else if (tree.wave3Done && score == 33)
        {
            ChangeMoveType(1);
            speed = 0;
            outcomeText.text = "My precious 33 grapes are all mine! Thank you kind sir. This shall be shared with my fellow Primordial Gang Gang!";
            gameOver = true;
            outcome.SetActive(true);
            grapes.SetActive(true);
        }

        //Player Animations
        if (gameOver == false)
        {
            if (Input.GetKeyUp(KeyCode.A))
            {
                ChangeDirection(1);
                ChangeMoveType(1);
            }

            else if (Input.GetKeyDown(KeyCode.A))
            {
                ChangeMoveType(2);
                ChangeDirection(1);
            }

            if (Input.GetKeyUp(KeyCode.D))
            {
                ChangeDirection(2);
                ChangeMoveType(1);
            }

            else if (Input.GetKeyDown(KeyCode.D))
            {
                ChangeMoveType(2);
                ChangeDirection(2);
            }
        }
    }

    void FixedUpdate()
    {
        //Move with A and D in game
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += -transform.right * Time.deltaTime * speed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * Time.deltaTime * speed;
        }
    }

    //Only allow the player to jump once with the spacebar
    private void OnCollisionStay2D(Collision2D ground)
    {
        if (ground.collider.CompareTag("Ground"))
        {
            if (gameOver == false)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    playerRigidBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                    ChangeMoveType(3);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D action)
    {
        //Prompt popup message and allow player to shake tree
        if (action.CompareTag("Tree"))
        {
            tree.PopUp();
            canPressP = true;
        }
    }

    private void OnTriggerExit2D(Collider2D exit)
    {
        //Remove shake tree popup message and disable ability to shake tree
        if (exit.CompareTag("Tree"))
        {
            tree.RemovePopUp();
            canPressP = false;
        }
    }

    //Enable player to shake tree
    public void ShakeTree()
    {
        //Shake tree for a certain amount of times until player can get grapes
        if (canPressP == true)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                tree.minimumShakeAmount -= 1;
                tree.leaf.GetComponent<Animator>().enabled = true;

                //Change shaking speeds
                if (tree.firstWave)
                {
                    tree.leaf.GetComponent<Animator>().speed = 1;
                }

                else if (tree.secondWave)
                {
                    tree.leaf.GetComponent<Animator>().speed = 3;
                    canSpawn = true;
                }

                else if (tree.thirdWave)
                {
                    tree.leaf.GetComponent<Animator>().speed = 3.5f;
                    canSpawn = true;
                }
            }

            TreeRumbled();
        }
    }

    public void TreeRumbled()
    {
        if (tree.minimumShakeAmount == 0 && tree.enabled == true)
        {
            tree.leaf.GetComponent<Animator>().enabled = false;

            //Change tree to new colors
            tree.ChangeTree();

            //Spawn a wave of grapes after shaking tree
            if (tree.firstWave)
            {
                wave = 1;
                tree.StartCoroutine("FirstSpawnGrapes");
            }

            else if (tree.secondWave)
            {
                wave = 2;
                tree.StartCoroutine("SecondSpawnGrapes");
            }

            else if (tree.thirdWave)
            {
                wave = 3;
                tree.StartCoroutine("ThirdSpawnGrapes");
            }
        }
    }

    //Reset level
    public void Reset()
    {
        //Enable new wave
        if (tree.wave1Done && !tree.wave2Done)
        {
            tree.secondWave = true;
            tree.minimumShakeAmount = 6;
        }

        else if (tree.wave2Done && !tree.wave3Done)
        {
            tree.thirdWave = true;
            tree.minimumShakeAmount = 8;
        }

        ShakeTree();
        tree.canReset = false;
    }

    //Different animation states of player
    enum moveType
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

    public void ChangeMoveType(int moveType)
    {
        playerAnim.SetInteger("moveType", moveType);
    }

    public void ChangeDirection(int direction)
    {
        playerAnim.SetInteger("direction", direction);
    }
}
