using Rajasekhar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Rajasekhar
{
    public class SaveSlotsMenu : Menu
    {
        [Header("Menu Navigation")]
        [SerializeField] private MainMenu mainMenu;
        private SaveSlot[] saveSlots;

        [Header("Buttons")]
        [SerializeField] private Button backButton;

        private bool isLoadingGame = false;

        private void Awake()
        {
            saveSlots = this.GetComponentsInChildren<SaveSlot>();
        }

        public void OnSaveSlotClicked(SaveSlot saveSlot)
        {
            DisableMenuButtons();

            DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());

            if (!isLoadingGame)
            {
                DataPersistenceManager.instance.NewGame();
            }

            SceneManager.LoadSceneAsync("Movement");

        }


        public void OnBackClicked()
        {
            mainMenu.ActivateMenu();
            this.DeactivateMenu();
        }

        public void ActivateMenu(bool isLoadingGame)
        {
            this.gameObject.SetActive(true);
            this.isLoadingGame = isLoadingGame;

            // load all of the profiles that exist
            Dictionary<string, GameData> profilesGameData = DataPersistenceManager.instance.GetAllProfilesGameData();

            GameObject firstSelected = backButton.gameObject;
            foreach (SaveSlot saveSlot in saveSlots)
            {
                GameData profileData = null;
                profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
                saveSlot.SetData(profileData);
                if (profileData == null && isLoadingGame)
                {
                    saveSlot.SetInteractable(false);
                }
                else
                {
                    saveSlot.SetInteractable(true);
                    if(firstSelected.Equals(backButton.gameObject))
                    {
                        firstSelected = saveSlot.gameObject;
                    }
                }
            }

            Button firstSelectedButton = firstSelected.GetComponent<Button>();
            this.SetFirstSelected(firstSelectedButton);
        }

        public void DeactivateMenu()
        {
            this.gameObject.SetActive(false);
        }

        private void DisableMenuButtons()
        {
            foreach (SaveSlot saveSlot in saveSlots)
            {
                saveSlot.SetInteractable(false);
            }
            backButton.interactable = false;
        }
    }
}

