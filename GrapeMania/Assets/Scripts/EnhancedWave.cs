using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnhancedWave : MonoBehaviour
{
    public GameObject obstacle;
    public GameObject enemy;

    private Player player;
    private TreeEvents tree;

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
        //Spawn roaming obstacle at wave 2
        if (tree.wave2Done == false && player.wave == 2 && player.canSpawn == true)
        {
            Instantiate(obstacle, new Vector2(gameObject.transform.position.x - 0.7f, gameObject.transform.position.y - 0.353f), Quaternion.identity);
            Instantiate(obstacle, new Vector2(gameObject.transform.position.x - 7, gameObject.transform.position.y - 0.353f), Quaternion.identity);
            player.canSpawn = false;
        }

        //Spawn enemy at wave 3
        if (tree.wave3Done == false && player.wave == 3 && player.canSpawn == true)
        {
            Instantiate(enemy, new Vector2(gameObject.transform.position.x - 5, gameObject.transform.position.y), Quaternion.identity);
            player.canSpawn = false;
        }
    }
}
