using Rajasekhar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Rajasekhar
{
    public class MainMenu : MonoBehaviour
    {
        [Header("buttons")]
        [SerializeField]private Button newGameButton;
        [SerializeField]private Button continueGameButton;

        private void Start()
        {
            if (!DataPersistenceManager.instance.HasGameData())
            {
                continueGameButton.interactable = false;
            }
        }
        public void OnNewGameClicked()
        {
            DisableAllButtons();
            DataPersistenceManager.instance.NewGame();

            SceneManager.LoadSceneAsync("Movement");
        }

        public void OnContinueGameClicked()
        {
            DisableAllButtons();
            SceneManager.LoadSceneAsync("Movement");
        }

        private void DisableAllButtons()
        {
            newGameButton.interactable = false;
            continueGameButton.interactable = false;    
        }

    }
}

