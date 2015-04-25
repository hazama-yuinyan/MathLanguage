using System;
using System.Collections.Generic;

namespace CalculatorCompetition.Backend.Runtime.Instruction
{
    /// <summary>
    /// Operation for referring a symbol.
    /// </summary>
    public class ReferenceSymbolOperation : IInstruction
    {
        StackMachine machine;
        string symbol_name;

        public ReferenceSymbolOperation(StackMachine machine, string name)
        {
            this.machine = machine;
            symbol_name = name;
        }

        #region IInstruction implementation

        public IEnumerable<Variable> Operate(IList<Variable> arguments)
        {
            var variable = StackMachine.Symbols.GetVariable(symbol_name);
            machine.PushOnStack(variable);
            return null;
        }

        public int NumberOfConsumingArguments {
            get {
                return 0;
            }
        }

        #endregion
    }
}

