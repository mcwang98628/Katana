using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Sirenix.Serialization;
using UnityEngine;

public class SaveManager : TSingleton<SaveManager>
{
    private string ArchivePath = Application.persistentDataPath + "/";
    

    public void SaveArchive<T>(string fileName,T data)
    {
        var bytes = ObjectToBytes<T>(data);
        // Debug.LogError(ArchivePath+fileName);
        using (FileStream Stream = File.Create(ArchivePath+fileName))
        {
            Stream.Write(bytes, 0, bytes.Length);
        }
    }

    public T LoadArchive<T>(string fileName)
    {
        using (FileStream Stream = new FileStream(ArchivePath+fileName, FileMode.Open, FileAccess.Read))
        {
            byte[] datas = new byte[Stream.Length];
            Stream.Read(datas, 0, datas.Length);
            return BytesToObject<T>(datas);
        }
    }

    public bool IsHaveArchiveFile(string fileName)
    {
        return File.Exists(ArchivePath + fileName);
    }
    
    byte[] ObjectToBytes<T>(T obj)
    {
        string json = JsonConvert.SerializeObject(obj);
        return Encoding.UTF8.GetBytes(json);
    }

    T BytesToObject<T>(byte[] datas)
    {
        return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(datas));
    }
}
