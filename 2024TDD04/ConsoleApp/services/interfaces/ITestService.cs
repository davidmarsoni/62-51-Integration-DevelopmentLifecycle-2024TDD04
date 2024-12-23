
using DTO;

namespace MVC.Services.Interfaces
{
    public interface ITestService
    {
        Task<bool> TestAccessAsync(RoomAccessDTO roomAccessDTO);
    }
}