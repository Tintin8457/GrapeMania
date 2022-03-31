using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeEvents : MonoBehaviour
{
    [Header("Events")]
    public GameObject popUp;
    public BoxCollider2D trigger;
    public bool canReset = false;

    [Header("Tree Components")]
    public int minimumShakeAmount;
    public int scoreAmount;
    public GameObject grapeShooter;

    [Header("Components")]
    public GameObject leaf;
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
    public bool wave1Done;
    public bool wave2Done;
    public bool wave3Done;

    [Header("Grape Waves")]
    public GameObject[] wave1;
    public GameObject[] wave2;
    public GameObject[] wave3;

    private Player player;

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
        //Reset level
        if (canReset == true)
        {
            trigger.enabled = true;
            player.Reset();
        }
    }

    //Popup and disable message to press P to shake tree
    public void PopUp()
    {
        popUp.SetActive(true);
    }

    public void RemovePopUp()
    {
        popUp.SetActive(false);
    }

    public void ChangeTree()
    {
        //Stop player from interacting with the tree
        trigger.enabled = false;

        //Change the tree to different colors for each wave
        if (firstWave)
        {
            leaf.GetComponent<SpriteRenderer>().color = leafColor;
            bark.GetComponent<SpriteRenderer>().color = barkColor;
        }

        else if (secondWave)
        {
            leaf.GetComponent<SpriteRenderer>().color = leafColor2;
            bark.GetComponent<SpriteRenderer>().color = barkColor2;
        }

        else if (thirdWave)
        {
            leaf.GetComponent<SpriteRenderer>().color = leafColor3;
            bark.GetComponent<SpriteRenderer>().color = barkColor3;
        }
    }

    //Spawn grapes at certain points
    //Wave 1
    public IEnumerator FirstSpawnGrapes()
    {
        grapeShooter.GetComponent<Animator>().enabled = true;
        

        for (int g1 = 0; g1 < wave1.Length; g1++)
        {
            if (wave1[g1] != null)
            {
                GameObject newGrape = Instantiate(wave1[g1], new Vector2(grapeShooter.transform.position.x, grapeShooter.transform.position.y), Quaternion.identity);
                yield return new WaitForSeconds(grapeDropWave1);
                wave1[g1] = newGrape;
            }
        }

        //Disable shooter when wave is done
        for (int i = 0; i < wave1.Length; i++)
        {
            if (wave1[i] == null)
            {
                yield return new WaitForSeconds(0.4f);
                grapeShooter.GetComponent<Animator>().enabled = false;
                firstWave = false;
                wave1Done = true;
                canReset = true;
            }
        }
    }

    //Wave 2
    public IEnumerator SecondSpawnGrapes()
    {
        grapeShooter.GetComponent<Animator>().enabled = true;

        for (int g2 = 0; g2 < wave2.Length; g2++)
        {
            if (wave2[g2] != null)
            {
                GameObject newGrape = Instantiate(wave2[g2], new Vector2(grapeShooter.transform.position.x, grapeShooter.transform.position.y), Quaternion.identity);
                yield return new WaitForSeconds(grapeDropWave2);
                wave2[g2] = newGrape;
            }
        }

        //Disable shooter when wave is done
        for (int i = 0; i < wave2.Length; i++)
        {
            if (wave2[i] == null)
            {
                yield return new WaitForSeconds(0.4f);
                grapeShooter.GetComponent<Animator>().enabled = false;
                secondWave = false;
                wave2Done = true;
                canReset = true;
            }
        }
    }

    //Wave 3
    public IEnumerator ThirdSpawnGrapes()
    {
        grapeShooter.GetComponent<Animator>().enabled = true;

        for (int g3 = 0; g3 < wave3.Length; g3++)
        {
            if (wave3[g3] != null)
            {
                GameObject newGrape = Instantiate(wave3[g3], new Vector2(grapeShooter.transform.position.x, grapeShooter.transform.position.y), Quaternion.identity);
                yield return new WaitForSeconds(grapeDropWave3);
                wave3[g3] = newGrape;
            }
        }

        //Disable shooter when wave is done
        for (int i = 0; i < wave3.Length; i++)
        {
            if (wave3[i] == null)
            {
                grapeShooter.GetComponent<Animator>().enabled = false;
                thirdWave = false;
                yield return new WaitForSeconds(1f);
                wave3Done = true;
            }
        }
    }
}
