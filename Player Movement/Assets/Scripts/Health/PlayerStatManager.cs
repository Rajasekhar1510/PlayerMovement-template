using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rajasekhar
{
    public class PlayerStatManager : MonoBehaviour, IDataPersistence
    {
        [Header("Health Parameters")]
        public float health;
        public float maxHealth;

        [Header("Components")]
        public Image healthBarImage;

        [Header("References")]
        public GameObject player;

        void Awake()
        {
            health = maxHealth;
        }

        private void KillPlayer()
        {
            Destroy(player);
        }

        public void TakeDamage(float damage)
        {
            health -= damage;
        }

        void Update()
        {
            healthBarImage.fillAmount = Mathf.Clamp(health / maxHealth, 0f, 1f);

            if (health > 100)
            {
                health = 100;
            }

            if (health <= 0)
            {
                health = 0;
                KillPlayer();
            }


        }

        //SAVING AND LOADING PLAYERS HEALTH-------->>>>>>>>
        #region LOADING AND SAVING HEALTH DATA
        public void LoadData(GameData gameData)
        {
            this.health = gameData.healthCount;
            maxHealth = 100;
        }

        public void SaveData(ref GameData gameData)
        {
            gameData.healthCount = this.health;
            maxHealth = 100;
        }
        #endregion
    }
}

