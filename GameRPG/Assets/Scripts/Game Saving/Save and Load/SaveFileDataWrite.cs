using System;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using UnityEngine;
namespace TV
{
    public class SaveFileDataWirter 
    {
        public string saveDataDirectoryPath = "";
        public string saveFileName = "";
        // must check to see if one of this character slot already exist(4 players)
        public bool CheckToSeeFileExits()
        {
            if (File.Exists(Path.Combine(saveDataDirectoryPath, saveFileName)))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public void DeleteSaveFile()
        {
            File.Delete(Path.Combine(saveDataDirectoryPath, saveFileName));
        }
       // using when starting new game
        public void CreateNewCharacterSaveFile(CharacterSaveData characterSaveData)
        {
            string savePath = Path.Combine(saveDataDirectoryPath, saveFileName);
            try
            {
                //Create a Directory if it does not exist
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                Debug.Log("Directory Created"+ savePath);

                // Serialize the characterSaveData object to JSON
                string datatoStore = JsonUtility.ToJson(characterSaveData, true);

                // wirte a file in system
                using (FileStream stream = new FileStream(savePath, FileMode.Create))
                {
                    using (StreamWriter filewriter = new StreamWriter(stream))
                    {
                        filewriter.Write(datatoStore);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Error creating save file: " + savePath+ "\n"+ex);
            }

        }

        public CharacterSaveData LoadSaveFile()
        {
            // return null if heave error
            CharacterSaveData characterSaveData = null;
            string loadPath = Path.Combine(saveDataDirectoryPath, saveFileName);
            if (File.Exists(loadPath)) { 
                try
                {
                    string dataToLoad = "";
                    // open file
                    using (FileStream steam = new FileStream(loadPath, FileMode.Open))
                    {
                        //read stream
                        using (StreamReader reader = new StreamReader(steam))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }
                    // Deserialize the JSON data back into a CharacterSaveData objects
                    characterSaveData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);
                }
                catch (Exception ex)
                {
                    Debug.LogError("Error loading save file: " + loadPath + "\n" + ex);
                }


            }
          return characterSaveData;
        } 
    }
}
