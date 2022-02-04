using MemoryGame.Data;
using Newtonsoft.Json;
using UnityEngine;

namespace MemoryGame.Controllers
{
    public class SaveController
    {
        private static string saveDataPath = "/SaveData.json";

        public static SaveData LoadFromJson()
        {
            if (!System.IO.File.Exists(Application.persistentDataPath + saveDataPath))
            { 
                return null;
            }

            string stringData = System.IO.File.ReadAllText(Application.persistentDataPath + saveDataPath);
            return JsonConvert.DeserializeObject<SaveData>(stringData);
        }

        public static void SaveIntoJson(SaveData data)
        {
            string stringData = JsonConvert.SerializeObject(data);
            System.IO.File.WriteAllText(Application.persistentDataPath + saveDataPath, stringData);
        }
    }
}
