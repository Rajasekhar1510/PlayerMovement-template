using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rajasekhar
{
    public interface IDataPersistence
    {
        void LoadData(GameData gameData);
        void SaveData(ref GameData gameData);
    }

}
