using Malwis.General.Texts.Validators;

namespace MalwisTest;

public class EmailValidatorTest
{
    [Theory]
    [InlineData("testmail@gmail.com",true)]
    [InlineData("randomtext",false)]
    [InlineData("invalid email@test.com",false)]
    [InlineData("text with email@test.com somewhere",false)]
    [InlineData("valid.abc-mail@test.com",true)]
    public void EmailValidator_IsMail(string email, bool expectedResult)
    {
        bool actual = EmailValidator.IsEmail(email);

        Assert.Equal(expectedResult, actual);
    }
}
