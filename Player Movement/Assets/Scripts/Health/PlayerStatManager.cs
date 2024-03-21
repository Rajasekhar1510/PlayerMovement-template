using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Rajasekhar
{
    public class PlayerStatManager : MonoBehaviour, IDataPersistence
    {
        [Header("Parameters")]
        public float health;
        public float maxHealth;
        public int CashAmt {  get; private set; }

        [Header("Components")]
        public Image healthBarImage;
        public TextMeshProUGUI cashText;
        public TextMeshProUGUI healthText;

        [Header("References")]
        public GameObject player;

        void Awake()
        {
            health = maxHealth;
        }

        #region TAKING DAMAGE, KILLING PLAYER AND INCREASING HEALTH
        private void KillPlayer()
        {
            Destroy(player);
        }

        public void TakeDamage(float damage)
        {
            health -= damage;
        }
        public void IncreaseHealth(float increaseHealth)
        {
            health += increaseHealth;
        }
        #endregion

        #region CASH REFERENCE
        public void CashCollected()
        {
            CashAmt++;
            updateCashText();
        }

        public void updateCashText()
        {
            cashText.text = "CASH:" + CashAmt.ToString();
        }
        #endregion
        void Update()
        {
            #region UPDATING THE HEALTH VALUES
            healthBarImage.fillAmount = Mathf.Clamp(health / maxHealth, 0f, 1f);
            healthText.text = "HEALTH:" + health.ToString(); 


            if (health > 100)
            {
                health = 100;
            }

            if (health <= 0)
            {
                health = 0;
                KillPlayer();
            }
            #endregion
        }

        //SAVING AND LOADING PLAYERS STATS-------->>>>>>>>
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

