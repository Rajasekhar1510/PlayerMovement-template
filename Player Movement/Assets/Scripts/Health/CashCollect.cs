using Rajasekhar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rajasekhar
{
    public class CashCollect : MonoBehaviour, IDataPersistence
    {
        public bool isCollected = false;
        [SerializeField]private string id;
        [ContextMenu("Generate id")]
        private void GenerateGuid()
        {
            id = System.Guid.NewGuid().ToString();
        }

        private void OnTriggerEnter(Collider other)
        {
            PlayerStatManager playerStatManager = other.GetComponent<PlayerStatManager>();

            if (!isCollected)
            {
                if (playerStatManager != null)
                {
                    playerStatManager.CashCollected();
                    gameObject.SetActive(false);
                }
                isCollected = true;
            }
        }
        //SAVING AND LOADING PLAYERS STATS-------->>>>>>>>
        #region LOADING AND SAVING COLLECTIBLE CASH DATA
        public void LoadData(GameData gameData)
        {
            gameData.cash.TryGetValue(id, out isCollected);
            if (isCollected)
            {
                gameObject.SetActive(false);
            }
        }

        public void SaveData(ref GameData gameData)
        {
            if (gameData.cash.ContainsKey(id))
            {
                gameData.cash.Remove(id);
            }
            gameData.cash.Add(id, isCollected);
        }
        #endregion

    }
}

