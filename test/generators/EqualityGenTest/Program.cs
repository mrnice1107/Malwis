// See https://aka.ms/new-console-template for more information

using EqualityGenTest;

Console.WriteLine("Hello, World!");

Person person1 = new("Aslan", "Eru", 0);
Person person2 = new("Aslan", "Eru", 0);
Person person3 = new("Alan", "Eru", 0);

Console.WriteLine(person1.Equals(person2));
Console.WriteLine(person1.Equals(person3));
Console.WriteLine(!person1.Equals(person2));
Console.WriteLine(!person1.Equals(person3));
Console.WriteLine(person1 == person2);
Console.WriteLine(person1 == person3);
Console.WriteLine(person1 != person2);
Console.WriteLine(person1 != person3);

PersonWithEquality personW1 = new("Aslan", "Eru", 0);
PersonWithEquality personW2 = new("Aslan", "Eru", 0);
PersonWithEquality personW3 = new("Alan", "Eru", 0);

Console.WriteLine(personW1.Equals(personW2));
Console.WriteLine(personW1.Equals(personW3));
Console.WriteLine(!personW1.Equals(personW2));
Console.WriteLine(!personW1.Equals(personW3));
Console.WriteLine(personW1 == personW2);
Console.WriteLine(personW1 == personW3);
Console.WriteLine(personW1 != personW2);
Console.WriteLine(personW1 != personW3);
