using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flawless.LifeSys;
using Unity.VisualScripting;

namespace Flawless.Planet.PlanetsFunctions
{
    [RequireComponent(typeof(GravitationField))]
    public class PlanetAir : PlanetLife
    {
        private GravitationField _gravitationField;
        private float _mass;

        private float _nowScale;
        private float _atmosScale;
        [Header("Atmos")] public Transform Atmos;
        [Header("ShrinkVale")] public float objectiveScale = 0.6f;
        public float shrinkSpeed = 0.1f;

        private bool isDead = false;

        public void Start()
        {
            _gravitationField = GetComponent<GravitationField>();
            _mass = _gravitationField.Mass;

            _nowScale = this.transform.localScale.x;
            _atmosScale = Atmos.localScale.x;
        }

        public void Update()
        {
            if (isDead && _nowScale > objectiveScale)
            {
                Shrink();
            }
        }

        public override void SetPlanetDead()
        {
            _gravitationField.Mass = _mass * 2;
            isDead = true;
        }

        private void Shrink()
        {
            _nowScale -= shrinkSpeed * Time.unscaledDeltaTime;
            this.transform.localScale = new Vector3(_nowScale, _nowScale, _nowScale);
            _atmosScale -= shrinkSpeed * Time.unscaledDeltaTime;
            Atmos.localScale = new Vector3(_atmosScale, _atmosScale, _atmosScale);
        }
    }
}