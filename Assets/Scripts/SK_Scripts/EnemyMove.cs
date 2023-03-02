using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    //NavMeshAgent nav;
    Vector3 savePos;
    Vector3 destination;
    Vector3 direction;
    public float speed = 10.0f;
    public Transform nextPos;
    public GameObject nextPosEffect;
    
    void Start()
    {
        savePos = transform.position;
        //nav = GetComponent<NavMeshAgent>();
        MakeDestination();
    }

    void Update()
    {
        //nav.SetDestination(destination);
        CheckDistance();
        direction = destination - transform.position;
        transform.position += direction * speed * Time.deltaTime;
    }

    void CheckDistance()
    {
        if(Vector3.Distance(transform.position, destination) <= 0.1f)
        {
            MakeDestination();
        }
    }

    private void MakeDestination()
    {
        Vector3 Sqhere = Random.insideUnitSphere;
        Sqhere.Normalize();
        Sqhere *= 5;
        destination = savePos + new Vector3(Sqhere.x, Sqhere.y, Sqhere.z);
        nextPos.transform.position = destination;
        nextPosEffect.SetActive(true);
        nextPosEffect.transform.position = nextPos.transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Note")
        {
            gameObject.SetActive(false);
            StartCoroutine(Reverse());
        }
    }

    IEnumerator Reverse()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(true);
        gameObject.transform.position = savePos;
        MakeDestination();
    }
}
