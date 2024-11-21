using DTO;
using DAL.Models;

namespace WebApi.Mapper
{
    public class UserMapper
    {
        public static UserDTO toDTO (User user)
        {
            UserDTO userDTO = new UserDTO
            {
                UserId = user.Id,
                Username = user.Username,
                IsDeleted = user.IsDeleted
            };
            return userDTO;
        }

        public static User toDAL(UserDTO userDTO)
        {
            User user = new User
            {
                Id = userDTO.UserId,
                Username = userDTO.Username,
                IsDeleted = userDTO.IsDeleted
            };
            return user;
        }
    }
}
