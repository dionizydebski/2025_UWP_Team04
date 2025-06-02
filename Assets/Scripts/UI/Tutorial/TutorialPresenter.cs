using System.Collections.Generic;
using Core;
using TMPro;
using Tower;
using UnityEngine;
using Wave;

namespace UI.Tutorial
{
    public class TutorialPresenter : MonoBehaviour
    {
        private readonly Dictionary<GameObject, bool> _isWindowShown = new Dictionary<GameObject, bool>();
        private const string TutorialTag = "Tutorial";
        private TutorialEntry _currentShownTutorial;

        private int _currentTutorialIndex;


        [SerializeField] private TutorialView tutorialView;

        [Header("Tutorial Data")] [SerializeField]
        private List<TutorialEntry> tutorialEntries = new List<TutorialEntry>();

        private void Awake()
        {
            TutorialEventsManager.Instance.NextTutorial += ShowNextTutorial;
            TutorialEventsManager.Instance.OnTutorialStep += OnTutorialStepTriggered;
            FindAllTutorials();
            Debug.Log(tutorialEntries.Count);
            if (tutorialEntries.Count > 0)
            {
                _currentShownTutorial = tutorialEntries[_currentTutorialIndex];
                Debug.Log(_currentShownTutorial);
            }

            ShowCurrentTutorial();
        }

        private void OnTutorialStepTriggered(string tutorialName, int tutorialStep)
        {
            Debug.Log("OnTutorialStepTriggered: " + tutorialName + " tutorial step: " + tutorialStep);
            if (_currentShownTutorial == null || !_currentShownTutorial.tutorialObject.name.Equals(tutorialName)) return;

            var content = _currentShownTutorial.content;
            if (content != null && _currentShownTutorial.tutorialTextSteps.Count > tutorialStep)
            {
                content.text = _currentShownTutorial.tutorialTextSteps[tutorialStep];
            }
            if (tutorialStep == _currentShownTutorial.tutorialTextSteps.Count)
            {
                tutorialView.HideTutorial(_currentShownTutorial.tutorialObject);
                //WaveManager.Instance.UnpauseWaveSpawning();
                Time.timeScale = 1;
            }
        }


        private void ShowCurrentTutorial()
        {
            if (_currentShownTutorial == null || _currentShownTutorial.tutorialObject == null) return;

            if (_isWindowShown[_currentShownTutorial.tutorialObject]) return;
            tutorialView.ShowTutorial(_currentShownTutorial.tutorialObject);
            _isWindowShown[_currentShownTutorial.tutorialObject] = true;
            //WaveManager.Instance.PauseWaveSpawning();
            Time.timeScale = 0;

            var content = _currentShownTutorial.content;
            if (content != null)
            {
                content.text = _currentShownTutorial.tutorialTextSteps[0];
            }
        }

        private void ShowNextTutorial()
        {
            if (_currentTutorialIndex + 1 >= tutorialView.GetTutorialObjects().Count) return;

            _currentTutorialIndex++;
            _currentShownTutorial = tutorialEntries[_currentTutorialIndex];
            ShowCurrentTutorial();
        }

        private void FindAllTutorials()
        {
            foreach (Transform child in transform)
            {
                if (!child.gameObject.tag.Equals(TutorialTag)) continue;
                _isWindowShown.TryAdd(child.gameObject, false);
                tutorialView.AddTutorial(child.gameObject);
            }
        }

        public void OnTowerToPlaceSelected()
        {
            OnTutorialStepTriggered(TutorialEventsManager.PlaceTowerTutorialName, 1);
        }
    }
}