using System;
using System.IO;
using System.Text;
using UnityEngine;

public static class JsonDataManager // 문의 : 수민
{
    public static void CreateFile<T>(string fileName, T data)
    {
        string path = Application.dataPath + $"/Resources/Json/{fileName}.json";
        string json = ToJson(data);

        FileStream fileStream = new FileStream(path, FileMode.Create);
        byte[] bData = Encoding.UTF8.GetBytes(json);
        fileStream.Write(bData, 0, bData.Length);
        fileStream.Close();
    }

    // Json -> <T>
    // <T> -> Json
    public static void SaveToPrefs<T>(string key, T data) //파일로 오버로드 가능
    {
        string json = ToJson(data);
        PlayerPrefs.SetString(key, json);
    }

    /// <summary>
    /// 반드시 파일 먼저 만들고 쓸 것
    /// </summary>
    /// <param name="fileName">
    /// .json 확장자 없이 쓸 것
    /// </param>
    public static void SaveToFile<T>(string fileName, T data)
    {
        string path = Application.dataPath + $"/Resources/Json/{fileName}.json";
        string json = ToJson(data);

        FileStream fileStream = new FileStream(path, FileMode.Open);
        byte[] bData = Encoding.UTF8.GetBytes(json);
        fileStream.Write(bData, 0, bData.Length);
        fileStream.Close();
    }

    public static T LoadFromPrefs<T>(string key) //파일로 오버로드 가능
    {
        string json = PlayerPrefs.GetString(key, default);
        return FromJson<T>(json);
    }

    public static T LoadFromFile<T>(string fileName)
    {
        string path = Application.dataPath + $"/Resources/Json/{fileName}.json";
        FileStream fileStream = new FileStream(path, FileMode.Open);
        byte[] bData = new byte[fileStream.Length];
        fileStream.Read(bData, 0, bData.Length);
        fileStream.Close();

        string jsonData = Encoding.UTF8.GetString(bData);
        return FromJson<T>(jsonData);
    }

    public static string ToJson<T>(T data)
    {
        try
        {
            string json = JsonUtility.ToJson(data);

            if (string.IsNullOrEmpty(json))
            {
                Debug.LogError("Serialization failed: Data is not serializable.");
                return default;
            }

            //return AesEncryption.Encrypt(json);
            return json;
        }
        catch (Exception ex)
        {
            Debug.LogError($"ToJson Error: {ex.Message}");
            return default;
        }
    }

    public static T FromJson<T>(string json)
    {
        try
        {
            if (string.IsNullOrEmpty(json))
            {
                Debug.LogError("Decryption failed: Empty JSON string.");
                return default;
            }

            //string decryptedJson = AesEncryption.Decrypt(json);

            /*if (string.IsNullOrEmpty(decryptedJson))
            {
                Debug.LogError("Decryption failed: Resulting JSON is empty.");
                return default;
            }*/

            //return JsonUtility.FromJson<T>(decryptedJson);
            return JsonUtility.FromJson<T>(json);
        }
        catch (Exception ex)
        {
            Debug.LogError($"FromJson Error: {ex.Message}");
            return default;
        }
    }
}
