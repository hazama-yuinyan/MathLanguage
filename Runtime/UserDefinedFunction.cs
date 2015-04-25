using System;
using System.Collections.Generic;
using CalculatorCompetition.Backend.Runtime.Instruction;
using System.Linq;

namespace CalculatorCompetition.Backend.Runtime
{
    /// <summary>
    /// Represents a user-defined function.
    /// </summary>
    public class UserDefinedFunction
    {
        StackMachine machine;
        IList<IInstruction> instructions;
        IEnumerable<string> arg_names;

        public UserDefinedFunction(StackMachine machine, IList<IInstruction> instructions, IEnumerable<string> argNames)
        {
            this.machine = machine;
            this.instructions = instructions;
            arg_names = argNames;
        }

        public void Call(IList<Variable> args)
        {
            var parent_symbols = new List<Variable>(args.Count);
            // Keeps old values aside
            foreach(var pair in arg_names.Zip(args,
                (a, b) => new Tuple<string, Variable>(a, b))){
                if(StackMachine.Symbols.HasVariable(pair.Item1))
                    parent_symbols.Add(StackMachine.Symbols.GetVariable(pair.Item1));

                StackMachine.Symbols.AssignVariable(pair.Item1, pair.Item2);
            }

            foreach(var inst in instructions)
                machine.Operate(inst);

            foreach(var pair in parent_symbols.Zip(arg_names,
                (a, b) => new Tuple<string, Variable>(b, a)))
                StackMachine.Symbols.AssignVariable(pair.Item1, pair.Item2);
        }
    }
}
