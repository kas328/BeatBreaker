using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using UnityEngine;
public struct NoteMaker
{
    public float makeTime;
    public int createPos;

    public NoteMaker(float time, int pos)
    {
        makeTime = time;
        createPos = pos;
    }
}

public class CSVReader1 : MonoBehaviour
{
    void Start()
    {
        TextAsset data = Resources.Load("NoteFile", typeof(TextAsset)) as TextAsset;
        StringReader sr = new StringReader(data.text);

        string source = sr.ReadLine();
        string[] note;

        while (source != null)
        {
            note = source.Split(',');
            if (note.Length == 0)
            {
                sr.Close();
                return;
            }
            else
            {
                source = sr.ReadLine();
                print(source);
            }
        }
    }

    void MakeNote()
    {

    }
    /*
    void MakeNote()
    {
        //currentTime이 note 사이의 간격을 지나갈때마다 노트가 생성되어야 한다.
        if (currentTime >= noteMakers.Peek().makeTime)
        {
            GameObject note = ObjPoolingManager.instance.GetQueue();
            note.transform.position = transform.position;

            // note.createPos에 생성
            //note.transform.position = noteCreatePos[NoteMaker.createPos].transform.position;
            //createNumber에 대한 정의가 필요
        }

    }
    */

}
