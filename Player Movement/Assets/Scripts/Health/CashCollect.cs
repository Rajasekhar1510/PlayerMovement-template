using Rajasekhar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rajasekhar
{
    public class CashCollect : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            PlayerStatManager playerStatManager = other.GetComponent<PlayerStatManager>();

            if (playerStatManager != null)
            {
                playerStatManager.CashCollected();
                gameObject.SetActive(false);
            }
        }
    }
}

