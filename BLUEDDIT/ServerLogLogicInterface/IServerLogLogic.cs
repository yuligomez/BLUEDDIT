using Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.Responses;

namespace ServerLogLogicInterface
{
    public interface IServerLogLogic
    {
        void AddLog(Log log);
        Task<PaginatedResponse<Log>> GetLogsByUsernameAsync(string username, int page, int pageSize);
        Task<PaginatedResponse<Log>> GetLogsByTypeAsync(string objectType, int page, int pageSize);
        Task<PaginatedResponse<Log>> GetAllLogs(int page, int pageSize);
        Task<List<Log>> GetAllInfoLogsAync();
        Task<List<Log>> GetAllWarningLogsAsync();
        Task<PaginatedResponse<Log>> GetEntityTypeLogsAsync(string entityType, int page, int pageSize);
        Task<PaginatedResponse<Log>> GetLogsByDateRangeAsync(DateTime startDate, DateTime endDate, int page, int pageSize);
    }
}
