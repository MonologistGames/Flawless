using UnityEngine;
using UnityEngine.SceneManagement;
using Monologist.Patterns.Singleton;

using System.Collections;
using Flawless.LifeSys;

namespace Flawless
{
    public class LevelManager : SingletonPersistent<LevelManager>
    {
        public GameObject LoadingPanel;
        private PlanetLife[] _planets;

        #region Monobehaviour Callbacks

        

            #endregion
        
        IEnumerator SceneChangeCoroutine(int index)
        {
            LoadingPanel.SetActive(true);
            float loadTime = 1f;
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
            
            LoadingPanel.SetActive(false);
            sceneLoad.allowSceneActivation = true;
            Initialize();
        }

        public void ChangeToSceneIndex(int index)
        {
            StartCoroutine(SceneChangeCoroutine(index));
        }

        public void GetPlanets()
        {
            _planets = FindObjectsOfType<PlanetLife>();
        }

        public void Initialize()
        {
            GetPlanets();
            FindObjectOfType<PlayerLife>().OnAbsorbed += CheckSceneUnlocked;
        }

        public void CheckSceneUnlocked()
        {
            var isUnlocked = true;
            
            foreach (var planetLife in _planets)
            {
                isUnlocked &= planetLife.IsAbsorbed;
            }

            if (isUnlocked)
            {
                // TODO：解锁下一关
            }
        }
    }
}