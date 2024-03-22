using Rajasekhar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Rajasekhar
{
    public class MainMenu : Menu
    {
        [Header("Menu Navigation")]
        [SerializeField] private SaveSlotsMenu saveSlotsMenu;

        [Header("buttons")]
        [SerializeField]private Button newGameButton;
        [SerializeField]private Button continueGameButton;
        [SerializeField]private Button loadGameButton;

        private void Start()
        {
            if (!DataPersistenceManager.instance.HasGameData())
            {
                continueGameButton.interactable = false;
                loadGameButton.interactable = false;
            }
        }
        public void OnNewGameClicked()
        {
            saveSlotsMenu.ActivateMenu(false);
            this.DeactivateMenu();
        }

        public void OnLoadGameClicked()
        {
            saveSlotsMenu.ActivateMenu(true);
            this.DeactivateMenu();
        }

        public void OnContinueGameClicked()
        {
            DisableAllButtons();
            SceneManager.LoadSceneAsync("Movement");
        }
        public void ActivateMenu()
        {
            this.gameObject.SetActive(true);
        }
        public void DeactivateMenu()
        {
            this.gameObject.SetActive(false);
        } 

        private void DisableAllButtons()
        {
            newGameButton.interactable = false;
            continueGameButton.interactable = false;    
        }



    }
}

