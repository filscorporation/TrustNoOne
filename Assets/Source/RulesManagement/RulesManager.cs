using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Assets.Source.RulesManagement
{
    /// <summary>
    /// Manages all ingame rules to players movement
    /// </summary>
    public class RulesManager
    {
        private Dictionary<string, RuleBase> rules;

        /// <summary>
        /// Loads all rules from assembly
        /// </summary>
        public void LoadRulesFromSoltion()
        {
            rules = new Dictionary<string, RuleBase>();
            foreach (Type type in Assembly.GetAssembly(typeof(RuleBase)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(RuleBase))))
            {
                RuleBase rule = (RuleBase)Activator.CreateInstance(type);
                rules[rule.Name] = rule;
            }
        }

        /// <summary>
        /// Check rule by its name
        /// </summary>
        /// <param name="ruleName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public bool CheckRule(string ruleName, Dictionary<string, int> parameters)
        {
            return rules[ruleName].Check(FieldManager.Instance.Field, Player.Instance.Steps, parameters);
        }
    }
}
