using System;
using System.Collections.Generic;

namespace CalculatorCompetition.Backend.Runtime
{
    /// <summary>
    /// Represents the symbol table.
    /// It manages all the variables and the functions.
    /// </summary>
    public class SymbolTable
    {
        Dictionary<string, Variable> variable_table = new Dictionary<string, Variable>();
        Dictionary<string, IList<Delegate>> function_table = new Dictionary<string, IList<Delegate>>();

        public void AssignVariable(string name, Variable new_value)
        {
            if(!variable_table.ContainsKey(name))
                variable_table.Add(name, new_value);
            else
                variable_table[name] = new_value;
        }

        public Variable GetVariable(string name)
        {
            if(!variable_table.ContainsKey(name)){
                throw new InvalidOperationException(
                    string.Format("{0} is not defined!", name)
                );
            }
            return variable_table[name];
        }

        public void AddFunction(string name, Delegate function)
        {
            if(!function_table.ContainsKey(name)){
                function_table.Add(name, new List<Delegate>{function});
            }else{
                var values = function_table[name];
                values.Add(function);
            }
        }

        public IList<Delegate> GetFunction(string name)
        {
            return function_table[name];
        }
    }
}

