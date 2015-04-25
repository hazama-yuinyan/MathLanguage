using System;
using System.Collections.Generic;
using CalculatorCompetition.Backend.Runtime.Instruction;
using System.Linq;
using System.Linq.Expressions;

namespace CalculatorCompetition.Backend.Runtime
{
    /// <summary>
    /// Represents the stack machine for the language.
    /// </summary>
    public class StackMachine
    {
        const int StackCapacity = 100;

        static Stack<Variable> stack;
        internal static SymbolTable Symbols{
            get; set;
        }

        public static bool IsStackEmpty{
            get{return stack.Count == 0;}
        }

        static StackMachine()
        {
            stack = new Stack<Variable>(StackCapacity);
            Symbols = new SymbolTable();
            RegisterFunctions();
        }

        static void RegisterFunctions()
        {
            string[] exclude_names = new string[]{
                "GetHashCode",
                "ToString",
                "Equals",
                "ReferenceEquals",
                "DivRem",
                "GetType"
            };

            foreach(var func in typeof(Math).GetMethods()){
                if(exclude_names.Any(name => name.Equals(func.Name)))
                    continue;

                var parameters = func.GetParameters();
                var return_type = func.ReturnType;
                Delegate @delegate;
                if(return_type == typeof(void)){
                    var param_types = parameters.Select(x => x.ParameterType).ToArray();
                    @delegate = 
                        (parameters.Length == 0) ? func.CreateDelegate(typeof(Action<>)) :
                        (parameters.Length == 1) ? func.CreateDelegate(typeof(Action<>).MakeGenericType(param_types)) :
                        (parameters.Length == 2) ? func.CreateDelegate(typeof(Action<,>).MakeGenericType(param_types)) :
                        (parameters.Length == 3) ? func.CreateDelegate(typeof(Action<,,>).MakeGenericType(param_types)) :
                        (parameters.Length == 4) ? func.CreateDelegate(typeof(Action<,,,>).MakeGenericType(param_types)) : null;
                }else{
                    var param_types = parameters.Select(x => x.ParameterType).Concat(new []{return_type}).ToArray();
                    @delegate = 
                        (parameters.Length == 0) ? func.CreateDelegate(typeof(Func<>).MakeGenericType(param_types)) :
                        (parameters.Length == 1) ? func.CreateDelegate(typeof(Func<,>).MakeGenericType(param_types)) :
                        (parameters.Length == 2) ? func.CreateDelegate(typeof(Func<,,>).MakeGenericType(param_types)) :
                        (parameters.Length == 3) ? func.CreateDelegate(typeof(Func<,,,>).MakeGenericType(param_types)) : null;
                }
                Symbols.AddFunction(func.Name, @delegate);
            }
        }

        public void PushOnStack(Variable variable)
        {
            stack.Push(variable);
        }

        public Variable PopOffStack()
        {
            return stack.Pop();
        }

        public Variable PeekAtStack()
        {
            return stack.Peek();
        }

        public Variable Operate(IInstruction instruction)
        {
            if(stack.Count < instruction.NumberOfConsumingArguments){
                throw new InvalidOperationException(
                    string.Format("We have only {0} elements on the stack, {1}-ary instrcution given.",
                        stack.Count, instruction.NumberOfConsumingArguments
                    )
                );
            }

            var args = new List<Variable>(instruction.NumberOfConsumingArguments);
            for(int i = 0; i < instruction.NumberOfConsumingArguments; ++i)
                args.Add(stack.Pop());

            args.Reverse();
            var results = instruction.Operate(args);
            if(results != null){
                foreach(var result in results)
                    stack.Push(result);
            }

            return (stack.Count != 0) ? stack.Peek() : null;
        }
    }
}

