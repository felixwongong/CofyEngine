using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

namespace CofyEngine.Engine.Util.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextBinder : UIBinder<TextMeshProUGUI>
    {
        private const string pattern = @"\{(.+?)\}";

        protected override void Awake()
        {
            scope = GetComponent<UIScope>();

            if (scope == null)
            {
                scope = gameObject.AddComponent<UIScope>();
                scope.useGameObjectName = false;
            }
            
            if (!scope.useGameObjectName)
            {
                target ??= GetComponent<TextMeshProUGUI>();
                Match match = Regex.Match(target.text, pattern);
                scope.scopeName =  match.Success ? match.Groups[1].Value : string.Empty;
            }
            
            base.Awake();
        }
        
        //TODO: Add replacing placeholder instead of direct setting text in tmp
    }
}