namespace Calculator_Conundrum;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }
    
    public static class SimpleCalculator
    {
        public static string Calculate(int operand1, int operand2, string? operation)
        {
            
            if (operation is null)
            {
                throw new ArgumentNullException(nameof(operation));
            }

            if (operation == "")
            {
                throw new ArgumentException(nameof(operation));
            }

            try
            {
                if (operation == "*")
                {
                    return $"{operand1} * {operand2} = {operand1 * operand2}";
                }
                else if (operation == "/")
                {
                    if (operand2 == 0)
                    {
                        return "Division by zero is not allowed.";
                    }

                    return $"{operand1} / {operand2} = {operand1 / operand2}";
                }
                else if (operation == "+")
                {
                    return $"{operand1} + {operand2} = {operand1 + operand2}";
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(operation));
                }
            }
            catch (DivideByZeroException)
            {
                return "Division by zero is not allowed.";
            }
        }
    }
}