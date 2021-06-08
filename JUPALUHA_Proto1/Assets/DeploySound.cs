using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeploySound : MonoBehaviour
{
    public GameObject soundPrefab;
    public float respawnTime = 1f;

    public GameObject spawnStartPoint;

    public bool ventOpen = true;

    void Start()
    {
        StartCoroutine(SoundWave());
    }

    private void FixedUpdate()
    {
        if (GameObject.Find("Vent").transform.localPosition.y >= 1)
        {
            ventOpen = true;
        }
        else
        {
            ventOpen = false;
        }
    }


    private void SpawnSound()
    {
        if (ventOpen == true)
        {
            GameObject clone = Instantiate(soundPrefab) as GameObject;
            clone.transform.position = spawnStartPoint.transform.position;
        }

    }

    IEnumerator SoundWave()
    {

        while (true)
        {
            yield return new WaitForSeconds(respawnTime);
            SpawnSound();
        }
    }

}
