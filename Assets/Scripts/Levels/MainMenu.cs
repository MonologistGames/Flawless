using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Flawless
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject LoadingPanel;
        private void Update()
        {
            if (Keyboard.current.anyKey.wasPressedThisFrame)
            {
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