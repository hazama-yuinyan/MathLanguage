using System;
using System.IO;
using System.Text;
using CalculatorCompetition.Backend.Runtime;
using CalculatorCompetition.Backend.TypeSystem;
using MathLanguage;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace CalculatorTest
{
    class Helper
    {
        internal static void CompareToExpected(IEnumerable<Variable> actual, IEnumerable<Variable> expected)
        {
            foreach(var pair in actual.Zip(expected,
                (a, b) => new Tuple<Variable, Variable>(a, b))){
                if(pair.Item1.TypeFlag != pair.Item2.TypeFlag){
                    Assert.Fail("Type `{0}` is expected; actual: {1}",
                        pair.Item2.TypeFlag, pair.Item1.TypeFlag
                    );
                }

                Assert.AreEqual(pair.Item2.Value, pair.Item1.Value);
            }
        }

        internal static Variable CreateVariable(dynamic value)
        {
            if(value is int)
                return new Variable((int)value);
            else if(value is double)
                return new Variable((double)value);
            else if(value is BigInteger)
                return new Variable((BigInteger)value);
            else if(value is decimal)
                return new Variable((decimal)value);
            else if(value is Vector<int>)
                return new Variable((Vector<int>)value);
            else if(value is Vector<double>)
                return new Variable((Vector<double>)value);
            else if(value is Matrix<int>)
                return new Variable((Matrix<int>)value);
            else if(value is Matrix<double>)
                return new Variable((Matrix<double>)value);
            else
                throw new ArgumentException("Unknown type!");
        }
    }

    [TestFixture]
    public class Test
    {
        [Test]
        public void Basic()
        {
            var source = @"1 + 2
1 - 2
1 * 2
1 / 2
2 ^ 3
a = 1
b = 2
a + b";

            var expected = new Variable[]{
                new Variable(3),
                new Variable(-1),
                new Variable(2),
                new Variable(0),
                new Variable(8.0),
                new Variable(3)
            };
            var bytes = Encoding.UTF8.GetBytes(source);
            using(var output = new StringWriter())
            using(var stream = new MemoryStream(bytes)){
                var parser = new Parser(new Scanner(stream));
                Parser.Output = output;
                parser.Parse();
                Console.Out.Write(output);
                Helper.CompareToExpected(Parser.Results.Reverse(), expected);
            }
        }

        [Test]
        public void Vectors()
        {
            var source = @"a = 2
u = (1 2 1)
v = (2 2 3)
a * u
u / a
-u
u + v
u - v
u * v
u . v";

            var expected = new []{
                new Variable(new Vector<int>(2, 4, 2)),
                new Variable(new Vector<int>(0, 1, 0)),
                new Variable(new Vector<int>(-1, -2, -1)),
                new Variable(new Vector<int>(3, 4, 4)),
                new Variable(new Vector<int>(-1, 0, -2)),
                new Variable(new Vector<int>(4, -1, -2)),
                new Variable(9)
            };

            var bytes = Encoding.UTF8.GetBytes(source);
            using(var output = new StringWriter())
            using(var stream = new MemoryStream(bytes)){
                var parser = new Parser(new Scanner(stream));
                Parser.Output = output;
                parser.Parse();
                Console.Out.Write(output);
                Helper.CompareToExpected(Parser.Results.Reverse(), expected);
            }
        }

        [Test]
        public void Matrices()
        {
            var source = @"k = 2
a = [1 2 3, 3 2 1, 4 5 6]
b = [3 2 1, 1 0 1, 1 2 3]
k * a
a + b
a - b
a * b
a ^ k";

            var expected = new Variable[]{
                new Variable(new Matrix<int>(3, 3, 2, 4, 6, 6, 4, 2, 8, 10, 12)),
                new Variable(new Matrix<int>(3, 3, 4, 4, 4, 4, 2, 2, 5, 7, 9)),
                new Variable(new Matrix<int>(3, 3, -2, 0, 2, 2, 2, 0, 3, 3, 3)),
                new Variable(new Matrix<int>(3, 3, 3, 4, 3, 3, 0, 1, 4, 10, 18)),
                new Variable(new Matrix<int>(3, 3, 19, 21, 23, 13, 15, 17, 43, 48, 53))
            };

            var bytes = Encoding.UTF8.GetBytes(source);
            using(var output = new StringWriter())
            using(var stream = new MemoryStream(bytes)){
                var parser = new Parser(new Scanner(stream));
                Parser.Output = output;
                parser.Parse();
                Console.Out.Write(output);
                Helper.CompareToExpected(Parser.Results.Reverse(), expected);
            }
        }
    }
}

