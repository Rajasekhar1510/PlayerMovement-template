using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Rajasekhar
{
    public class DataPersistenceManager : MonoBehaviour
    {
        [Header("File Storage Config")]
        [SerializeField] private string fileName;

        private GameData gameData;

        private List<IDataPersistence> dataPersistenceObjects;

        private FileDataHandler dataHandler;

        public static DataPersistenceManager instance { get; private set; }

        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("Found more than one DP Manager in the scene");
            }
            instance = this;
        }

        private void Start()
        {
            this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
            this.dataPersistenceObjects = FindAllDataPersistenceObjects();
            LoadGame();
        }

        #region NEW GAME

        public void NewGame()
        {
            this.gameData = new GameData();
        }

        #endregion

        #region LOAD GAME
        public void LoadGame()
        {
            //Load any saved data from a file using the data handler.
            this.gameData = dataHandler.Load();

            if (this.gameData == null)
            {
                Debug.Log("No data was found");
                NewGame();
            }

            //Push the loaded data to all the scripts
            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.LoadData(gameData);
            }
        }

        #endregion

        #region SAVE GAME
        public void SaveGame()
        {
            //Pass the data to other scripts that can update it and save the data files using data handler.
            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.SaveData(ref gameData);
            }
            dataHandler.Save(ref gameData);
        }

        #endregion

        #region APPLICATION

        public void OnApplicationQuit()
        {
            SaveGame();
        }

        #endregion

        private List<IDataPersistence> FindAllDataPersistenceObjects()
        {
            IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

            return new List<IDataPersistence>(dataPersistenceObjects);

        }

    }

}
