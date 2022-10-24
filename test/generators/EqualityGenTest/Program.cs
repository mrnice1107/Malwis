// See https://aka.ms/new-console-template for more information

using EqualityGenTest;

Console.WriteLine("Hello, World!");

Person person1 = new("Aslan", "Eru", 0);
object person2 = new Person("Aslan", "Eru", 0);
object person3 = new Person("Alan", "Eru", 0);

Console.WriteLine(person1 == person2);
Console.WriteLine(person1 != person3);

//EqualsTest(person1, person2, person3);

EqualityPerson equalityPersonW1 = new("Aslan", "Eru", 0);
EqualityPerson equalityPersonW2 = new("Aslan", "Eru", 0);
EqualityPerson equalityPersonW3 = new("Alan", "Eru", 0);

//EqualsTest(equalityPersonW1, equalityPersonW2, equalityPersonW3);

void EqualsTest<T>(params T[] testCandidates) where T: class, IEquatable<T>
{
    Console.WriteLine("--- Test started ---");

    string typeName = typeof(T).Name;
    Console.WriteLine($"TestType: {typeName}");
    Console.WriteLine();
    
    for (int i = 0; i < testCandidates.Length; i++)
    {
        T testCandidate = testCandidates[i];
        Console.WriteLine($"{i}: {testCandidate}");
    }

    Console.WriteLine();
    
    for (int i = 0; i < testCandidates.Length; i++)
    {
        T atI = testCandidates[i];
        for (int j = i; j < testCandidates.Length; j++)
        {
            T atJ = testCandidates[j];
            Console.WriteLine("---");
            Console.WriteLine($"at[{i}].Equals(at[{j}]) -> {atI.Equals(atJ)}");
            Console.WriteLine($"at[{i}] == at[{j}] -> {atI == atJ}");
            Console.WriteLine($"at[{i}] != at[{j}] -> {atI != atJ}");
        }        
        Console.WriteLine();
    }

    Console.WriteLine("--- Test ended ---");
}

