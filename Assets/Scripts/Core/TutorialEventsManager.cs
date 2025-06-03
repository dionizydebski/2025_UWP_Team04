using System;
using Singleton;

namespace Core
{
    public class TutorialEventsManager : Singleton<TutorialEventsManager>
    {
        public const string SellTowerTutorialName = "SellTower";
        public const string PlaceTowerTutorialName = "PlaceTower";
        public const string EnemyAttackTutorialName = "EnemyAttack";
        public const string UpgradeTowerTutorialName = "UpgradeTower";
        public const string SetStrategyTutorialName = "SetStrategy";
        
        public event Action<string, int> OnTutorialStep;
        public event Action NextTutorial;

        public void TriggerTutorialStepEvent(string tutorialName, int tutorialStep)
        {
            this.OnTutorialStep?.Invoke(tutorialName, tutorialStep);
        }
        
        public void TriggerNextTutorialEvent()
        {
            this.NextTutorial?.Invoke();
        }
    }
}