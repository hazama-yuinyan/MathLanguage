using System;
using System.Numerics;

namespace CalculatorCompetition.Backend.Runtime
{
    public static class BasicOperationHelper
    {
        public static int DoFactorial(int x)
        {
            int result = 1;
            for(; x > 0; --x)
                result *= x;

            return result;
        }

        public static BigInteger DoFactorial(BigInteger x)
        {
            BigInteger result = BigInteger.One;
            for(; x > 0; --x)
                result *= x;

            return result;
        }
    }
}

