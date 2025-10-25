using System;
using System.Collections.Generic;

namespace KT7_2
{
    public abstract class Animal
    {
        public string Name { get; set; }
        public Animal(string name) => Name = name;
        public abstract void SayHello();
    }

    public class Dog : Animal
    {
        public Dog(string name) : base(name) { }
        public override void SayHello() => Console.WriteLine($"{Name}: Гав-гав!");
        public void Fetch() => Console.WriteLine($"{Name} приносит палочку!");
    }

    public class Cat : Animal
    {
        public Cat(string name) : base(name) { }
        public override void SayHello() => Console.WriteLine($"{Name}: Мяу-мяу!");
        public void Purr() => Console.WriteLine($"{Name} мурлычет: Мррррр...");
    }

    class Program
    {
        public static void ProcessAnimals(List<Animal> animals, Action<Animal> action)
        {
            foreach (var animal in animals)
                action(animal);
            Console.WriteLine();
        }

        static void Main()
        {
            var animals = new List<Animal>
        {
            new Dog("Бобик"), new Cat("Мурзик"), new Dog("Рекс"), new Cat("Васька")
        };

            var dogs = new List<Dog> { new Dog("Шарик"), new Dog("Полкан") };
            var cats = new List<Cat> { new Cat("Барсик"), new Cat("Рыжик") };

            Console.WriteLine("1. БАЗОВОЕ ИСПОЛЬЗОВАНИЕ:");
            ProcessAnimals(animals, animal => animal.SayHello());

            Console.WriteLine("2. КОНТРАВАРИАНТНОСТЬ (Action<Animal> → Action<Dog>):");
            Action<Animal> animalAction = animal =>
            {
                animal.SayHello();
                if (animal is Dog dog) dog.Fetch();
            };

            Action<Dog> dogAction = animalAction;
            foreach (var dog in dogs)
                dogAction(dog);
            Console.WriteLine();

            Console.WriteLine("3. РАЗНЫЕ ТИПЫ ДЕЛЕГАТОВ:");

            Action<Animal> specialAction = animal =>
            {
                animal.SayHello();
                if (animal is Dog d) d.Fetch();
                if (animal is Cat c) c.Purr();
            };
            ProcessAnimals(animals, specialAction);

            Console.WriteLine("4. КОВАРИАНТНОСТЬ (List<Dog> → IEnumerable<Animal>):");
            IEnumerable<Animal> animalsFromDogs = dogs;
            foreach (var animal in animalsFromDogs)
                animal.SayHello();
            Console.WriteLine();

            Console.WriteLine("5. КОНТРАВАРИАНТНОСТИ:");

            Action<Animal> dogHandler = animal =>
            {
                if (animal is Dog dog)
                {
                    Console.WriteLine($"Обрабатываем собаку {dog.Name}");
                    dog.Fetch();
                }
            };

            Action<Animal> catHandler = animal =>
            {
                if (animal is Cat cat)
                {
                    Console.WriteLine($"Обрабатываем кота {cat.Name}");
                    cat.Purr();
                }
            };

            void ProcessWithAnimalAction(Action<Animal> action)
            {
                foreach (var animal in animals)
                    action(animal);
                Console.WriteLine();
            }

            ProcessWithAnimalAction(dogHandler);
            ProcessWithAnimalAction(catHandler);
        }
    }
}