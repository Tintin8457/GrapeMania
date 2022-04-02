using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeEvents : MonoBehaviour
{
    [Header("Events")]
    public BoxCollider2D trigger;
    public bool activateShaking = false;

    [Header("Tree Components")]
    public int scoreAmount = 1;
    public GameObject grapeShooter;

    [Header("Components")]
    public GameObject leaf;
    public Animator leafMovement;
    public GameObject bark;

    [Header("Component Colors 1")]
    public Color32 leafColor;
    public Color32 barkColor;

    [Header("Component Colors 2")]
    public Color32 leafColor2;
    public Color32 barkColor2;

    [Header("Component Colors 3")]
    public Color32 leafColor3;
    public Color32 barkColor3;

    [Header("Grape Drop Difficulty by Wave")]
    public float grapeDropWave1 = 1.7f;
    public float grapeDropWave2 = 1.63f;
    public float grapeDropWave3 = 1.54f;

    [Header("Current Waves")]
    public bool firstWave = true;
    public bool secondWave = false;
    public bool thirdWave = false;

    [Header("Last Waves")]
    public bool wave1Done = false;
    public bool wave2Done = false;
    public bool wave3Done = false;

    [Header("Grape Waves")]
    public GameObject[] wave1;
    public GameObject[] wave2;
    public GameObject[] wave3;

    private Player player;
    GameObject newGrape;

    // Start is called before the first frame update
    void Start()
    {
        //Find player
        GameObject findPlayer = GameObject.FindGameObjectWithTag("Player");

        if (findPlayer != null)
        {
            player = findPlayer.GetComponent<Player>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Wave done check
        if (wave1Done == true)
        {
            firstWave = false;
            secondWave = true;
        }

        if (wave2Done == true)
        {
            secondWave = false;
            thirdWave = true;
        }

        if (wave3Done == true)
        {
            thirdWave = false;
            player.finishedGame = true;
        }

        //Allow player to shake tree again
        if (activateShaking == true)
        {
            trigger.enabled = true;
            player.promptTrigger[0].enabled = true;
            player.promptTrigger[1].enabled = true;
            activateShaking = false;
        }
    }

    //Change the tree to different colors for each wave
    public void ChangeTree()
    {
        if (firstWave == true)
        {
            leaf.GetComponent<SpriteRenderer>().color = leafColor;
            bark.GetComponent<SpriteRenderer>().color = barkColor;
        }

        else if (secondWave == true)
        {
            leaf.GetComponent<SpriteRenderer>().color = leafColor2;
            bark.GetComponent<SpriteRenderer>().color = barkColor2;
        }

        else if (thirdWave == true)
        {
            leaf.GetComponent<SpriteRenderer>().color = leafColor3;
            bark.GetComponent<SpriteRenderer>().color = barkColor3;
        }
    }

    //Spawn grapes at certain points
    //Wave 1
    public IEnumerator FirstSpawnGrapes()
    {
        if (player.score <= 11)
        {
            grapeShooter.GetComponent<Animator>().enabled = true;

            for (int g1 = 0; g1 < wave1.Length; g1++)
            {
                if (firstWave == true && g1 < wave1.Length && wave1[g1] != null)
                {
                    if (newGrape == null)
                    {
                        newGrape = Instantiate(wave1[g1], new Vector2(grapeShooter.transform.position.x, grapeShooter.transform.position.y), Quaternion.identity);
                        yield return new WaitForSeconds(grapeDropWave1);
                        wave1[g1] = newGrape;
                    }
                }
            }
        }

        else if (player.score >= 11)
        {
            grapeShooter.GetComponent<Animator>().enabled = false;
            wave1Done = true;
            activateShaking = true;
            player.stopShaking = false;
        }

        //Disable shooter when wave is done
        for (int i = 0; i < wave1.Length; i++)
        {
            if (wave1[10] == null)
            {
                yield return new WaitForSeconds(0.4f);
                grapeShooter.GetComponent<Animator>().enabled = false;
                wave1Done = true;
                activateShaking = true;
                player.stopShaking = false;
            }
        }
    }

    //Wave 2
    public IEnumerator SecondSpawnGrapes()
    {
        if (player.score <= 22)
        {
            grapeShooter.GetComponent<Animator>().enabled = true;

            for (int g2 = 0; g2 < wave2.Length; g2++)
            {
                if (secondWave == true && g2 < wave2.Length && wave2[g2] != null)
                {
                    if (newGrape == null)
                    {
                        newGrape = Instantiate(wave2[g2], new Vector2(grapeShooter.transform.position.x, grapeShooter.transform.position.y), Quaternion.identity);
                        yield return new WaitForSeconds(grapeDropWave2);
                        wave2[g2] = newGrape;
                    }
                }
            }
        }

        //Disable shooter when wave is done
        for (int i = 0; i < wave2.Length; i++)
        {
            if (wave2[10] == null)
            {
                yield return new WaitForSeconds(0.4f);
                grapeShooter.GetComponent<Animator>().enabled = false;
                wave2Done = true;
                activateShaking = true;
                player.stopShaking = false;
                player.canSpawn = false;
            }
        }
    }

    //Wave 3
    public IEnumerator ThirdSpawnGrapes()
    {
        if (player.score <= 33)
        {
            grapeShooter.GetComponent<Animator>().enabled = true;

            for (int g3 = 0; g3 < wave3.Length; g3++)
            {
                if (thirdWave == true && g3 < wave3.Length && wave3[g3] != null)
                {
                    if (newGrape == null)
                    {
                        newGrape = Instantiate(wave3[g3], new Vector2(grapeShooter.transform.position.x, grapeShooter.transform.position.y), Quaternion.identity);
                        yield return new WaitForSeconds(grapeDropWave3);
                        wave3[g3] = newGrape;
                    }
                }
            }
        }

        //Disable shooter when wave is done
        for (int i = 0; i < wave3.Length; i++)
        {
            if (wave3[10] == null)
            {
                grapeShooter.GetComponent<Animator>().enabled = false;
                yield return new WaitForSeconds(1f);
                wave3Done = true;
                activateShaking = true;
                player.stopShaking = false;
            }
        }
    }
}
