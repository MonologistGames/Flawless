using System.Collections;
using Flawless.LifeSys;
using Flawless.PlayerCharacter;
using UnityEngine;

namespace Flawless
{
   [RequireComponent(typeof(Collider))] 
    public class DeathTrigger : MonoBehaviour
    {
        public Transform RespawnPoint;
        public float FadeTime = 1f;
        public Animator WhiteFieldAnimator;
        
        private PlanetController _planetController;
        private PlayerLifeAmount _playerLifeAmount;
        
        private static readonly int Begin = Animator.StringToHash("Begin");

        private void Start()
        {
            _planetController = FindObjectOfType<PlanetController>();
            _playerLifeAmount = FindObjectOfType<PlayerLifeAmount>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            WhiteFieldAnimator.SetTrigger(Begin);
            _playerLifeAmount.LifeAmount = 0;
            
            StartCoroutine(Respawn());
        }
        
        private IEnumerator Respawn()
        {
            yield return new WaitForSeconds(FadeTime);
            _planetController.transform.position = RespawnPoint.position;
            _planetController.SetControlled();
            yield return new WaitForSeconds(FadeTime);
            _planetController.SetPlaying();
        }
    }
}
