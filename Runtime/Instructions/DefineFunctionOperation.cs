using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculatorCompetition.Backend.Runtime.Instruction
{
    /// <summary>
    /// Represents the operation of defining a user-defined function.
    /// </summary>
    public class DefineFunctionOperation : SymbolManipulator, IInstruction
    {
        string function_name;
        IEnumerable<string> arg_names;
        int num_args;

        public DefineFunctionOperation(SymbolTable table, string name, IEnumerable<string> arg_names)
            : base(table)
        {
            function_name = name;
            this.arg_names = arg_names;
            num_args = arg_names.Count();
        }

        #region IInstruction implementation

        public IEnumerable<Variable> Operate(IList<Variable> arguments)
        {
            var instructions = arguments[0];
            if(!instructions.IsInstructions){
                throw new InvalidOperationException(
                    string.Format("Without instructions, there is no way of achieving something.")
                );
            }

            return null;
        }

        public int NumberOfConsumingArguments {
            get {
                return 1;
            }
        }

        #endregion
    }
}

