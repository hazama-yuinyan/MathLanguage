using System;

namespace CalculatorCompetition.Backend.Runtime
{
    /// <summary>
    /// Represents the type of a variable.
    /// </summary>
    [Flags]
    public enum VariableType
    {
        /// <summary>
        /// Indicates that the corresponding variable is of type integer.
        /// </summary>
        Integer = 0x01,             // 00000001

        /// <summary>
        /// Indicates that the corresponding variable is of type integer.
        /// </summary>
        Double = 0x02,              // 00000010

        /// <summary>
        /// Indicates that the corresponding variable is of type decimal.
        /// </summary>
        Decimal = 0x04,             // 00000100

        /// <summary>
        /// Indicates that the corresponding variable is of type <see cref="System.Numrics.BigInteger"/>.
        /// </summary>
        BigInteger = 0x08,          // 00001000

        /// <summary>
        /// Indicates that the corresponding variable is of type vector.
        /// </summary>
        Vector = 0x10,              // 00010000

        /// <summary>
        /// Indicates that the corresponding variable is of type matrix.
        /// </summary>
        Matrix = 0x20,              // 00100000

        /// <summary>
        /// Indicates that the corresponding variable represents a list of operations.
        /// </summary>
        ListOfOperations = 0x40     // 01000000
    }
}

