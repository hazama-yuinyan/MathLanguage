using System;

namespace CalculatorCompetition.Backend.Runtime
{
    /// <summary>
    /// Represents the type of an operator.
    /// Some are defined in function-oriented way, and some are in name-oriented way.
    /// </summary>
    public enum OperatorType
    {
        None,
        Plus,
        Minus,
        Times,
        Divide,
        Dot,
        Exclamation
    }
}

