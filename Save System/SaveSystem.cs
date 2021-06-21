using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    

    private static readonly string path = Application.persistentDataPath + "/Saved files/"; 
    public static void Save<T>(T savedItem, string key) {
        Directory.CreateDirectory(path);
        BinaryFormatter bFormatter = new BinaryFormatter();
        using (FileStream fileStream = new FileStream(path + key + ".txt", FileMode.Create)) {
            bFormatter.Serialize(fileStream, savedItem);
            Debug.Log("Progress Saved");
        }
        
    }

    public static T Load<T>(string key) {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        T data = default(T);
        using(FileStream fileStream = new FileStream(path + key + ".txt", FileMode.Open)) {
            data = (T) binaryFormatter.Deserialize(fileStream);
        }

        return data;

    }

    public  static bool FileExist(string key) {
        return File.Exists(path + key + ".txt");
    }
}
