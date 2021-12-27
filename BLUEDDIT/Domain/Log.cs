using System;
namespace Domain
{
    public class Log
    {
        public string Level { get; set; }
        public string Message { get; set; }
        public string UserName { get; set; }
        public DateTime Date { get; set; }
        public string ObjectType { get; set; }

        public Log()
        {
            
        }
    }
}
