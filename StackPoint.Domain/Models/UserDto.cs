namespace StackPoint.Domain.Models
{
    /// <summary>
    /// Dto пользователя
    /// </summary>
    public class UserDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Patronymic { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string OrganisationName { get; set; }
    }
}
