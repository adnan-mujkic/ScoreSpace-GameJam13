using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class OptionsManager: MonoBehaviour
{
   public static OptionsSave SaveFile;
   public static string SavePath;

   private void Awake() {
      SaveFile = new OptionsSave();
      SavePath = Path.Combine(Application.persistentDataPath, "Save.data");
   }
   public static void SaveGame() {
      FileStream dataStream = new FileStream(SavePath, FileMode.Create);
      BinaryFormatter converter = new BinaryFormatter();
      converter.Serialize(dataStream, SaveFile);
      dataStream.Close();
   }
   public static void LoadGame() {
      if(File.Exists(SavePath)) {
         FileStream dataStream = new FileStream(SavePath, FileMode.Open);
         BinaryFormatter converter = new BinaryFormatter();
         SaveFile = converter.Deserialize(dataStream) as OptionsSave;
         dataStream.Close();
      } else {
         SaveGame();
      }
   }
}
[System.Serializable]
public class OptionsSave
{
   public bool FirstTime;
   public int Wave;
   public void AddWave() {
      Wave++;
   }
   public void Reset() {
      Wave = 0;
   }
}
