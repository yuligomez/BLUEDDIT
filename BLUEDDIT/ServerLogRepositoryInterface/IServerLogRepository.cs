using System;
using System.Collections.Generic;
using Domain;

namespace ServerLogRepositoryInterface
{
    public interface IServerLogLogicRepository
    {
        void AddLog(Log log);
        List<Log> GetInfoLogs();
        List<Log> GetWarningLogs();
    }
}
