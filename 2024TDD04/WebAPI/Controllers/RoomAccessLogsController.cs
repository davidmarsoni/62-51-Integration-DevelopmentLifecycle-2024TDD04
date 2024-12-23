using DAL;
using DAL.Models;
using DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Mapper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomAccessLogsController : ControllerBase
    {
        private readonly RoomAccessContext _context;

        public RoomAccessLogsController(RoomAccessContext context)
        {
            _context = context;
        }


        // GET: api/RoomAccessLogs?logNumber=10&offset=0&order=desc
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomAccessLogDTO>>> GetRoomAccessLogsAsync([FromQuery] int? logNumber, [FromQuery] int? offset, [FromQuery] string? order)
        {
            // If the parameters are not provided, use default values
            if (logNumber == null || logNumber < 0)
            {
                logNumber = 10;
            }
            if (offset == null || offset < 0)
            {
                offset = 0;
            }
            if (order == null || (order != "asc" && order != "desc"))
            { // take the latest logs by default
                order = "desc";
            }
            // Get the logs
            IEnumerable<RoomAccessLog> logs;
            if (order == "asc"){
                logs = await _context.RoomAccessLogs
                .OrderBy(l => l.Timestamp)
                .Skip((int)offset)
                .Take((int)logNumber)
                .ToListAsync();
            } else {
                logs = await _context.RoomAccessLogs
                .OrderByDescending(l => l.Timestamp)
                .Skip((int)offset)
                .Take((int)logNumber)
                .ToListAsync();
            }

            // Map the logs to DTO
            List<RoomAccessLogDTO> logsDTO = new List<RoomAccessLogDTO>();
            foreach (var log in logs)
            {
                logsDTO.Add(RoomAccessLogMapper.toDTO(log));
            }

            return logsDTO;
        }
    }
}
