using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPooler : MonoBehaviour {

    public int NumPlatsPerSection = 5; //Number of platforms per section
    public GameObject pitStopPlatform; //The pitstop Platform
    public List<GameObject> platforms; //The big ol' list o' platforms

    private int platformIndex; //An incrementing number for platforms
    private Vector3 lastPlatPos; //The position of the last platform spawned
    private int sectionsNum = 3; //Number of sections per level
    private Vector3 initPos; //The platform's initial position

    void Awake()
    {
        platforms = new List<GameObject>(); //Creates a list of gameobjects for the platforms
        initPos = Vector3.zero; //Resets the platform position to 0,0,0;
        lastPlatPos = Vector3.zero; //Makes sure the last platform position is initialized to 0 at the start
        platformIndex = 1; //Makes sure the platform index is set to 1 at the start

        //Adds all the child platforms to the platform list
        foreach(Transform child in transform)
        {
            platforms.Add(child.gameObject);
        }

        //Deactivates all the platforms in the list and makes sure they are set to their initial position
        foreach(GameObject plat in platforms)
        {
            plat.SetActive(false);
            plat.transform.position = initPos;
        }
           
        //Places all the platforms in the pool, making sure they are separated by pitstops
        while(sectionsNum > 0)
        {
            SpawnPlatforms();
            sectionsNum--;
        }
    }

    //Picks a number at random and returns the platform with that index number, then removes it from the list
    GameObject PickRandomPlatform()
    {
        int randoNum = Random.Range(0, platforms.Count);
        if(platforms[randoNum].tag != "PitStop") //Makes sure it doesn't add a pitstop to the randomized platforms
        {
            GameObject platPicked = platforms[randoNum];
            platforms.RemoveAt(randoNum);
            return platPicked;
        }
        else
        {
            return null; //Returns nothing if a pitstop platform is selected by the randomizer
        }
    }

    //Takes the random number from the PickRandomPlatform function and spawns the platform assigned to it
    void SpawnPlatforms()
    {
        GameObject currentPlatform;

        for(int i = 0; i < NumPlatsPerSection; i++)
        {
            currentPlatform = PickRandomPlatform();

            //If the randompicker returns nothing (aka a pitstop), it'll skip it and try again
            if(currentPlatform != null) 
            {
                currentPlatform.SetActive(true); //Turns on the platforms
                currentPlatform.transform.position += new Vector3(40 * platformIndex, 0, 0); //Makes sure the platforms don't stack on one another
                lastPlatPos = currentPlatform.transform.position; //Stores the last platform's position
                platformIndex++; //increases the platform index to make sure the next one is placed in the correct spot
            }
            else
            {
                i--;
            }
        }
        lastPlatPos += new Vector3(40, 0, 0); //Moves the position of the last platform to prevent platforms overlapping with the pitstop
        SpawnPitStop(lastPlatPos); //Spawns a pit stop after a section is placed in the game scene

    }

    //Spawns a pitstop at the position next to the last platform, then increases the platform index (The pitstop is a platform after all!)
    void SpawnPitStop(Vector3 pos)
    {
        GameObject pitStop = Instantiate(pitStopPlatform, pos, Quaternion.identity) as GameObject;
        lastPlatPos += new Vector3(40, 0, 0);
        platformIndex++;
    }

    //Puts the platforms back in the pool
    public void AddPlatformBack(GameObject platform)
    {
        platforms.Add(platform);
    }
}
