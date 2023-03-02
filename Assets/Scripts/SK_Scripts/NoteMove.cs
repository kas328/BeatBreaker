using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteMove : MonoBehaviour
{
    public float speed = 5.0f;


    // Update is called once per frame
    void Update()
    {
        transform.position += -Vector3.forward * speed * Time.deltaTime;

        if (!GameStateChanger.Instance.CurrentGameState.Equals(GameState.GameOn))
        {
            ObjPoolingManager.instance.ObstacleSetQueue_Left(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Destroy(gameObject);
            ObjPoolingManager.instance.NoteSetQueue_Right(gameObject);
        }
    }
}
