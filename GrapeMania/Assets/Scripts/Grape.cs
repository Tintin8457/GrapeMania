using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grape : MonoBehaviour
{
    private TreeEvents tree;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        //Find player
        GameObject getTree = GameObject.FindGameObjectWithTag("Tree");

        if (getTree != null)
        {
            tree = getTree.GetComponent<TreeEvents>();
        }

        //Find player
        GameObject findPlayer = GameObject.FindGameObjectWithTag("Player");

        if (findPlayer != null)
        {
            player = findPlayer.GetComponent<Player>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Destroy when they have splat on the ground
        if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }

        //Give player max score of 3 for grapes
        else if (collision.CompareTag("Player"))
        {
            if (tree.firstWave == true && player.score == 11)
            {
                collision.gameObject.GetComponent<Player>().score += 0;
            }

            else if (tree.firstWave == true && player.score == 22)
            {
                collision.gameObject.GetComponent<Player>().score += 0;
            }

            else
            {
                collision.gameObject.GetComponent<Player>().score += tree.scoreAmount;
            }

            Destroy(gameObject);
        }
    }
}
