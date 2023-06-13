using Monologist.Patterns.Singleton;
using UnityEngine;

namespace Flawless
{
    public class PlayerDataManager : SingletonPersistent<PlayerDataManager>
    {
        public int LifeUnitsCount = 1;
        public float LifeAmount;

        public void ReadData()
        {
            var playerLife = FindObjectOfType<LifeSys.PlayerLife>();
            Debug.Log(playerLife.GetInstanceID());
            LifeUnitsCount = playerLife.LifeUnitsCount;
            LifeAmount = playerLife.LifeAmount;
            Debug.Log("read: " + LifeUnitsCount);
            Debug.Log("read: " + LifeAmount);
        }
        
        public void WriteData()
        {
            var playerLife = FindObjectOfType<LifeSys.PlayerLife>();
            Debug.Log(playerLife.GetInstanceID());
            playerLife.LifeUnitsCount = LifeUnitsCount;
            playerLife.LifeAmount = LifeAmount;
            Debug.Log("write: " + LifeUnitsCount);
            Debug.Log("write: " + LifeAmount);
        }
    }
}