using StackPoint.Domain.Models;
using StackPoint.Domain.Services;

namespace StackPoint.Services
{
    public class UserValidator : IUserValidator
    {
        private const string RequiredFieldMessage = "Поле \"{0}\" обязательно для заполнения";
        
        public string CheckUser(UserDto dto)
        {
            if (string.IsNullOrEmpty(dto.Name))
            {
                return GetRequiredFieldMessage("Имя");
            }

            if (string.IsNullOrEmpty(dto.LastName))
            {
                return GetRequiredFieldMessage("Фамилия");
            }

            if (string.IsNullOrEmpty(dto.Phone))
            {
                return GetRequiredFieldMessage("Номер телефона");
            }

            //if (string.IsNullOrEmpty(dto.Email))
            //{
            //    return GetRequiredFieldMessage("Почта");
            //}

            return null;
        }

        private static string GetRequiredFieldMessage(string fieldName) => string.Format(RequiredFieldMessage, fieldName);
    }
}
