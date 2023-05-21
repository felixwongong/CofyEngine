using System;
using System.Collections.Generic;

namespace cofydev.util.StateMachine
{
    public class EState
    {
        private static readonly Dictionary<string, EState> Values = new();
        private readonly Type Value;

        public EState(Type stateType)
        {
            if (stateType.GetInterface(nameof(IStateContext)) == null)
                throw new Exception($"Type ({stateType}) not implementing IStateContext, early ended");

            Value = stateType;
            Values[stateType.Name] = this;
        }

        protected EState()
        {
        }

        public static explicit operator Type(EState type)
        {
            return type.Value;
        }

        public static implicit operator EState(Type type)
        {
            if (!Values.ContainsKey(type.Name))
                throw new Exception($"Type ({type.FullName}) not defined. Please defined in class before use.");

            return Values[type.Name];
        }

        public static implicit operator string(EState type)
        {
            return type.Value.Name;
        }

        public static explicit operator EState(string typeName)
        {
            if (!Values.ContainsKey(typeName))
                throw new Exception($"Type ({typeName}) not defined. Please defined in class before use.");

            return Values[typeName];
        }
    }
}