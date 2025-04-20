using System;
using Singleton;

namespace Core
{
    public class TutorialEventsManager : Singleton<TutorialEventsManager>
    {
        public const string SellTowerTutorialName = "SellTower";
        public const string PlaceTowerTutorialName = "PlaceTower";
        
        public event Action<string, int> OnTutorialStep;

        public void TriggerTutorialEvent(string tutorialName, int tutorialStep)
        {
            this.OnTutorialStep?.Invoke(tutorialName, tutorialStep);
        }
    }
}