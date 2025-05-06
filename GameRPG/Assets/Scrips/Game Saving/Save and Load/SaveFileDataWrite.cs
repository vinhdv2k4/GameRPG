using System;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using UnityEngine;
namespace TV
{
    public class SaveFileDataWirte : MonoBehaviour
    {
        public string saveDataDirectionPath = "";
        public string saveDataFileName = "";
        // must check to see if one of this character slot already exist(4 players)
        public bool CheckToSeeFileExits()
        {
            if (File.Exists(Path.Combine(saveDataDirectionPath, saveDataFileName)))
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
            File.Delete(Path.Combine(saveDataDirectionPath, saveDataFileName));
        }
       // using when starting new game
        public void CreateNewCharacterSaveFile(CharacterSaveData characterSaveData)
        {
            string savePath = Path.Combine(saveDataDirectionPath, saveDataFileName);
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
            string loadPath = Path.Combine(saveDataDirectionPath, saveDataFileName);
            if (File.Exists(loadPath)) { 
                try
                {
                    string dataload = "";
                    // open file
                    using (FileStream steam = new FileStream(loadPath, FileMode.Open))
                    {
                        //read stream
                        using (StreamReader reader = new StreamReader(steam))
                        {
                            dataload = reader.ReadToEnd();
                        }
                    }
                    // Deserialize the JSON data back into a CharacterSaveData objects
                    characterSaveData = JsonUtility.FromJson<CharacterSaveData>(dataload);
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
