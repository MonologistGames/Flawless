using System.Collections.Generic;
using Flawless.LifeSys;
using UnityEngine;
using UnityEngine.UI;

namespace Flawless.UI.LifeAmount
{
    public class LifeAmountUI : MonoBehaviour
    {
        public float FillOffset = 0.2f;
        public List<Image> UnitFills;

        private void Awake()
        {
            GameObject.FindWithTag("Player").GetComponentInChildren<PlayerLifeAmount>().OnLifeAmountChanged +=
                UpdateLifeAmountUI;
        }

        public void UpdateLifeAmountUI(float lifeAmount, float lifeUnit, int lifeUnitsCount)
        {
            var unitCountFloor = Mathf.FloorToInt(lifeAmount / lifeUnit);
            var unitCountCeil = Mathf.CeilToInt(lifeAmount / lifeUnit);
            var fillValue = (lifeAmount - unitCountFloor * lifeUnit) / lifeUnit;

            // Extend or delete life units
            if (lifeUnitsCount > UnitFills.Count)
            {
                var unitsToAdd = lifeUnitsCount - UnitFills.Count;
                for (var i = 0; i < unitsToAdd; i++)
                {
                    var newFill = Instantiate(UnitFills[0].transform.parent, this.transform, true);
                    UnitFills.Add(newFill.GetChild(0).GetComponent<Image>());
                }
            }

            if (lifeUnitsCount < UnitFills.Count)
            {
                for (var i = lifeUnitsCount; i < UnitFills.Count; i++)
                {
                    UnitFills[i].transform.parent.gameObject.SetActive(false);
                }
            }

            // Set fill amount
            for (var i = 0; i < unitCountFloor; i++)
            {
                UnitFills[i].transform.parent.gameObject.SetActive(true);
                UnitFills[i].fillAmount = 1;
            }

            if (unitCountFloor == unitCountCeil)
            {
                return;
            }
            
            UnitFills[unitCountFloor].transform.parent.gameObject.SetActive(true);
            UnitFills[unitCountFloor].fillAmount = fillValue * (1 - FillOffset) + FillOffset;

            for (var i = unitCountCeil; i < lifeUnitsCount; i++)
            {
                UnitFills[i].transform.parent.gameObject.SetActive(true);
                UnitFills[i].fillAmount = 0;
            }
        }
    }
}