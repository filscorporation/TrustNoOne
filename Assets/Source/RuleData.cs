using System;
using System.Collections.Generic;

namespace Assets.Source
{
    [Serializable]
    public class RuleData
    {
        public string Name;

        public Dictionary<string, int> Parameters;
    }
}
