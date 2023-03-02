using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform noteManager1;
    public Transform noteManager2;
    public Transform obstacleManager1;
    public Transform obstacleManager2;
    public Transform obstacleManager3;
    List<NoteMake> noteList;
    int curNoteNum = 0;
    float currentTime;
    // Start is called before the first frame update
    void Start()
    {
        noteList = NoteCreator.Instance.GetNotes();
    }

    // Update is called once per frame
    void Update()
    {
        CollisionPrevent();

        if (!GameStateChanger.Instance.CurrentGameState.Equals(GameState.GameOn)) return;
        currentTime += Time.deltaTime;
        // 2. 현재 플레이할 노트가 저장된 노트 크기 안에 있고
        //  경과시간이 노트 생성시간을 초과하면
        if(curNoteNum < noteList.Count && currentTime > noteList[curNoteNum].currentTime)
        {

            //만약 Number가 1이면 NoteManager1, Number가 2이면 NoteManager2에서 생성
            if ( noteList[curNoteNum].number == 1)
            {
                GameObject note = ObjPoolingManager.instance.NoteGetQueue_Left();
                note.transform.position = noteManager1.transform.position;
                print("b");
            }

            if ( noteList[curNoteNum].number == 2)
            {
                GameObject note = ObjPoolingManager.instance.NoteGetQueue_Right();
                note.transform.position = noteManager2.transform.position;
            }

            if (noteList[curNoteNum].number == 3)
            {
                GameObject obstacle = ObjPoolingManager.instance.ObstacleGetQueue_Left();
                obstacle.transform.position = obstacleManager1.transform.position;
            }

            if (noteList[curNoteNum].number == 4)
            {
                GameObject obstacle = ObjPoolingManager.instance.ObstacleGetQueue_Right();
                obstacle.transform.position = obstacleManager2.transform.position;
            }
            if (noteList[curNoteNum].number == 5)
            {
                GameObject obstacle = ObjPoolingManager.instance.ObstacleGetQueue_middle();
                obstacle.transform.position = obstacleManager3.transform.position;
            }

            // 다음 노트로 이동
            curNoteNum++;
        }
    }

    void CollisionPrevent()
    {
        Physics.IgnoreLayerCollision(10, 12, true);
    }
}
