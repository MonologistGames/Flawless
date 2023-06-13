using Monologist.Patterns.Singleton;

namespace Flawless
{
    public class PlayerDataManager : SingletonPersistent<PlayerDataManager>
    {
        public int LifeUnitsCount;
        public float LifeAmount;

        public void ReadData()
        {
            var playerLife = FindObjectOfType<LifeSys.PlayerLife>();
            LifeUnitsCount = playerLife.LifeUnitsCount;
            LifeAmount = playerLife.LifeAmount;
        }
        
        public void WriteData()
        {
            var playerLife = FindObjectOfType<LifeSys.PlayerLife>();
            playerLife.LifeUnitsCount = LifeUnitsCount;
            playerLife.LifeAmount = LifeAmount;
        }
    }
}