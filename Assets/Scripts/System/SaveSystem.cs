using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using System;

public class SaveSystem : Singleton<SaveSystem>
{
    #region Public Field
    public static string SAVE_FOLDER = null;
    #endregion

    #region Serialize Field
    [SerializeField] private int _rankTop = 8; 
    #endregion

    #region Properties
    public List<Tuple<string, string, float>> Records { get; set; } = null;
    #endregion

    private void Awake()
    {
        SAVE_FOLDER = Application.persistentDataPath + "/SaveFile/";
        Records = new List<Tuple<string, string, float>>();
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
        Load("Ranking");
    }

    #region Public Methods
    public void Save(string song, string name, float score)
    {
        Records.Add(new Tuple<string, string, float>(song, name, score));
        //List<Tuple<string, string, float>> rank = (from record in Records
        //                     orderby record.Item3 descending select record)
        //                     .Take(_rankTop)
        //                     .ToList<Tuple<string, string, float>>();

        var rank = (from record in Records
                    orderby record.Item3 descending
                    select record)
                    .ToList<Tuple<string, string, float>>();

        string jsonRecord = JsonConvert.SerializeObject(rank);
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jsonRecord);
        string format = Convert.ToBase64String(bytes);

        File.WriteAllText(SAVE_FOLDER + "Ranking.json", format);
    }
    
    public string Load(string fileName)
    {
        string reformat = null;
        if (File.Exists(SAVE_FOLDER + fileName + ".json"))
        {
            string jsonRecord = File.ReadAllText(SAVE_FOLDER + fileName + ".json");
            byte[] bytes = Convert.FromBase64String(jsonRecord);
            reformat = System.Text.Encoding.UTF8.GetString(bytes);

            Records = JsonConvert.DeserializeObject<List<Tuple<string, string, float>>>(reformat);
        }
        return reformat;
    }

    public List<Tuple<string, string, float>> GetRankOfSong(string song)
    {
        List<Tuple<string, string, float>> rank = new List<Tuple<string, string, float>>();
        foreach (Tuple<string, string, float> record in Records)
        {
            if (record.Item1.Equals(song))
            {
                rank.Add(record);
                if (rank.Count == _rankTop) break;
            }
        }
        return rank;
    }
    #endregion
}