using Flawless.LifeSys;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;

namespace Flawless
{
    public class PlanetLifeVFX : MonoBehaviour
    {
        public VisualEffect AbsorbEffect;

        public MeshRenderer MeshRenderer;
        
        [FormerlySerializedAs("PlanetLifeAmount")] public PlanetLife PlanetLife;

        private float _initLifeAmount;
        private bool _isEffectOn;

        private static readonly int Saturation = Shader.PropertyToID("_Saturation");

        // Start is called before the first frame update
        void Start()
        {
            _initLifeAmount = PlanetLife.LifeAmount;
            
            FindObjectOfType<PlayerLife>().OnAbsorbStateChanged += UpdateAbsorbEffect;
            PlanetLife.OnLifeAmountChanged += UpdateMaterial;
        }

        // Update is called once per frame
        private void UpdateAbsorbEffect(bool state, PlanetLife planetLife)
        {
            if (planetLife != PlanetLife || !PlanetLife) return;
            if (_isEffectOn == state) return;
            _isEffectOn = state;
            AbsorbEffect.SetBool("IsAbsorbing", state);
        }

        private void UpdateMaterial(float lifeAmount)
        {
            MeshRenderer.material.SetFloat(Saturation, lifeAmount / _initLifeAmount);
        }
    }
}
