using DAL.Models;
using DTO;

namespace WebAPI.Mapper
{
    public class RoomAccessLogMapper
    {
        public static RoomAccessLogDTO toDTO(RoomAccessLog roomAccessLog)
        {
            RoomAccessLogDTO accessDTO = new RoomAccessLogDTO
            {
                Id = roomAccessLog.Id,
                RoomId = roomAccessLog.RoomId,
                GroupId = roomAccessLog?.GroupId,
                UserId = roomAccessLog.UserId,
                Info = roomAccessLog.Info
            };
            return accessDTO;
        }

        public static RoomAccessLog toDAL(RoomAccessLogDTO roomAccessLogDTO)
        {
            RoomAccessLog roomAccessLog = new RoomAccessLog
            {
                Id = roomAccessLogDTO.Id,
                RoomId = roomAccessLogDTO.RoomId,
                GroupId = roomAccessLogDTO?.GroupId,
                UserId = roomAccessLogDTO.UserId,
                Info = roomAccessLogDTO.Info
            };
            return roomAccessLog;
        }
    }
}
