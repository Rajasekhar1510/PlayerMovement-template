using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Rajasekhar
{
    public class DataPersistenceManager : MonoBehaviour
    {
        [Header("DEBUGGING")]
        [SerializeField] private bool initializeDataIfNull = false;
        [SerializeField] private bool disableDataPersistence = false;
        [SerializeField] private bool overrideSelectedProfileId = false;
        [SerializeField] private string testSelectedProfileId = "test";

        [Header("File Storage Config")]
        [SerializeField] private string fileName;
        [SerializeField] private bool useEncryption;

        private GameData gameData;

        private List<IDataPersistence> dataPersistenceObjects;

        private FileDataHandler dataHandler;

        private string selectedProfileId = "";
        public static DataPersistenceManager instance { get; private set; }

        private void Awake()
        {

            if (instance != null)
            {
                Debug.LogError("Found more than one DP Manager in the scene & new manager was destroyed");
                Destroy(this.gameObject);
                return;
            }
            instance = this;

            DontDestroyOnLoad(this.gameObject);

            if (disableDataPersistence)
            {
                Debug.LogWarning("Data Persistence is currently disabled");
            }

            this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
            this.selectedProfileId = dataHandler.GetMostRecentlyUpdatedProfileId();

            if(overrideSelectedProfileId)
            {
                this.selectedProfileId = testSelectedProfileId;
                Debug.LogWarning("override selected profile id with test id" + testSelectedProfileId);
            }
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }


        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            this.dataPersistenceObjects = FindAllDataPersistenceObjects();
            LoadGame();
        }

        public void OnSceneUnloaded(Scene scene)
        {
            SaveGame();
        }

        public void ChangeSelectedProfileId(string newProfileId)
        {
            this.selectedProfileId = newProfileId;
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
            if (disableDataPersistence)
            {
                return;
            }
            //Load any saved data from a file using the data handler, if there is none, dont.
            this.gameData = dataHandler.Load(selectedProfileId);

            //start a new game if the data is null
            if (this.gameData == null && initializeDataIfNull)
            {
                NewGame();
            }

            if (this.gameData == null)
            {
                Debug.Log("No data was found, A new GAme needs to be started");
                return;
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
            if (disableDataPersistence)
            {
                return;
            }

            //if we dont have any data to save, log a warning here.
            if (this.gameData == null)
            {
                Debug.LogWarning("no data was found, a new game needs to be started before the game is saved");
                return;
            }

            //Pass the data to other scripts that can update it and save the data files using data handler.
            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.SaveData(ref gameData);
            }

            gameData.lastUpdated = System.DateTime.Now.ToBinary();

            dataHandler.Save(ref gameData, selectedProfileId);
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

        public bool HasGameData()
        {
            return gameData != null;
        }

        public Dictionary<string, GameData> GetAllProfilesGameData()
        {
            return dataHandler.LoadAllProfiles();
        }

    }

}
