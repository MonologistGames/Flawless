using UnityEngine;

namespace Flawless.LifeSys.LifeAttributes
{
    public abstract class LifeAttribute
    {
        public virtual float AdjustDecreaseSpeed(float animal, float planet,
            float temperature, float toxicGas, float greenHouseGas)
        {
            return 0f;
        }
    }
}