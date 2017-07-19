using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPooler : MonoBehaviour {

    public int NumPlatsPerSection = 5;
    public List<GameObject> platforms;
    private Vector3 initPos;

    void Awake()
    {
        platforms = new List<GameObject>();
        initPos = Vector3.zero;

        foreach(Transform child in transform)
        {
            platforms.Add(child.gameObject);
        }

        foreach(GameObject plat in platforms)
        {
            plat.SetActive(false);
            plat.transform.position = initPos;
        }
            
        SpawnPlatforms();
    }

    void Update()
    {
        
    }

    GameObject PickRandomPlatform()
    {
        int randoNum = Random.Range(0, platforms.Count);
        GameObject platPicked = platforms[randoNum];
        platforms.RemoveAt(randoNum);

        return platPicked;

    }

    void SpawnPlatforms()
    {
        for(int i = 0; i < NumPlatsPerSection; i++)
        {
            GameObject currentPlatform = PickRandomPlatform();

            currentPlatform.SetActive(true);
            currentPlatform.transform.position += new Vector3(40 * (i + 1), 0, 0);

        }
    }

    public void AddPlatformBack(GameObject platform)
    {
        platforms.Add(platform);
    }
}
