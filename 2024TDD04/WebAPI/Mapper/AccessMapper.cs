﻿using DAL.Models;
using DTO;

namespace WebAPI.Mapper
{
    public class AccessMapper
    {
        public static AccessDTO toDTO(Access access)
        {
            AccessDTO accessDTO = new AccessDTO
            {
                Id = access.Id,
                RoomId = access.RoomId,
                GroupId = access.GroupId
            };
            return accessDTO;
        }

        public static Access toDAL(AccessDTO accessDTO)
        {
            Access access = new Access
            {
                Id = accessDTO.Id,
                RoomId = accessDTO.RoomId,
                GroupId = accessDTO.GroupId
            };
            return access;
        }
    }
}
