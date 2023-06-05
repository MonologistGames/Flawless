using UnityEngine;
using UnityEngine.VFX;

namespace Flawless.LifeSys
{
    public class PlayerLifeVFX : MonoBehaviour
    {
        public MeshRenderer MeshRenderer;
        private static readonly int SwitchValue = Shader.PropertyToID("_SwitchValue");

        public VisualEffect VisualEffect;
        
        private void Start()
        {
            FindObjectOfType<PlayerLife>().OnLifeAmountChanged += UpdateLifeAmountEffect;
        }

        private void UpdateLifeAmountEffect(float lifeAmount, float lifeUnit, int lifeUnitsCount)
        {
            MeshRenderer.material.SetFloat(SwitchValue, lifeAmount / (lifeUnit * lifeUnitsCount));
            VisualEffect.SetFloat("LifeAmount", lifeAmount / lifeUnit * 4);
        }
    }
}
