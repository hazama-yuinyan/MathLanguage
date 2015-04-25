using System;
using System.Collections.Generic;

namespace CalculatorCompetition.Backend.Runtime.Instruction
{
    /// <summary>
    /// Interface for instructions of the math language stack machine.
    /// </summary>
    public interface IInstruction
    {
        /// <summary>
        /// The number of arguments the stack machine will pop off its stack and hand on to
        /// the current instruction.
        /// </summary>
        int NumberOfConsumingArguments{
            get;
        }

        /// <summary>
        /// Operates this instruction and returns the result(s).
        /// </summary>
        IEnumerable<Variable> Operate(IList<Variable> arguments);
    }
}

