using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rajasekhar
{
    [System.Serializable]
    public class GameData
    {
        [Header("PLAYER STATS & EXTRAS")]
        public float healthCount;
        public string characterName;
        public float secondsPlayer;
        public SerializableDictionary<string, bool> cash;

        [Header("World COORDINATES")]
        public float xPosition;
        public float yPosition;
        public float zPosition;

        /*
        The values defined in this const. will be default values
        The game starts with when there is no data to Load.
         */

        public GameData()
        {
            this.healthCount = 100;
            cash = new SerializableDictionary<string, bool>();
        }
    }

}
