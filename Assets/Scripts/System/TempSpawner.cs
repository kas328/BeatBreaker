using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempSpawner : MonoBehaviour
{
    [SerializeField] private float _delayTime = 3f;

    private void Start()
    {
        StartCoroutine(Spawn(Random.Range(0.5f, _delayTime)));
    }

    private IEnumerator Spawn(float time)
    {
        print(GameStateChanger.Instance.IsGameOn());
        while (GameStateChanger.Instance.IsGameOn())
        {
            yield return new WaitForSeconds(time);
            GameObject obj = ObjPoolingManager.instance.NoteGetQueue_Left();

            obj.transform.position = transform.position;
        }

    }
}
