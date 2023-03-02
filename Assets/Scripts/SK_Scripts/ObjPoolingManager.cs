using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPoolingManager : MonoBehaviour
{
    public GameObject noteFactory1;
    public GameObject noteFactory2;
    public GameObject obstacleFactory1;
    public GameObject obstacleFactory2;
    public Queue<GameObject> left_queue = new Queue<GameObject>();
    public Queue<GameObject> right_queue = new Queue<GameObject>();
    public Queue<GameObject> left_obs_queue = new Queue<GameObject>();
    public Queue<GameObject> right_obs_queue = new Queue<GameObject>();
    public Queue<GameObject> middle_obs_queue = new Queue<GameObject>();
    public static ObjPoolingManager instance = null;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        for (int i = 0; i < 100; i++)
        {
            GameObject note1 = Instantiate(noteFactory1);
            left_queue.Enqueue(note1);
            note1.SetActive(false);
        }

        for (int i = 0; i < 100; i++)
        {
            GameObject note2 = Instantiate(noteFactory2);
            right_queue.Enqueue(note2);
            note2.SetActive(false);
        }

        for (int i = 0; i < 100; i++)
        {
            GameObject obstacle1 = Instantiate(obstacleFactory1);
            left_obs_queue.Enqueue(obstacle1);
            obstacle1.SetActive(false);
        }

        for (int i = 0; i < 100; i++)
        {
            GameObject obstacle2 = Instantiate(obstacleFactory2);
            right_obs_queue.Enqueue(obstacle2);
            obstacle2.SetActive(false);
        }

        for (int i = 0; i < 100; i++)
        {
            GameObject obstacle3 = Instantiate(obstacleFactory2);
            middle_obs_queue.Enqueue(obstacle3);
            obstacle3.SetActive(false);
        }
    }
    //Note GetSet
    public void NoteSetQueue_Left(GameObject a_note)
    {
        left_queue.Enqueue(a_note);
        AddNoteRigidBody(a_note);
    }

    public void NoteSetQueue_Right(GameObject a_note)
    {
        right_queue.Enqueue(a_note);
        AddNoteRigidBody(a_note);
    }

    public GameObject NoteGetQueue_Left()
    {
        GameObject note = left_queue.Dequeue();
        note.SetActive(true);
        return note;
    }
    public GameObject NoteGetQueue_Right()
    {
        GameObject note = right_queue.Dequeue();
        note.SetActive(true);
        return note;
    }

    //Obstacle GetSet
    public void ObstacleSetQueue_Left(GameObject a_obstacle)
    {
        left_obs_queue.Enqueue(a_obstacle);
        AddObstacleRigidBody(a_obstacle);
    }

    public void ObstacleSetQueue_Right(GameObject a_obstacle)
    {
        right_obs_queue.Enqueue(a_obstacle);
        AddObstacleRigidBody(a_obstacle);
    }
    public void ObstacleSetQueue_middle(GameObject a_obstacle)
    {
        middle_obs_queue.Enqueue(a_obstacle);
        AddObstacleRigidBody(a_obstacle);
    }

    public GameObject ObstacleGetQueue_Left()
    {
        GameObject obstacle = left_obs_queue.Dequeue();
        obstacle.SetActive(true);
        return obstacle;
    }
    public GameObject ObstacleGetQueue_Right()
    {
        GameObject obstacle = right_obs_queue.Dequeue();
        obstacle.SetActive(true);
        return obstacle;
    }
    public GameObject ObstacleGetQueue_middle()
    {
        GameObject obstacle = middle_obs_queue.Dequeue();
        obstacle.SetActive(true);
        return obstacle;
    }

    // DataBase

    void AddNoteRigidBody(GameObject a_note)
    {
        a_note.SetActive(false);

        Rigidbody rb = a_note.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        a_note.transform.rotation = Quaternion.identity;
    }
    void AddObstacleRigidBody(GameObject a_obstacle)
    {
        a_obstacle.SetActive(false);

        Rigidbody rb = a_obstacle.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
