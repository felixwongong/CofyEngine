﻿using System;
using System.Reflection;
using System.Text;
using UnityCodeGen;
using UnityEditor;

namespace CofyEngine.Editor
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class AUIPanelAttribute : Attribute
    {
        public string path { get; set; }
        
        public AUIPanelAttribute(string path)
        {
            this.path = path;
        }
    }
    
    [Generator]
    public class UIPanelGenerator: ICodeGenerator
    {
        public void Execute(GeneratorContext context)
        {
            context.OverrideFolderPath("Assets/_Generated");
            
            var types = TypeCache.GetTypesWithAttribute<AUIPanelAttribute>();
            
            StringBuilder sb = new StringBuilder();
            foreach (var type in types)
            {
                var attr = type.GetCustomAttribute<AUIPanelAttribute>();
                var path = attr.path;

                sb.AppendFormat("loadFutures.Add(BindUI(new {0}() ,\"{1}\"));\n", type.Name, path);
            }
            
            var code = 
                $@"using System.Collections.Generic;
using CofyEngine;

namespace CofyDev.CityMonster
{{
    // <auto-generated/>
    public  class UILoadStateImpl: UILoadState
    {{
        protected override Future<List<UIPanel>> LoadAll()
        {{
            List<Future<UIPanel>> loadFutures = new List<Future<UIPanel>>();

            {sb}
            return loadFutures.Group();
        }}
    }}
}}
";
            
            context.AddCode("UILoadState.Generated.cs", code);
        }
    }
}