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
        public async Task<ActionResult<IEnumerable<RoomAccessLogDTO>>> GetRoomAccessLogs(
            int? logNumber = 10, 
            int? offset = 0, 
            string? order = "desc")
        {
            // Validate and normalize parameters
            logNumber = Math.Max(0, logNumber ?? 10);
            logNumber = logNumber == 0 ? 10 : logNumber;
            offset = Math.Max(0, offset ?? 0);
            order = (order?.ToLower() == "asc") ? "asc" : "desc";
        
            // Build base query with included relations
            var query = _context.RoomAccessLogs
                .Include(log => log.User)
                .Include(log => log.Room)
                .AsNoTracking();
        
            // Apply ordering
            query = order == "asc" 
                ? query.OrderBy(log => log.Timestamp)
                : query.OrderByDescending(log => log.Timestamp);

            // Apply pagination and execute query
            var logs = await query
                .Skip(offset.Value)
                .Take(logNumber.Value)
                .ToListAsync();
        
            // Map to DTOs
            var logsDTO = logs.Select(log => RoomAccessLogMapper.toDTO(log)).ToList();
        
            return logsDTO;
        }
    }
}
