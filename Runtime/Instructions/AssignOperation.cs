using System;
using System.Collections.Generic;

namespace CalculatorCompetition.Backend.Runtime.Instruction
{
    /// <summary>
    /// Represents the assignment operation.
    /// </summary>
    public class AssignOperation : SymbolManipulator, IInstruction
    {
        string name;

        public AssignOperation(SymbolTable table, string name)
            : base(table)
        {
            this.name = name;
        }

        #region IInstruction implementation

        public IEnumerable<Variable> Operate(IList<Variable> arguments)
        {
            var variable = arguments[0];

            symbols.AssignVariable(name, variable);
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

