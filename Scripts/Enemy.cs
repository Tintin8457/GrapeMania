using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 5.0f;

    private Player player;
    private TreeEvents tree;
    public Animator enemyAnim;

    // Start is called before the first frame update
    void Start()
    {
        //Find player
        GameObject findPlayer = GameObject.FindGameObjectWithTag("Player");

        if (findPlayer != null)
        {
            player = findPlayer.GetComponent<Player>();
        }

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
        transform.position += -transform.right * Time.deltaTime * speed;

        //Remove enemy after wave 3 is done
        if (tree.wave3Done)
        {
            Destroy(gameObject);
        }
    }

    //Change directions when hitting the barrier
    private void OnCollisionEnter2D(Collision2D barrier)
    {
        //Remove a life from the player
        if (barrier.gameObject.CompareTag("Player"))
        {
            player.health -= 1;
        }

        //Move right
        else if (barrier.gameObject.CompareTag("Barrier1"))
        {
            transform.rotation = new Quaternion(0f, -180f, 0f, 0f);
        }

        //Move left
        else if (barrier.gameObject.CompareTag("Barrier2"))
        {
            transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }
    }
}
