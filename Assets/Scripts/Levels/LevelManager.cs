using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Monologist.Patterns.Singleton;

using System.Collections;
using Flawless.Levels.Gates;
using Flawless.LifeSys;

namespace Flawless
{
    public class LevelManager : Singleton<LevelManager>
    {
        public GameObject LoadingPanel;
        public OpenGate OpenGate;
        private PlanetLife[] _planets;
        private int _currentSceneIndex;

        #region Monobehaviour Callbacks

        private void OnEnable()
        {
            Initialize();
        }

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

        public void ChangeToNextScene()
        {
            StartCoroutine(SceneChangeCoroutine(_currentSceneIndex + 1));
        }

        private void GetPlanets()
        {
            _planets = FindObjectsOfType<PlanetLife>();
        }

        public void Initialize()
        {
            _currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            GetPlanets();
            FindObjectOfType<PlayerLife>().OnAbsorbed += CheckSceneUnlocked;
        }

        public void CheckSceneUnlocked()
        {
            var isUnlocked = true;
            
            foreach (var planetLife in _planets)
            {
                isUnlocked &= (planetLife.IsAbsorbed & planetLife.LifeAmount == 0);
            }

            if (isUnlocked)
            {
                OpenGate.Unlock();
            }
        }
    }
}