using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniNote : MonoBehaviour
{
    //MiniNote의 좌표를 랜덤으로 NotePosition의 좌표로 변경하고 싶다.
    //필요속성 : NotePosition의 좌표
    // Start is called before the first frame update
    void Start()
    {
        //Note 자식의 notePosition1~5 중 하나를 랜덤으로 찾아 활성화 시키고 싶다. 
        Transform notePosition = transform.Find("NotePosition" + Random.Range(1, 6));
        notePosition.gameObject.SetActive(true);
        transform.position = notePosition.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        /*for (int i = 0; i < list.Count - 4; i++)
        {
            miniNote.transform.position = Random.Range(list[0], )
        }*/
    }

}
