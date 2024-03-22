using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace Rajasekhar
{
    //This class  will save the files and write the data
    public class FileDataHandler
    {
        [Header("STRING VARIABLES")]
        private string dataDirPath = "";
        private string dataFileName = "";

        private bool useEncryption = false;
        private readonly string encryptionCodeWord = "word";

        #region SETTING THE DIRECTORY DATA PATH
        public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
        {
            this.dataDirPath = dataDirPath;
            this.dataFileName = dataFileName;
            this.useEncryption = useEncryption;
        }

        #endregion


        #region GAMEDATA LOAD
        public GameData Load(string profileId)
        {
            if (profileId == null)
            {
                return null;
            }

            string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);

            GameData loadedData = null;

            if (File.Exists(fullPath))
            {
                try
                {
                    string dataToLoad = "";
                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    //OPTIONALLY DECRYPT THE FILE
                    if (useEncryption)
                    {
                        dataToLoad = EncryptDecrypt(dataToLoad);
                    }

                    //deserialize the data
                    loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
                }
                catch (Exception e)
                {
                    Debug.LogError("Error occured when trying to load data from file" + fullPath + "\n" + e);
                }
            }
            return loadedData;
        }
        #endregion


        #region GAMEDATA SAVE

        public void Save(ref GameData gameData, string profileId)
        {
            if (profileId == null)
            {
                return;
            }
            string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
            try
            {
                //create the dir path if it doesnt exist
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                //serialize game data into a json string
                string dataToStore = JsonUtility.ToJson(gameData, true);

                //OPTIONALLY ENCRYPT THE DATA
                if (useEncryption)
                {
                    dataToStore = EncryptDecrypt(dataToStore);
                }

                //write the serialized data to the file
                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured to save to file" + fullPath + "\n" + e);
            }
        }
        #endregion

        public Dictionary<string, GameData> LoadAllProfiles()
        {
            Dictionary<string, GameData> profileDictionary = new Dictionary<string, GameData>();
            //loop over all directory names
            IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();
            foreach (DirectoryInfo dirInfo in dirInfos)
            {
                string profileId = dirInfo.Name;
                string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
                if (!File.Exists(fullPath))
                {
                    Debug.LogWarning("Skipping dictionary when loading all profiles" + profileId);
                    continue;
                }

                GameData profileData = Load(profileId);

                if (profileData != null)
                {
                    profileDictionary.Add(profileId, profileData);
                }
                else
                {
                    Debug.LogError("Tried loading but something went wrong" + profileId);
                }

            }
            return profileDictionary;
        }

        public string GetMostRecentlyUpdatedProfileId()
        {
            string mostRecentProfileId = null;  

            Dictionary<string, GameData> profilesGameData = LoadAllProfiles();
            foreach(KeyValuePair<string, GameData> pair in profilesGameData)
            {
                string profileId = pair.Key;
                GameData gameData = pair.Value;

                if (gameData == null)
                {
                    continue;
                }

                if (mostRecentProfileId == null)
                {
                    mostRecentProfileId = profileId;
                }
                else
                {
                    DateTime mostRecentDateTime = DateTime.FromBinary(profilesGameData[mostRecentProfileId].lastUpdated);
                    DateTime newDateTime = DateTime.FromBinary(gameData.lastUpdated);

                    if(newDateTime > mostRecentDateTime)
                    {
                        mostRecentProfileId = profileId;
                    }
                }

            }

            return mostRecentProfileId;
        }

        //THE BELOW IS A SIMPLE IMPLEMENTATION OF XOR ENCRYPTION
        #region ENCRYPTION AND DECRYPTION BY XOR
        private string EncryptDecrypt(string data)
        {
            string modifiedData = "";
            for (int i = 0; i < data.Length; i++)
            {
                modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
            }
            return modifiedData;
        }
        #endregion
    }

}
