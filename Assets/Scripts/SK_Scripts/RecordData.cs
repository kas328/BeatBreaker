using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

//1. 게임이 시작되고 노래가 나온 후 1번 버튼을 누르면(currentTime, Left) 저장, 2번 버튼을 누르면(currentTime, Right) 저장
//2. 저장된 값이 구글 스프레드 시트에 저장되게끔 연동
//3. 구글 스프레드 시트에 저장된 CurrentTime과 Direction 값을 유니티로 가져옴
//4. 가져온 값을 노트의 currentTime과 dir에 대입
//5. 노래가 시작된 후 currentTime이 지나면 각각의 노트가 자동으로 나오게 하고
//Left면 NoteManager1에서 Light면 NoteManager2에서 나오게 함
//6. 비트에 따라서 찍었던 노트의 시간과 노트가 날아오는 동안의 시간 값의 차를 계산해 적용
//(나중에 사고를 방지하기 위해 노래 시작 후 3초 이후부터 비트를 찍음)

public class RecordData : MonoBehaviour
{
    //public TextAsset excelText;
    string[,] sentence;
    int lineSize;
    int rowSize;

    const string URL = "https://script.google.com/macros/s/AKfycbzS6fomtCsZdz8uiAtqJt0_FucjzIqcJT88SoKMUULZgMb1Xls/exec";
    public InputField curTimeInput, dirInput;
    int curTime;
    int dir;
    
    void GetInput()
    {
        WWWForm form = new WWWForm();
        form.AddField("currentTime", "direction");
        form.AddField("curTime", curTime);
        form.AddField("dir", dir);

        StartCoroutine(Post(form));
    }

    IEnumerator Post(WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            yield return www.SendWebRequest();

            if (www.isDone)
            {
                print(www.downloadHandler.text);
            }
            else print("웹의 응답이 없습니다.");
        }
    }
    // Start is called before the first frame update
    /*public void ParseExel()
    {
        string currentText = excelText.text.Substring(0, excelText.text.Length - 1);
        string[] line = currentText.Split('\n');
        lineSize = line.Length;
        rowSize = line[0].Split('\t').Length;
          
        sentence = new string[lineSize, rowSize];

        for (int i = 0; i < lineSize; i++)
        {
            string[] row = line[i].Split('\t');
            for (int j = 0; j < rowSize; j++)
            {
                sentence[i, j] = row[j];
            }
        }
    }*/

    // Update is called once per frame
    void Update()
    {
        
    }
}
