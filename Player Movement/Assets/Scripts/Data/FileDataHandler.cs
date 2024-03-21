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

        #region SETTING THE DIRECTORY DATA PATH
        public FileDataHandler(string dataDirPath, string dataFileName)
        {
            this.dataDirPath = dataDirPath;
            this.dataFileName = dataFileName;
        }

        #endregion


        #region GAMEDATA LOAD
        public GameData Load()
        {
            string fullPath = Path.Combine(dataDirPath, dataFileName);

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

        public void Save(ref GameData gameData)
        {
            string fullPath = Path.Combine(dataDirPath, dataFileName);
            try
            {
                //create the dir path if it doesnt exist
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                //serialize game data into a json string
                string dataToStore = JsonUtility.ToJson(gameData, true);

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
    }

}
