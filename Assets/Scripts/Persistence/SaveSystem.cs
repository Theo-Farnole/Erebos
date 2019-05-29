using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static OptionsData optionsData;
    readonly static string pathOptions = "options_data.gd";


    public static void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Create(Application.persistentDataPath + "/" + pathOptions);
        bf.Serialize(file, optionsData);

        file.Close();

        optionsData.ApplySettings();
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/" + pathOptions))
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/" + pathOptions, FileMode.Open);
            optionsData = (OptionsData)bf.Deserialize(file);

            file.Close();
        }
        else
        {
            optionsData = new OptionsData();
        }

        optionsData.ApplySettings();
    }
}