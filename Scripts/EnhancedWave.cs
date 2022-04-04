using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnhancedWave : MonoBehaviour
{
    public GameObject [] obstacle;
    public GameObject enemy;

    private Player player;
    private TreeEvents tree;

    public bool spawnNext = false;

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

        obstacle[0].transform.position = new Vector3(-3.76f, gameObject.transform.position.y - 0.353f, -0.51924f);
        obstacle[1].transform.position = new Vector3(2.54f, gameObject.transform.position.y - 0.353f, -0.51924f);

        enemy.transform.position = new Vector3(gameObject.transform.position.x - 5, gameObject.transform.position.y, gameObject.transform.position.z);
    }
    
    // Update is called once per frame
    void Update()
    {
        //Spawn roaming obstacle at wave 2
        if (tree.secondWave == true && player.canSpawn == true)
        {
            obstacle[0].SetActive(true);
            obstacle[1].SetActive(true);
        }

        //Spawn enemy at wave 3
        if (player.stopShaking == true && tree.thirdWave == true && player.canSpawn == true && tree.wave3Done == false)
        {
            enemy.SetActive(true);
        }
    }
}
