using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text;

/*
 * Link to Azure function
 * https://csce496hw.azurewebsites.net/api/ClassTestFunction?code=i1OfTSR5Bcb7Hz531VLQ0sfPYrzN2iwxdy3QoO7IMnI5lAnxiPaNRw==
 */
namespace Homework2
{
    public class StackCalculator
    {
        Stack<decimal> stack = new Stack<decimal>();
        StringBuilder result = new StringBuilder();
        bool error = false;

        public string Calculate(string[] commands)
        {

            foreach (var command in commands)
            {
                var splits = command.Split(' ');

                if (error)
                {
                    return result.ToString();
                }

                if (splits.Length > 0)
                {
                    switch (splits[0])
                    {
                        case "EXIT":
                            return result.ToString();
                        case "PUSH":
                            Push(splits);
                            break;
                        case "PRINT":
                            Print();
                            break;
                        case "ADD":
                            Add();
                            break;
                        case "SUBTRACT":
                            Subtract();
                            break;
                        case "MULTIPLY":
                            Multiply();
                            break;
                        case "DIVIDE":
                            Divide();
                            break;
                        default:
                            result.AppendLine("Error: Unknown command");
                            error = true;
                            break;
                    }
                }
            }
            return result.ToString();
        }

        void Push(string[] splits)
        {
            if (splits.Length > 1)
            {
                var number = decimal.Parse(splits[1]);
                if (stack.Count < 10)
                {
                    stack.Push(number);
                }
                else
                {
                    result.AppendLine("Error: Stack full");
                    error = true;
                }
            }
        }

        void Print()
        {
            if (stack.Count > 0)
            {
                result.AppendLine(stack.Peek().ToString());
            }
        }

        void Add()
        {
            if (stack.Count > 1)
            {
                var numberOne = stack.Pop();
                var numberTwo = stack.Pop();

                stack.Push(numberOne + numberTwo);
            }
            else
            {
                result.AppendLine("Error: stack must be more that 1");
                error = true;
            }
        }

        void Subtract()
        {
            if (stack.Count > 1)
            {
                var numberOne = stack.Pop();
                var numberTwo = stack.Pop();

                stack.Push(numberOne - numberTwo);
            }
            else
            {
                result.AppendLine("Error: stack must be more that 1");
                error = true;
            }
        }

        void Multiply()
        {
            if (stack.Count > 1)
            {
                var numberOne = stack.Pop();
                var numberTwo = stack.Pop();

                stack.Push(numberOne * numberTwo);
            }
            else
            {
                result.AppendLine("Error: stack must be more that 1");
                error = true;
            }
        }

        void Divide()
        {
            if (stack.Count > 1)
            {
                var numberOne = stack.Pop();
                var numberTwo = stack.Pop();

                stack.Push(numberOne / numberTwo);
            }
            else
            {
                result.AppendLine("Error: stack must be more that 1");
                error = true;
            }
        }

    }


    public static class ClassTestFunction
    {
        [FunctionName("ClassTestFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var lines = new List<string>();

            using (var reader = new StreamReader(req.Body))
            {
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    Console.WriteLine(line);
                    lines.Add(line);
                }
            }

            var calculator = new StackCalculator();
            var response = calculator.Calculate(lines.ToArray());

            return new OkObjectResult(response);
        }
    }
}
