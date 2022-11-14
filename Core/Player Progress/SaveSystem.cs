using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Diagnostics;

public class SaveSystem
{
    public static void SavePlayer(PlayerDataStructure player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.fog";

        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            formatter.Serialize(stream, player);
        }
    }

    public static PlayerDataStructure LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.fog";

        if (File.Exists(path))
        {
            PlayerDataStructure data;
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                if (stream.Length == 0)
                    return null;

                data = formatter.Deserialize(stream) as PlayerDataStructure;
            }

            return data;
        }
        return null;
    }

    public static void ResetPlayerData()
    {
        string path = Application.persistentDataPath + "/player.fog";

        if (File.Exists(path))
            File.Delete(path);
    }


    public static void OpenPlayerDataFolder() => Process.Start(Application.persistentDataPath);
}
