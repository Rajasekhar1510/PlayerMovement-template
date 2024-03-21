using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rajasekhar
{
    public class Damage : MonoBehaviour
    {
        public PlayerStatManager playerStatManager;

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                playerStatManager.TakeDamage(25f);
               
            }


        }
    }

}
