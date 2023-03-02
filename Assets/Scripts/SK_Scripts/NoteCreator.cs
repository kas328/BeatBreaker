using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;


public struct NoteMake
{
    public float currentTime;
    public int number;

    public NoteMake(float time, int num)
    {
        currentTime = time;
        number = num;
    }

    public override string ToString()
    {
        return currentTime + "," + number;
    }
}

public class NoteCreator : MonoBehaviour
{
    AudioSource source;
    bool musicPlaying = false;
    float currentTime;

    List<NoteMake> noteList = new List<NoteMake>();

    public static NoteCreator Instance;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            musicPlaying = true;
        }

        if (musicPlaying)
        {
            NoteCreate();
        }

        if(Input.GetButtonDown("Fire2"))
        {
            musicPlaying = false;
            SaveDataToFile();
        }
    }

    void NoteCreate()
    {
        currentTime += Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            noteList.Add(new NoteMake(currentTime, 1));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            noteList.Add(new NoteMake(currentTime, 2));
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            noteList.Add(new NoteMake(currentTime, 3));
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            noteList.Add(new NoteMake(currentTime, 4));
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            noteList.Add(new NoteMake(currentTime, 5));
        }
    }

    void SaveDataToFile()
    {
        StringBuilder sb = new StringBuilder();
        foreach(NoteMake note in noteList)
        {
            print(note.ToString());
            sb.AppendLine(note.ToString());
        }
        string path = Application.dataPath + "/NoteFile.csv";
        print(path);
        File.WriteAllText(path, sb.ToString());
    }

    public List<NoteMake> GetNotes()
    {
        List<NoteMake> noteList = new List<NoteMake>();
        TextAsset data = Resources.Load("NoteFile(I'm note that girl)", typeof(TextAsset)) as TextAsset;
        StringReader sr = new StringReader(data.text);

        string source = sr.ReadLine();
        string[] note;

        while (source != null)
        {
            note = source.Split(',');
            if (note.Length == 0) 
            {
                sr.Close();
                break;
            }
            else
            {
                float time = float.Parse(note[0]);
                int number = int.Parse(note[1]);
                NoteMake m_note = new NoteMake(time, number);
                noteList.Add(m_note);

                source = sr.ReadLine();
                //print(source);
            }
        }

        return noteList;
    }
}
