using NUnit.Framework;
using StackPoint.Domain.Models;

namespace StackPoint.Services.Tests
{
    [TestFixture(Author = "������� �.�.")]
    public class UserValidatorTests
    {
        private UserValidator _userValidator;

        [SetUp]
        public void Setup()
        {
            _userValidator = new UserValidator();
        }

        [Test(Description =
            "���� ��������� ��������� �� ������, ���� �� ��������� ����������� ����. ��������� ���������� ������ ���������� ������������� ����.")]
        [TestCase(null, null, null, "���� \"���\" ����������� ��� ����������")]
        [TestCase(null, "last name", "phone", "���� \"���\" ����������� ��� ����������")]
        [TestCase("", "last name", "phone", "���� \"���\" ����������� ��� ����������")]
        [TestCase("name", null, "phone", "���� \"�������\" ����������� ��� ����������")]
        [TestCase("name", "last name", null, "���� \"����� ��������\" ����������� ��� ����������")]
        public void CheckUserTest_CheckUserWithoutRequiredField_GetErrors(
            string name, string lastName, string phone, string expectedError)
        {
            var userDto = new UserDto {Name = name, LastName = lastName, Phone = phone};
            var error = _userValidator.CheckUser(userDto);

            Assert.IsNotNull(error);
            Assert.That(error, Is.EqualTo(expectedError));
        }

        [Test(Description =
            "���� ��������� null ������ ���������, ���� ��������� ��� ������������ ����. �������� - �������������� ����.")]
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