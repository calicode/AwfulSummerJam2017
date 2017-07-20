using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPooler : MonoBehaviour {

    public int NumPlatsPerSection = 5; //Number of platforms per section
    public List<GameObject> platforms; //The big ol' list o' platforms
    private Vector3 initPos; //The platform's initial position

    void Awake()
    {
        platforms = new List<GameObject>(); //Creates a list of gameobjects for the platforms
        initPos = Vector3.zero; //Resets the platform position to 0,0,0;

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
           
        //Puts all the platforms in place
        SpawnPlatforms();
    }

    //Picks a number at random and returns the platform with that index number, then removes it from the list
    GameObject PickRandomPlatform()
    {
        int randoNum = Random.Range(0, platforms.Count);
        GameObject platPicked = platforms[randoNum];
        platforms.RemoveAt(randoNum);

        return platPicked;

    }

    //Takes the random number from the PickRandomPlatform function and spawns the platform assigned to it
    void SpawnPlatforms()
    {
        for(int i = 0; i < NumPlatsPerSection; i++)
        {
            GameObject currentPlatform = PickRandomPlatform();

            currentPlatform.SetActive(true); //Turns on the platforms
            currentPlatform.transform.position += new Vector3(40 * (i + 1), 0, 0); //Makes sure the platforms don't stack on one another

        }
    }

    //Puts the platforms back in the pool
    public void AddPlatformBack(GameObject platform)
    {
        platforms.Add(platform);
    }
}
