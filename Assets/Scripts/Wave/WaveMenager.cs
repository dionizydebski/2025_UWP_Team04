using System;
using UnityEngine;

namespace Wave
{
    public class WaveMenager : MyMonoBehaviour
    {
        public static WaveMenager waveMenager;

        public Transform startPoint;
        public Transform[] path;

        private void Awake()
        {
            waveMenager = this;
        }
    }
}
