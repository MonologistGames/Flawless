using System.Collections;
using Cinemachine;
using Flawless.PlayerCharacter;
using UnityEngine;
using UnityEngine.VFX;

namespace Flawless.Levels.Gates
{
    public class OpenGate : MonoBehaviour
    {
        public VisualEffect OpenEffect;
        public Animator OpenGateAnimator;
        public SphereCollider Trigger;
        public CinemachineVirtualCamera VirtualCamera;
        public PlayerController PlayerController;
        public CanvasGroup NormalCanvasGroup;
        public GameObject CinematicPanel;
        
        private bool _isUnlocked = false;

        public void Unlock()
        {
            if (_isUnlocked) return;
            _isUnlocked = true;
            StartCoroutine(ActivateCamera());
        }

        IEnumerator ActivateCamera()
        {
            CinematicPanel.SetActive(true);
            NormalCanvasGroup.alpha = 0f;
            VirtualCamera.enabled = true;
            PlayerController.SetControlled();;
            yield return new WaitForSeconds(1f);
            OpenEffect.enabled = true;
            OpenGateAnimator.SetTrigger("Open");
            Trigger.enabled = true;
            yield return new WaitForSeconds(5f);   
            VirtualCamera.enabled = false;
            PlayerController.SetPlaying();
            NormalCanvasGroup.alpha = 1f;
            CinematicPanel.SetActive(false);
        }
    }
}