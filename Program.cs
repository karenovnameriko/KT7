using System;

namespace KT7_1
{
    public interface IConverter<in T, out U>
    {
        U Convert(T value);
    }
    public class StringToIntConverter : IConverter<string, int>
    {
        public int Convert(string value)
        {
            if (int.TryParse(value, out int result))
            {
                return result;
            }
            return 0;
        }
    }
    public class ObjectToStringConverter : IConverter<object, string>
    {
        public string Convert(object value)
        {
            return value?.ToString() ?? "null";
        }
    }
    public class Animal
    {
        public string Name { get; set; }
        public override string ToString() => $"Animal: {Name}";
    }

    public class Dog : Animal
    {
        public string Breed { get; set; }
        public override string ToString() => $"Dog: {Name}, Breed: {Breed}";
    }

    public class AnimalToStringConverter : IConverter<Animal, string>
    {
        public string Convert(Animal value)
        {
            return value?.ToString() ?? "null animal";
        }
    }
    public class ArrayConverter
    {
        public static U[] ConvertArray<T, U>(T[] sourceArray, IConverter<T, U> converter)
        {
            if (sourceArray == null)
                return Array.Empty<U>();

            U[] result = new U[sourceArray.Length];
            for (int i = 0; i < sourceArray.Length; i++)
            {
                result[i] = converter.Convert(sourceArray[i]);
            }
            return result;
        }

    }
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== ДЕМОНСТРАЦИЯ КОВАРИАНТНОСТИ И КОНТРАВАРИАНТНОСТИ ===\n");

            var stringToIntConverter = new StringToIntConverter();
            var objectToStringConverter = new ObjectToStringConverter();
            var animalToStringConverter = new AnimalToStringConverter();

            Console.WriteLine("1. КОВАРИАНТНОСТЬ (возвращаемые значения):");

            IConverter<Animal, string> animalStringConverter = new AnimalToStringConverter();

            IConverter<Animal, object> animalObjectConverter = animalStringConverter;

            var animal = new Animal { Name = "Мурзик" };
            string stringResult = animalStringConverter.Convert(animal);
            object objectResult = animalObjectConverter.Convert(animal);

            Console.WriteLine($"AnimalToStringConverter: {stringResult}");
            Console.WriteLine($"Как IConverter<Animal, object>: {objectResult}");
            Console.WriteLine();

            Console.WriteLine("2. КОНТРАВАРИАНТНОСТЬ (входные параметры):");

            IConverter<Animal, string> animalStringConverter2 = new AnimalToStringConverter();

            IConverter<Dog, string> dogStringConverter = animalStringConverter2;

            var dog = new Dog { Name = "Бобик", Breed = "Овчарка" };
            string animalResult = animalStringConverter2.Convert(dog);    // Dog как Animal
            string dogResult = dogStringConverter.Convert(dog);          // Dog как Dog

            Console.WriteLine($"AnimalToStringConverter с Dog: {animalResult}");
            Console.WriteLine($"Как IConverter<Dog, string>: {dogResult}");
            Console.WriteLine();

            Console.WriteLine("3. ПРАКТИЧЕСКОЕ ИСПОЛЬЗОВАНИЕ:");

            string[] stringNumbers = { "10", "20", "30", "abc", "40" };
            int[] intNumbers = ArrayConverter.ConvertArray(stringNumbers, stringToIntConverter);

            Console.WriteLine("String → Int преобразование:");
            Console.WriteLine($"Вход: [{string.Join(", ", stringNumbers)}]");
            Console.WriteLine($"Выход: [{string.Join(", ", intNumbers)}]");
            Console.WriteLine();

            object[] objects = { 123, 45.67, "hello", null};
            string[] strings = ArrayConverter.ConvertArray(objects, objectToStringConverter);

            Console.WriteLine("Object → String преобразование:");
            Console.WriteLine($"Вход: [{string.Join(", ", objects)}]");
            Console.WriteLine($"Выход: [{string.Join(", ", strings)}]");
            Console.WriteLine();

            Console.WriteLine("4. РАСШИРЕННАЯ ДЕМОНСТРАЦИЯ:");

            var animals = new Animal[]
            {
            new Animal { Name = "Животное" },
            new Dog { Name = "Шарик", Breed = "Дворняжка" }
            };

            var dogs = new Dog[]
            {
            new Dog { Name = "Рекс", Breed = "Овчарка" },
            new Dog { Name = "Люси", Breed = "Пудель" }
            };

            var converter = new AnimalToStringConverter();

            IConverter<Dog, string> dogConverter = converter;

            string[] dogStrings = ArrayConverter.ConvertArray(dogs, converter);
            Console.WriteLine("Dogs как Animals:");
            foreach (var str in dogStrings)
                Console.WriteLine($"  {str}");

            IConverter<Animal, string> animalStringConverter3 = converter;
            IConverter<Animal, object> animalObjectConverter2 = animalStringConverter3; // string → object

            object[] animalObjects = ArrayConverter.ConvertArray(animals, animalObjectConverter2);
            Console.WriteLine("\nAnimals как Objects:");
            foreach (var obj in animalObjects)
                Console.WriteLine($"  {obj} (тип: {obj.GetType()})");
        }
    }
}
