using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public GameObject[] treePrefabs;
    public int count = 10;
    public Vector3 bounds;
    public LayerMask groundMask;

    
    void Start()
    {
        for (int i = 0; i < count; i++)
        {
            var prefab = treePrefabs[Random.Range(0, treePrefabs.Length)];
            var pos = new Vector3();
            pos.x = Random.Range(0, bounds.x);
            pos.z = Random.Range(0, bounds.z);
            pos.y = bounds.y + 10;


            if (Physics.Raycast(pos, Vector3.down, out var hit, groundMask))
            {
                Instantiate(prefab, hit.point, Quaternion.identity, transform);
            }

        }
    }

   
}
