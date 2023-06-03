using Flawless.LifeSys;
using UnityEngine;
using UnityEngine.VFX;

namespace Flawless
{
    public class PlanetLifeAmountEffect : MonoBehaviour
    {
        public VisualEffect AbsorbEffect;

        public MeshRenderer MeshRenderer;
        
        public PlanetLifeAmount PlanetLifeAmount;

        private float _initLifeAmount;
        private bool _isEffectOn;

        private static readonly int Saturation = Shader.PropertyToID("_Saturation");

        // Start is called before the first frame update
        void Start()
        {
            _initLifeAmount = PlanetLifeAmount.LifeAmount;
            
            FindObjectOfType<PlayerLifeAmount>().OnAbsorbStateChanged += UpdateAbsorbEffect;
            PlanetLifeAmount.OnLifeAmountChanged += UpdateMaterial;
        }

        // Update is called once per frame
        private void UpdateAbsorbEffect(bool state, PlanetLifeAmount planetLifeAmount)
        {
            if (planetLifeAmount != PlanetLifeAmount || !PlanetLifeAmount) return;
            if (_isEffectOn == state) return;
            AbsorbEffect.SetBool("IsAbsorbing", state);
        }

        private void UpdateMaterial(float lifeAmount)
        {
            MeshRenderer.material.SetFloat(Saturation, lifeAmount / _initLifeAmount);
        }
    }
}
