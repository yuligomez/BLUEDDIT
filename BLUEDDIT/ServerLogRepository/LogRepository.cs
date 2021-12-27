using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using ServerLogRepositoryInterface;

namespace ServerLogRepository
{
   
    public class LogRepository : IServerLogLogicRepository
    {
        public List<Log> InfoLogs { get; set; }
        public List<Log> WarningLogs { get; set; }
        private static LogRepository Instance = null;
        private static readonly object ObjectLock = new object();

        private LogRepository()
        {
            InfoLogs = new List<Log>();
            WarningLogs = new List<Log>();
        }

        public static LogRepository GetInstance()
        {
            if (LogRepository.Instance == null)
            {
                lock (LogRepository.ObjectLock)
                {
                    if (LogRepository.Instance == null)
                    {
                        LogRepository.Instance = new LogRepository();
                    }
                }
            }
            return LogRepository.Instance;
        }


        public void AddLog(Log log)
        {
            if (log.Level.Equals("[info]"))
            {
                InfoLogs.Add(log);
            }
            else
            {
                WarningLogs.Add(log);
            }
        }

        public List<Log> GetInfoLogs()
        {
            return InfoLogs;
           
        }

        public List<Log> GetWarningLogs()
        {
            return WarningLogs;
        }
        

    }
}
