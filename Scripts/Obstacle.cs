using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private TreeEvents tree;

    // Start is called before the first frame update
    void Start()
    {
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
        //Remove obstacle after wave 2 is done
        if (tree.wave2Done)
        {
            Destroy(gameObject);
        }
    }
}
