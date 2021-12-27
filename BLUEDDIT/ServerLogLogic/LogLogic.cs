using Domain;
using ServerLogLogicInterface;
using ServerLogRepositoryInterface;
using ServerLogRepository;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using Domain.Helpers;
using Domain.Responses;

namespace ServerLogLogic
{
    public class LogLogic : IServerLogLogic
    {
        private readonly IServerLogLogicRepository serverLogRepository;

        public LogLogic()
        {
            serverLogRepository = LogRepository.GetInstance();
        }
        public void AddLog(Log log)
        {
            serverLogRepository.AddLog(log);
        }
   
        public async Task<PaginatedResponse<Log>> GetLogsByUsernameAsync(string username, int page = 1, int pageSize = 25)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return null;
            }
            var listInfoLog =   await Task.Run(() => serverLogRepository.GetInfoLogs());
            var listWarningLog =  await Task.Run(() => serverLogRepository.GetWarningLogs());
            var concatedList = listInfoLog.Concat(listWarningLog).ToList();
            var filteredList = concatedList.Where(elem => elem.UserName.Equals(username)).ToList();
            var filteredListResponse = GetPaginatedLogs(filteredList, page, pageSize);
            if (filteredListResponse == null)
            {
                return null;
            }
            return PaginationHelper<Log>.GeneratePaginatedResponse(pageSize, filteredListResponse.Count(), filteredListResponse);
        } 
        
        public async Task<PaginatedResponse<Log>> GetEntityTypeLogsAsync(string entityType, int page = 1, int pageSize = 25)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return null;
            }
            if (entityType.Equals("Info"))
            {
                var logs = await GetAllInfoLogsAync();
                var paginatedLogs = GetPaginatedLogs(logs, page, pageSize);
                if (paginatedLogs == null)
                {
                    return null;
                }
                return PaginationHelper<Log>.GeneratePaginatedResponse(pageSize, paginatedLogs.Count(), paginatedLogs);
            }
            else
            {
                var logs = await GetAllWarningLogsAsync();
                var paginatedLogs = GetPaginatedLogs(logs, page, pageSize);
                if (paginatedLogs == null)
                {
                    return null;
                }
                return PaginationHelper<Log>.GeneratePaginatedResponse(pageSize, paginatedLogs.Count(), paginatedLogs);
            }
        }
        
        public async Task<PaginatedResponse<Log>> GetLogsByTypeAsync(string objectType, int page = 1, int pageSize = 25)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return null;
            }
            var listInfoLog = await Task.Run(() => serverLogRepository.GetInfoLogs());
            var listWarningLog = await Task.Run(() => serverLogRepository.GetWarningLogs());
            var concatedList = listInfoLog.Concat(listWarningLog).ToList();
            var filteredList = concatedList.Where(elem => elem.ObjectType.Equals(objectType)).ToList();
            var paginatedLogs = GetPaginatedLogs(filteredList, page, pageSize);
            return PaginationHelper<Log>.GeneratePaginatedResponse(pageSize, paginatedLogs.Count(), paginatedLogs);
        }

        public async Task<PaginatedResponse<Log>> GetAllLogs(int page, int pageSize)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return null;
            }
            var listInfoLog = await Task.Run(() => serverLogRepository.GetInfoLogs());
            var listWarningLog = await Task.Run(() => serverLogRepository.GetWarningLogs());
            var allLogs = listInfoLog.Concat(listWarningLog).ToList();
            var paginatedLogs = GetPaginatedLogs(allLogs, page, pageSize);
            return PaginationHelper<Log>.GeneratePaginatedResponse(pageSize, paginatedLogs.Count(), paginatedLogs);
            
        }


        public async Task<PaginatedResponse<Log>> GetLogsByDateRangeAsync(DateTime startDate, DateTime endDate, int page = 1, int pageSize = 25)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return null;
            }
            var listInfoLog = await Task.Run(() => serverLogRepository.GetInfoLogs());
            var listWarningLog = await Task.Run(() => serverLogRepository.GetWarningLogs());
            var concatedList = listInfoLog.Concat(listWarningLog).ToList();
            var filteredList = concatedList.Where(elem => 
                elem.Date >= startDate && 
                elem.Date <= endDate).ToList();
            var paginatedLogs = GetPaginatedLogs(filteredList, page, pageSize);
            return PaginationHelper<Log>.GeneratePaginatedResponse(pageSize, paginatedLogs.Count(), paginatedLogs);
        }

        public async Task<List<Log>> GetAllWarningLogsAsync()
        {
            var listWarningLogs = await Task.Run(() => serverLogRepository.GetWarningLogs());
            return listWarningLogs;
        }

        public async Task<List<Log>> GetAllInfoLogsAync()
        {
            var listInfoLog = await Task.Run(() => serverLogRepository.GetInfoLogs());
            return listInfoLog;
        }

        public IEnumerable<Log> GetPaginatedLogs(List<Log> logs, int page, int pageSize)
        {
            int totalInfoLogs = logs.Count;
            int offset = (page - 1) * pageSize;
            if (offset > totalInfoLogs)
            {
                return new List<Log>();
            }
            if (pageSize > totalInfoLogs)
            {
                pageSize = totalInfoLogs;
            }
            var minPageSize = totalInfoLogs - ((page - 1) * pageSize);
            if (minPageSize < pageSize)
            {
                pageSize = minPageSize;
            }
            return logs.GetRange(offset, pageSize);
        }
    }
}



