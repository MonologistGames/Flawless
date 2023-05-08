using UnityEngine;
using UnityEngine.UI;

namespace Flawless.LifeSys.UI
{
    public class LifeAmountDisplay : MonoBehaviour
    {
        [Header("Slider UIs")]
        public Slider PlantSlider;
        private Image _plantSliderFill;
        private Image _plantScroller;
        
        public Slider AnimalSlider;
        private Image _animalSliderFill;
        private Image _animalScroller;
        
        private PlayerLifeAmount _player;

        public Color DisabledColor = new Color(0.2f, 0.2f, 0.2f, 0.5f);
        [Header("Slider Offset")]
        public float PlantSliderOffset = 0.46f;
        public float AnimalSliderOffset = 0.174f;
        
        // Start is called before the first frame update
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
            if (_player.PlantAmount <= 0f)
            {
                _plantSliderFill.color = DisabledColor;
                _plantScroller.color = DisabledColor;
            }
            else
            {
                _plantSliderFill.color = Color.white;
                _plantScroller.color = Color.white;
            }

            if (_player.AnimalAmount <= 0f)
            {
                _animalSliderFill.color = DisabledColor;
                _animalScroller.color = DisabledColor;
            }
            else
            {
                _animalSliderFill.color = Color.white;
                _animalScroller.color = Color.white;
            }
        }
    }
}
