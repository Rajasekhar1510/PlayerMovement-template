using Rajasekhar;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

namespace Rajasekhar
{
    public class SaveSlot : MonoBehaviour
    {
        [Header("PROFILE")]
        [SerializeField] private string profileId = "";

        [Header("Content")]
        [SerializeField] private GameObject noDataContent;
        [SerializeField] private GameObject hasDataContent;
        [SerializeField] private TextMeshProUGUI deathCountText;

        private Button saveSlotButton;
        private void Awake()
        {
            saveSlotButton = this.GetComponent<Button>();
        }

        public void SetData(GameData data)
        {
            if (data == null)
            {
                //  hasData = false;
                noDataContent.SetActive(true);
                hasDataContent.SetActive(false);
                //clearButton.gameObject.SetActive(false);
            }
            // there is data for this profileId
            else
            {
                //hasData = true;
                noDataContent.SetActive(false);
                hasDataContent.SetActive(true);
                // clearButton.gameObject.SetActive(true);

                deathCountText.text = "HEALTH COUNT: " + data.healthCount;
            }
        }

        public string GetProfileId()
        {
            return this.profileId;
        }

         public void SetInteractable(bool interactable)
         {
             saveSlotButton.interactable = interactable;
             //clearButton.interactable = interactable;
         }

    }



}
