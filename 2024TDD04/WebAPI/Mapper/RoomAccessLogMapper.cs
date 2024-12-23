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
                Room = roomAccessLog.Room,
                GroupId = roomAccessLog?.GroupId,
                UserId = roomAccessLog.UserId,
                TimeStamp = roomAccessLog.Timestamp,
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
                Room = roomAccessLogDTO.Room,
                GroupId = roomAccessLogDTO?.GroupId,
                UserId = roomAccessLogDTO.UserId,
                Info = roomAccessLogDTO.Info
            };
            return roomAccessLog;
        }
    }
}
