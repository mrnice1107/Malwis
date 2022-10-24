using System.Text;

namespace EqualityGenTest;

public class EqualityPerson : IEquatable<EqualityPerson>
{
    public EqualityPerson(string firstName, string lastName, int age)
    {
        FirstName = firstName;
        LastName = lastName;
        Age = age;

        PersonId = Guid.NewGuid();
    }

    public string FirstName { get; }
    public string LastName { get; }
    public int Age { get; set; }
    
    public Guid PersonId { get; }

    private void SomeMethod()
    {
        void DoSomething<T>(T t) {}
        
        DoSomething(PersonId);
        DoSomething(FirstName);
        DoSomething(LastName);
        DoSomething(Age);
    }

    #region Equality Operators

    public static bool operator ==(EqualityPerson? left, object? right) => left is not null && left.Equals(right);
    public static bool operator !=(EqualityPerson? left, object? right) => !(left == right);

    public static bool operator ==(EqualityPerson? left, EqualityPerson? right) => left is not null && left.Equals(right);
    public static bool operator !=(EqualityPerson? left, EqualityPerson? right) => !(left == right);
    
    public bool Equals(EqualityPerson? other) => other is not null && FirstName == other.FirstName && LastName == other.LastName && Age == other.Age;
    public override bool Equals(object? obj) => obj is EqualityPerson casted && Equals(casted);

    public override int GetHashCode()
    {
        HashCode code = new();
        
        code.Add(FirstName);
        code.Add(LastName);

        return code.ToHashCode();
    }    
    #endregion

    public override string ToString()
    {
        StringBuilder builder = new();

        builder.Append(nameof(EqualityPerson));
        builder.Append(" { ");

        builder.Append(nameof(FirstName));
        builder.Append(':');
        builder.Append(FirstName);
        
        builder.Append(", ");
        
        builder.Append(nameof(LastName));
        builder.Append(':');
        builder.Append(LastName);
        
        builder.Append(", ");
        
        builder.Append(nameof(Age));
        builder.Append(':');
        builder.Append(Age);
        
        builder.Append(" }");

        return builder.ToString();
    }
}
