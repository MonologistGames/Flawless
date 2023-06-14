using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;
using UnityEngine.SceneManagement;

namespace Flawless
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject LoadingPanel;
        private bool _playing = false;
        private void Update()
        {
            if (_playing)
            {
                return;
            }
            
            if (Keyboard.current.anyKey.isPressed || Mouse.current.leftButton.isPressed || Mouse.current.rightButton.isPressed
                || (Gamepad.current != null && Gamepad.current.buttonSouth.isPressed))
            {
                _playing = true;
                StartCoroutine(SceneChangeCoroutine(1));
            }
        }
        
        IEnumerator SceneChangeCoroutine(int index)
        {
            LoadingPanel.SetActive(true);
            float loadTime = 3f;
            AsyncOperation sceneLoad =  SceneManager.LoadSceneAsync(index);
            sceneLoad.allowSceneActivation = false;

            while (sceneLoad.progress < 0.9f)
            {
                loadTime -= Time.deltaTime;
                yield return null;
            }

            while (loadTime > 0f)
            {
                loadTime -= Time.deltaTime;
                yield return null;
            }
            
            sceneLoad.allowSceneActivation = true;
        }
    }
}