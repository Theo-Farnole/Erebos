using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    private static OptionsData _optionsData;
    readonly static string pathOptions = "options_data.gd";

    public static OptionsData OptionsData
    {
        get
        {
            if (_optionsData == null)
            {
                Load();
            }

            return _optionsData;
        }
    }

    public static void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Create(Application.persistentDataPath + "/" + pathOptions);
        bf.Serialize(file, _optionsData);

        file.Close();

        _optionsData.ApplySettings();
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/" + pathOptions))
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/" + pathOptions, FileMode.Open);
            _optionsData = (OptionsData)bf.Deserialize(file);

            file.Close();
        }
        else
        {
            _optionsData = new OptionsData();
        }

        _optionsData.ApplySettings();
    }
}