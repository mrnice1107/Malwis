// See https://aka.ms/new-console-template for more information

using EqualityGenTest;

Console.WriteLine("Hello, World!");

Person asl1 = new("Aslan", "Eru", 0);
Person asl2 = new("Aslan", "Eru", 0);
Person asl3 = new("Alan", "Eru", 0);

Console.WriteLine(asl1.Equals(asl2));
Console.WriteLine(asl1.Equals(asl3));
Console.WriteLine(!asl1.Equals(asl2));
Console.WriteLine(!asl1.Equals(asl3));
Console.WriteLine(asl1 == asl2);
Console.WriteLine(asl1 == asl3);
Console.WriteLine(asl1 != asl2);
Console.WriteLine(asl1 != asl3);
