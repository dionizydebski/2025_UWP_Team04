using System.Collections.Generic;
using TMPro;
using Tower;
using UnityEngine;
using Wave;

namespace UI
{
    public class TutorialPresenter : MonoBehaviour
    {
        private readonly Dictionary<GameObject, bool> isWindowShown = new Dictionary<GameObject, bool>();
        private const string TutorialTag = "Tutorial";
        private const string PlaceTowerTutorialName = "PlaceTower";
        private const string SellTowerTutorialName = "SellTower";
        private TutorialEntry currentShownTutorial;

        private int currentTutorialIndex;


        [SerializeField] private TutorialView tutorialView;

        [Header("Tutorial Data")] [SerializeField]
        private List<TutorialEntry> tutorialEntries = new List<TutorialEntry>();

        private void Awake()
        {
            Debug.Log("Awake");
            TowerManager.OnTowerPlaced += OnTowerPlaced;
            TowerManager.OnTowerSold += OnTowerSold;
            TowerManager.OnTowerUnselected += OnTowerUnselected;
            WaveManager.OnWaveEnd += OnWaveEnd;
            TowerManager.OnTowerSelected += OnTowerSelected;
            PlayerActionsController.OnStopTowerPlacing += OnStopPlacingTower;
            FindAllTutorials();
            Debug.Log(tutorialEntries.Count);
            if (tutorialEntries.Count > 0)
            {
                currentShownTutorial = tutorialEntries[currentTutorialIndex];
                Debug.Log(currentShownTutorial);
            }

            ShowCurrentTutorial();
        }

        private void ShowCurrentTutorial()
        {
            if (currentShownTutorial == null || currentShownTutorial.tutorialObject == null) return;

            if (isWindowShown[currentShownTutorial.tutorialObject]) return;
            tutorialView.ShowTutorial(currentShownTutorial.tutorialObject);
            isWindowShown[currentShownTutorial.tutorialObject] = true;
            WaveManager.Instance.PauseWaveSpawning();

            var content = currentShownTutorial.content;
            if (content != null)
            {
                content.text = currentShownTutorial.tutorialTextSteps[0];
            }
        }

        private void OnWaveEnd()
        {
            if (currentTutorialIndex + 1 >= tutorialView.GetTutorialObjects().Count) return;

            currentTutorialIndex++;
            currentShownTutorial = tutorialEntries[currentTutorialIndex];
            ShowCurrentTutorial();
        }

        private void FindAllTutorials()
        {
            foreach (Transform child in transform)
            {
                if (!child.gameObject.tag.Equals(TutorialTag)) continue;
                isWindowShown.TryAdd(child.gameObject, false);
                tutorialView.AddTutorial(child.gameObject);
            }
        }

        private void OnTowerPlaced()
        {
            if (currentShownTutorial.tutorialObject.name.Equals(PlaceTowerTutorialName))
            {
                tutorialView.HideTutorial(currentShownTutorial.tutorialObject);
                WaveManager.Instance.UnpauseWaveSpawning();
            }
        }

        private void OnTowerSold()
        {
            if (currentShownTutorial.tutorialObject.name.Equals(SellTowerTutorialName))
            {
                tutorialView.HideTutorial(currentShownTutorial.tutorialObject);
                WaveManager.Instance.UnpauseWaveSpawning();
            }
        }

        public void OnTowerToPlaceSelected()
        {
            Debug.Log("Tutorial Tower Selected");
            if (!currentShownTutorial.tutorialObject.name.Equals(PlaceTowerTutorialName)) return;

            TMP_Text content = currentShownTutorial.content;
            if (content != null)
            {
                content.text = currentShownTutorial.tutorialTextSteps[1];
            }
        }

        private void OnStopPlacingTower()
        {
            if (!currentShownTutorial.tutorialObject.name.Equals(PlaceTowerTutorialName)) return;

            TMP_Text content = currentShownTutorial.content;
            if (content != null)
            {
                content.text = currentShownTutorial.tutorialTextSteps[0];
            }
        }

        private void OnTowerUnselected()
        {
            if (!currentShownTutorial.tutorialObject.name.Equals(SellTowerTutorialName)) return;

            TMP_Text content = currentShownTutorial.content;
            if (content != null)
            {
                content.text = currentShownTutorial.tutorialTextSteps[0];
            }
        }

        private void OnTowerSelected()
        {
            if (!currentShownTutorial.tutorialObject.name.Equals(SellTowerTutorialName)) return;

            TMP_Text content = currentShownTutorial.content;
            if (content != null)
            {
                content.text = currentShownTutorial.tutorialTextSteps[1];
            }
        }
    }
}