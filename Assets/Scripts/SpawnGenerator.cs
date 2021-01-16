using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGenerator : MonoBehaviour
{
    public GameObject[] propPrefabs;
    public int count = 100;

    private List<GameObject> props = new List<GameObject>();
    private BoxCollider area;

    private void Start()
    {
        area = GetComponent<BoxCollider>();

        for(int i=0; i<count; i++)
        {
            Spawn();
        }

        area.enabled = false;
    }

    private void Spawn()
    {
        int selection = Random.Range(0, propPrefabs.Length);
        GameObject selectedPrefab = propPrefabs[selection];
        Vector3 spawnPos = GetRandomPostion();

        GameObject instance = Instantiate(selectedPrefab, spawnPos, Quaternion.identity);
        props.Add(instance);
    }

    private Vector3 GetRandomPostion()
    {
        Vector3 basePostion = transform.position;
        Vector3 size = area.size;

        float posX = basePostion.x + Random.Range(-size.x / 2f, size.x / 2f);
        float posY = basePostion.y + Random.Range(-size.y / 2f, size.y / 2f);
        float posZ = basePostion.z + Random.Range(-size.z / 2f, size.z / 2f);

        Vector3 spawnPos = new Vector3(posX, posY, posZ);

        return spawnPos;
    }

    public void ResetProp()
    {
        for(int i=0; i<props.Count; i++)
        {
            props[i].transform.position = GetRandomPostion();
            props[i].SetActive(true);
        }
    }
}
