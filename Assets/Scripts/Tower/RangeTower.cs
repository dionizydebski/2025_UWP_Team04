using System;
using System.Collections;
using System.Collections.Generic;
using Core;

namespace Tower
{
    public class RangeTower : BaseTower
    {
        public static int cost;
        private List<BaseTower> _towers = new List<BaseTower>();
        public string ProductName { get; set; }
        public void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}