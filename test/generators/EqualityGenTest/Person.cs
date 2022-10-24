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
    }

    private void SomeMethod()
    {
        void DoSomething<T>(T t) {}
        
        DoSomething(PersonId);
        DoSomething(FirstName);
        DoSomething(LastName);
        DoSomething(Age);
    }
    
    public string FirstName { get; }
    public string LastName { get; }
    private int Age { get; set; }
    
    [EqualityIgnore]
    public Guid PersonId { get; }
}
