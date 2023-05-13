using System;
using System.Collections;
using System.Collections.Generic;
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
            AbsorbEffect.SetBool("IsAbsorbing", state);
        }

        private void UpdateMaterial(float lifeAmount)
        {
            MeshRenderer.material.color = Color.Lerp(Color.black, Color.white, lifeAmount / _initLifeAmount);
        }
    }
}
