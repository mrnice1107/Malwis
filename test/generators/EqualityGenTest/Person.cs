using EqualityGeneratorAttributes;

namespace EqualityGenTest;

[AutoEquality]
public partial class Person
{
    public Person(string firstName, string lastName, int age)
    {
        FirstName = firstName;
        LastName = lastName;
        Age = age;

        PersonId = Guid.NewGuid();

        Print(firstName);
    }

    public string FirstName { get; }
    public string LastName { get; }
    public int Age { get; }
    
    [EqualityIgnore]
    public Guid PersonId { get; }
}
