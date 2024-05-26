using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public GameObject[] floorPrefabs;
    public float zSpawn = 0;
    public float floorLength = 30;
    public int numberofloors = 5;
    public Transform playerTransform;
    private List<GameObject> activefloors = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i < numberofloors; i++)
        {
            if (i==0)
                SpawnFloor(0); // to make the initial floor as 1
            else
                SpawnFloor(Random.Range(0,floorPrefabs.Length)); // Randomly deciding the floor
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(playerTransform.position.z - 35>zSpawn - (numberofloors * floorLength))
        {
            SpawnFloor(Random.Range(0, floorPrefabs.Length));
            // to delete previous floors
            DeleteFloor();
        }
    }

    public void SpawnFloor(int tileIndex)
    {
        GameObject go =Instantiate(floorPrefabs[tileIndex], transform.forward * zSpawn, transform.rotation);
        activefloors.Add(go);
        zSpawn+= floorLength;
    }
    private void DeleteFloor()
    {
        Destroy(activefloors[0]);
        activefloors.RemoveAt(0); 
    }
}
