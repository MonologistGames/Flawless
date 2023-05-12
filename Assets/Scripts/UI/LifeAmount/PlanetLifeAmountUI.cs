using System.Collections;
using System.Collections.Generic;
using Flawless.LifeSys;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Flawless.UI.LifeAmount
{
    public class PlanetLifeAmountUI : MonoBehaviour
    {
        private CanvasGroup LifeAmountCanvas { get; set; }
        private bool IsCanvasOn { get; set; }
        private PlanetLifeAmount LifeAmount { get; set; }
        public float ShowUpSpeed = 2f;
        public Image LifeFill;

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
            
            LifeFill.fillAmount = LifeAmount.LifeAmount / PlanetLifeAmount.MaxLifeAmount;
            
        }
        
        #endregion

        public void TurnCanvasOn(Collider other)
        {
            IsCanvasOn = true;
        }

        public void TurnCanvasOff(Collider other)
        {
            IsCanvasOn = false;
        }
    }
}
