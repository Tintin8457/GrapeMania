using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grape : MonoBehaviour
{
    private TreeEvents tree;

    // Start is called before the first frame update
    void Start()
    {
        GameObject getTree = GameObject.FindGameObjectWithTag("Tree");

        if (getTree != null)
        {
            tree = getTree.GetComponent<TreeEvents>();
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
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().score += tree.scoreAmount;
            Destroy(gameObject);
        }
    }
}
