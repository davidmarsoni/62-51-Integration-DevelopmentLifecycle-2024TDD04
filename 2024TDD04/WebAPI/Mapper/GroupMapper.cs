using DTO;
using DAL.Models;

namespace WebApi.Mapper
{
    public class GroupMapper
    {
        public static GroupDTO toDTO (Group group)
        {
            GroupDTO groupDTO = new GroupDTO
            {
                GroupId = group.Id,
                Name = group.Name,
                Acronym = group.Acronym,
                IsDeleted = group.IsDeleted
            };
            return groupDTO;
        }

        public static Group toDAL(GroupDTO groupDTO)
        {
            Group group = new Group 
            {
                Id = groupDTO.GroupId,
                Name = groupDTO.Name,
                Acronym = groupDTO.Acronym,
                IsDeleted = groupDTO.IsDeleted
            };
            return group;
        }
    }
}
