using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI.Tutorial
{
    [System.Serializable]
    public class TutorialEntry
    {
        public GameObject tutorialObject;
        public List<string> tutorialTextSteps;
        public TMP_Text content;
        
    }
}