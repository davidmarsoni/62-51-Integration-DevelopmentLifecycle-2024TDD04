using DTO;
using DAL.Models;

namespace WebApi.Mapper
{
    public class UserGroupMapper
    {
        public static UserGroupDTO toDTO (UserGroup userGroup,User? user,Group? group)
        {
            UserGroupDTO userGroupDTO = new UserGroupDTO
            {
                Id = userGroup.Id,  
                GroupId = userGroup.GroupId,
                Groupname = group?.Name,
                UserId = userGroup.UserId,
                Username = user?.Username
            };

            return userGroupDTO;
        }

        public static UserGroup toDAL(UserGroupDTO userGroupDTO)
        {
            UserGroup userGroup = new UserGroup
            {
                Id = userGroupDTO.Id,
                GroupId = userGroupDTO.GroupId,
                UserId = userGroupDTO.UserId
            };

            return userGroup;
        }
    }
}
