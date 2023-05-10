using System.Collections;
using System.Collections.Generic;
using Flawless.LifeSys;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Flawless.UI.LifeAmount
{
    public class PlanetLifeAmountDisplay : MonoBehaviour
    {
        private CanvasGroup LifeAmountCanvas { get; set; }
        private bool IsCanvasOn { get; set; }
        private PlanetLifeAmount LifeAmount { get; set; }
        public float ShowUpSpeed = 2f;
        public Image PlantImage;

        public Image AnimalImage;
        
        [Tooltip("Decides how much space there is between plant and animal")]
        public float Offset = 0.05f;

        #region MonoBehaviours
        
        // Start is called before the first frame update
        private void Start()
        {
            LifeAmountCanvas = GetComponent<CanvasGroup>();
            LifeAmount = GetComponentInParent<PlanetLifeAmount>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (IsCanvasOn && LifeAmountCanvas.alpha <1)
            {
                LifeAmountCanvas.alpha += Time.unscaledDeltaTime * ShowUpSpeed;
            }
            else if (!IsCanvasOn && LifeAmountCanvas.alpha > 0)
            {
                LifeAmountCanvas.alpha -= Time.unscaledDeltaTime * ShowUpSpeed;
            }
            
            float plantValue = LifeAmount.PlantAmount / PlanetLifeAmount.MaxLifeAmount;
            float animalValue = LifeAmount.AnimalAmount / PlanetLifeAmount.MaxLifeAmount;

            PlantImage.fillAmount = plantValue - Offset;
            AnimalImage.fillAmount = animalValue;
            
            AnimalImage.transform.localRotation =
                Quaternion.Euler(0,0,- (plantValue + Offset) * 360);
        }
        
        #endregion

        public void TurnCanvasOn(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            IsCanvasOn = true;
        }

        public void TurnCanvasOff(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            IsCanvasOn = false;
        }
    }
}
