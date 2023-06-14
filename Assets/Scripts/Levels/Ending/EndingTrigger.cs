using System.Collections;
using Flawless.PlayerCharacter;
using Unity.VisualScripting;
using UnityEngine.Video;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace Flawless
{
    public class EndingTrigger : MonoBehaviour
    {
        public VideoClip EndingVideo;
        public VideoPlayer VideoPlayer;

        public GameObject Subtitle;
        public GameObject Cover;

        private void OnTriggerEnter(Collider other)
        {
            var playerController = other.GetComponent<PlayerController>();
            if (playerController == null) return;
            
            playerController.SetControlled();
            Cover.SetActive(true);
            foreach (var lensFlare in FindObjectsOfType<LensFlareComponentSRP>())
            {
                lensFlare.enabled = false;
            }
            
            VideoPlayer.clip = EndingVideo;
            VideoPlayer.loopPointReached += EndTitle;
            VideoPlayer.IsAudioTrackEnabled(0);
            VideoPlayer.Play();
        }

        private void EndTitle(VideoPlayer source)
        {
            StartCoroutine(ScrollSubtitle());
        }
        
        private IEnumerator ScrollSubtitle()
        {
            Subtitle.SetActive(true);
            yield return new WaitForSeconds(5f);
            SceneManager.LoadScene(0);
        }
    }
}