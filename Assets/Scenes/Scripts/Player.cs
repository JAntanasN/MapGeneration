using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject cursor;
    public Transform cursor2;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        cursor2.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && agent.isActiveAndEnabled)
        {   
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out var hit))
            {
                cursor2.position = hit.point;
                cursor2.gameObject.SetActive(true);
                agent.SetDestination(hit.point);
            }

        }

        if(cursor.gameObject.activeSelf && agent.remainingDistance < 0.1f)
        {
            cursor.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        agent.enabled = true;
    }
}
