using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Numerics;
using CalculatorCompetition.Backend.TypeSystem;

namespace CalculatorCompetition.Backend.Runtime.Instruction
{
    /// <summary>
    /// Represents a function call operation.
    /// A function can be either a pre-defined one or user-defined one.
    /// </summary>
    public class CallOperation : IInstruction
    {
        IList<Delegate> call_targets;

        public CallOperation(IList<Delegate> targets)
        {
            call_targets = targets;
        }

        public CallOperation(LambdaExpression lambda)
        {
            call_targets = new List<Delegate>{lambda.Compile()};
        }

        #region IInstruction implementation

        public IEnumerable<Variable> Operate(IList<Variable> arguments)
        {
            Delegate real_target = null;
            // Do an easy overload resolution
            foreach(var call_target in call_targets){
                if(call_target.Method.Name == "Call"){
                    real_target = call_target;
                    break;
                }else{
                    int i = 0;
                    foreach(var param_tuple in call_target.Method.GetParameters()
                        .Zip(arguments, (a, b) => new Tuple<ParameterInfo, Variable>(a, b))){
                        if(param_tuple.Item1.ParameterType.IsAssignableFrom(param_tuple.Item2.Value.GetType()))
                            ++i;
                        else
                            break;
                    }

                    if(i == call_target.Method.GetParameters().Length){
                        real_target = call_target;
                        break;
                    }
                }
            }

            if(real_target == null){
                throw new InvalidOperationException(
                    string.Format("Can not call \"{0}\"; types mismatch.",
                        call_targets[0].Method.Name
                    )
                );
            }

            object[] real_args = (real_target.Method.Name == "Call") ? new []{arguments} as object[] : arguments.Select(x => x.Value).ToArray();
            var result = real_target.DynamicInvoke(real_args);
            var return_type = real_target.Method.ReturnType;
            if(return_type == typeof(void))
                return null;
            else if(return_type == typeof(int))
                return new []{new Variable((int)result)};
            else if(return_type == typeof(double))
                return new []{new Variable((double)result)};
            else if(return_type == typeof(BigInteger))
                return new []{new Variable((BigInteger)result)};
            else if(return_type == typeof(decimal))
                return new []{new Variable((decimal)result)};
            else if(return_type == typeof(Vector<int>))
                return new []{new Variable((Vector<int>)result)};
            else if(return_type == typeof(Vector<double>))
                return new []{new Variable((Vector<double>)result)};
            else if(return_type == typeof(Matrix<int>))
                return new []{new Variable((Matrix<int>)result)};
            else if(return_type == typeof(Matrix<double>))
                return new []{new Variable((Matrix<double>)result)};
            else
                throw new InvalidOperationException("Unknown return type!");
        }

        public int NumberOfConsumingArguments {
            get {
                return call_targets[0].Method.GetParameters().Length;
            }
        }

        #endregion
    }
}

