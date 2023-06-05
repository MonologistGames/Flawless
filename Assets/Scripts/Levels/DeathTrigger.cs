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
        
        private PlayerController _playerController;
        private PlayerLife _playerLife;
        
        private static readonly int Begin = Animator.StringToHash("Begin");

        private void Start()
        {
            _playerController = FindObjectOfType<PlayerController>();
            _playerLife = FindObjectOfType<PlayerLife>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            WhiteFieldAnimator.SetTrigger(Begin);
            _playerLife.LifeAmount = 0;
            
            StartCoroutine(Respawn());
        }
        
        private IEnumerator Respawn()
        {
            yield return new WaitForSeconds(FadeTime);
            _playerController.transform.position = RespawnPoint.position;
            _playerController.SetControlled();
            yield return new WaitForSeconds(FadeTime);
            _playerController.SetPlaying();
        }
    }
}
