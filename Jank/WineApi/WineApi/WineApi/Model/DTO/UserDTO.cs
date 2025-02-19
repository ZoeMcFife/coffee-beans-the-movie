namespace WineApi.Model.DTO
{
    public class UserDTO
    {
        public Guid Id { get; set; }

        public string Username { get; set; }
        public string Email { get; set; }
        public bool AdminRights { get; set; }

        public static User MapDtoToUser(UserDTO userDto)
        {
            if (userDto == null) throw new ArgumentNullException(nameof(userDto));

            return new User
            {
                Id = userDto.Id,
                Username = userDto.Username,
                Email = userDto.Email,
                AdminRights = userDto.AdminRights,
            };
        }

        public static UserDTO MapUserToDto(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                AdminRights = user.AdminRights,
            };
        }
    }
}
