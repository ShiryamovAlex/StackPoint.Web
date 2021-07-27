using NUnit.Framework;
using StackPoint.Domain.Models;

namespace StackPoint.Services.Tests
{
    [TestFixture(Author = "Ширямов А.И.")]
    public class UserValidatorTests
    {
        private UserValidator _userValidator;

        [SetUp]
        public void Setup()
        {
            _userValidator = new UserValidator();
        }

        [Test(Description =
            "Тест получения сообщения об ошибке, если не заполнено обязательно поле. Сообщение возвращает первое попавшееся незаполненное поле.")]
        [TestCase(null, null, null, "Поле \"Имя\" обязательно для заполнения")]
        [TestCase(null, "last name", "phone", "Поле \"Имя\" обязательно для заполнения")]
        [TestCase("", "last name", "phone", "Поле \"Имя\" обязательно для заполнения")]
        [TestCase("name", null, "phone", "Поле \"Фамилия\" обязательно для заполнения")]
        [TestCase("name", "last name", null, "Поле \"Номер телефона\" обязательно для заполнения")]
        public void CheckUserTest_CheckUserWithoutRequiredField_GetErrors(
            string name, string lastName, string phone, string expectedError)
        {
            var userDto = new UserDto {Name = name, LastName = lastName, Phone = phone};
            var error = _userValidator.CheckUser(userDto);

            Assert.IsNotNull(error);
            Assert.That(error, Is.EqualTo(expectedError));
        }

        [Test(Description =
            "Тест получения null вместо сообщения, если заполнены все обязательные поля. Отчество - необязательное поле.")]
        [TestCase("name", "last name", "patronumic", "phone")]
        [TestCase("name", "last name", null, "phone")]
        public void CheckUserTest_CheckFilledUser_GetNullError(
            string name, string lastName, string patronumic, string phone)
        {
            var userDto = new UserDto {Name = name, LastName = lastName, Patronymic = patronumic, Phone = phone};

            Assert.IsNull(_userValidator.CheckUser(userDto));
        }
    }
}