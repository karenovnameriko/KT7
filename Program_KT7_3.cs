using System;

namespace KT7_3
{
    public class Calculator
    {
        public static double Add(double x, double y)
        {
            Console.WriteLine($"Выполняется сложение: {x} + {y}");
            return x + y;
        }

        public static double Subtract(double x, double y)
        {
            Console.WriteLine($"Выполняется вычитание: {x} - {y}");
            return x - y;
        }

        public static double Multiply(double x, double y)
        {
            Console.WriteLine($"Выполняется умножение: {x} * {y}");
            return x * y;
        }

        public static double Divide(double x, double y)
        {
            if (y == 0)
            {
                Console.WriteLine("Ошибка: деление на ноль!");
                return double.NaN;
            }
            Console.WriteLine($"Выполняется деление: {x} / {y}");
            return x / y;
        }
    }

    class Program
    {
        static double PerformCalculation(double x, double y, Func<double, double, double> operation)
        {
            Console.WriteLine($"Числа: {x} и {y}");
            Console.WriteLine($"Операция: {operation.Method.Name}");

            double result = operation(x, y);

            Console.WriteLine($"Результат: {result}");

            return result;
        }

        static void Main()
        {
            double a = 20.0;
            double b = 5.0;

            Console.WriteLine("БАЗОВОЕ ИСПОЛЬЗОВАНИЕ");

            PerformCalculation(a, b, Calculator.Add);
            PerformCalculation(a, b, Calculator.Subtract);
            PerformCalculation(a, b, Calculator.Multiply);
            PerformCalculation(a, b, Calculator.Divide);

            Console.WriteLine("\nКОВАРИАНТНОСТЬ");

            Func<double, double, double> multiplyDelegate = Calculator.Multiply;

            Func<double, double, object> covariantWrapper = (x, y) =>
            {
                double result = Calculator.Multiply(x, y);
                return (object)$"Результат как object: {result}";
            };

            object objResult = covariantWrapper(a, b);
            Console.WriteLine($"Результат через обертку: {objResult}");

            Console.WriteLine("\nКОНТРАВАРИАНТНОСТЬ");

            static double UniversalOperation(object x, object y)
            {
                double dx = Convert.ToDouble(x);
                double dy = Convert.ToDouble(y);
                return dx * dy + 100; // Произвольная операция для демонстрации
            }

            // Создаем делегат, принимающий object
            Func<object, object, double> universalDelegate = UniversalOperation;

            // Используем универсальный делегат с double параметрами
            double universalResult = universalDelegate(a, b);
            Console.WriteLine($"Результат универсальной операции: {universalResult}");

            Console.WriteLine("\n");

            // Создаем массив операций для демонстрации
            Func<double, double, double>[] operations =
            {
            Calculator.Add,
            Calculator.Subtract,
            Calculator.Multiply,
            Calculator.Divide,
            (x, y) => x * x + y * y, // Лямбда-выражение
        };
            foreach (var operation in operations)
            {
                PerformCalculation(3, 4, operation);
            }

            // Демонстрация с групповым преобразованием методов
            Console.WriteLine("Групповое преобразование методов:");

            // Создаем делегат через групповое преобразование
            Func<double, double, double> addOperation = Calculator.Add;
            Func<double, double, double> customOperation = (x, y) => (x + y) * 2;

            PerformCalculation(10, 2, addOperation);
            PerformCalculation(10, 2, customOperation);

            Console.WriteLine("Программа завершена.");
        }
    }
}
