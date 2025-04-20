using System;
using System.Collections.Generic;
using UI.Tutorial;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class TutorialView : MonoBehaviour
    {
        private readonly List<GameObject> tutorials = new();
        private const string TutorialTag = "Tutorial";
        
        [SerializeField] private TutorialPresenter tutorialPresenter;

        public void ShowTutorial(GameObject tutorial)
        {
            if (!tutorials.Contains(tutorial)) return;
            tutorial.SetActive(true);
        }

        public void HideTutorial(GameObject tutorial)
        {
            if (!tutorials.Contains(tutorial)) return;
            tutorial.SetActive(false);
        }

        public bool GetTutorialVisibility(GameObject tutorial)
        {
            if (!tutorials.Contains(tutorial)) return false;
            return tutorial.activeSelf;
        }

        public List<GameObject> GetTutorialObjects()
        {
            return tutorials;
        }

        public void AddTutorial(GameObject tutorial)
        {
            tutorials.Add(tutorial);
        }
    }
}
