using DTO;
using DAL.Models;

namespace WebAPI.Mapper
{
    public class RoomMapper
    {
        public static RoomDTO toDTO(Room room)
        {
            RoomDTO roomDTO = new RoomDTO
            {
                Id = room.Id,
                Name = room.Name,
                RoomAbreviation = room.RoomAbreviation
            };
            return roomDTO;
        }

        public static Room toDAL(RoomDTO roomDTO)
        {
            Room room = new Room
            {
                Id = roomDTO.Id,
                Name = roomDTO.Name,
                RoomAbreviation = roomDTO.RoomAbreviation
            };
            return room;
        }
    }
}
