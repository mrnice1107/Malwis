namespace EqualityGenTest;

public class PersonWithEquality : IEquatable<PersonWithEquality>
{
    public PersonWithEquality(string firstName, string lastName, int age)
    {
        FirstName = firstName;
        LastName = lastName;
        Age = age;

        PersonId = Guid.NewGuid();
    }

    public string FirstName { get; }
    public string LastName { get; }
    public int Age { get; }
    
    public Guid PersonId { get; }

    public static bool operator ==(PersonWithEquality? left, object? right) => left is not null && left.Equals(right);
    public static bool operator !=(PersonWithEquality? left, object? right) => !(left == right);

    public static bool operator ==(PersonWithEquality? left, PersonWithEquality? right) => left is not null && left.Equals(right);
    public static bool operator !=(PersonWithEquality? left, PersonWithEquality? right) => !(left == right);
    
    public bool Equals(PersonWithEquality? other) => other is not null && FirstName == other.FirstName && LastName == other.LastName && Age == other.Age;
    public override bool Equals(object? obj) => obj is PersonWithEquality casted && Equals(casted);

    public override int GetHashCode()
    {
        HashCode code = new();
        
        code.Add(FirstName);
        code.Add(LastName);
        code.Add(Age);

        return code.ToHashCode();
    }
}
