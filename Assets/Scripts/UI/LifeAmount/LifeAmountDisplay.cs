using UnityEngine;
using UnityEngine.UI;
using Flawless.LifeSys;

namespace Flawless.UI.LifeAmount
{
    public class LifeAmountDisplay : MonoBehaviour
    {
        [Header("Slider UIs")] public Slider PlantSlider;
        private Image _plantSliderFill;
        private Image _plantScroller;

        public Slider AnimalSlider;
        private Image _animalSliderFill;
        private Image _animalScroller;

        private PlayerLifeAmount _player;

        public Color DisabledColor = new Color(0.2f, 0.2f, 0.2f, 0.5f);
        public float FadeThreshold = 0.05f;
        [Header("Slider Offset")] public float PlantSliderOffset = 0.46f;
        public float AnimalSliderOffset = 0.174f;

        #region MonoBehaviours

        void Start()
        {
            _plantSliderFill = PlantSlider.transform.GetChild(0).GetChild(0).GetComponent<Image>();
            _plantScroller = _plantSliderFill.transform.GetChild(0).GetComponent<Image>();

            _animalSliderFill = AnimalSlider.transform.GetChild(0).GetChild(0).GetComponent<Image>();
            _animalScroller = _animalSliderFill.transform.GetChild(0).GetComponentInChildren<Image>();

            _player = GameObject.FindWithTag("Player").GetComponentInChildren<PlayerLifeAmount>();
        }

        // Update is called once per frame
        void Update()
        {
            float plantValue = _player.PlantAmount / _player.MaxAmount;
            float animalValue = _player.AnimalAmount / _player.MaxAmount;

            #region Slider Color

            // Change the slider color to dark. In order to make this
            // Slider more obvious for players.
            // Uses a threshold to control where to begin fading.
            if (plantValue <= FadeThreshold)
            {
                Color fadedColor =
                    Color.Lerp(DisabledColor, Color.white, plantValue / FadeThreshold);
                _plantSliderFill.color = fadedColor;
                _plantScroller.color = fadedColor;
            }
            else
            {
                _plantSliderFill.color = Color.white;
                _plantScroller.color = Color.white;
            }

            if (animalValue <= FadeThreshold)
            {
                Color fadedColor =
                    Color.Lerp(DisabledColor, Color.white, animalValue / FadeThreshold);
                _animalSliderFill.color = fadedColor;
                _animalScroller.color = fadedColor;
            }
            else
            {
                _animalSliderFill.color = Color.white;
                _animalScroller.color = Color.white;
            }

            #endregion

            #region Slider Value

            PlantSlider.value = plantValue + PlantSliderOffset;
            AnimalSlider.value = plantValue + animalValue + AnimalSliderOffset;

            #endregion
        }

        #endregion

    }
}