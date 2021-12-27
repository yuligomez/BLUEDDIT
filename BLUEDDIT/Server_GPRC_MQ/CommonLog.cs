using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server_GPRC_MQ
{
    public class CommonLog
    {
        public void AddLog(string userName, Response response)
        {
            RabbitMqManagmentServer rabbitMqManagment = RabbitMqManagmentServer.GetInstance();
            var log = new Log { Level = response.Level, Message = response.Message, UserName = userName, 
                Date = DateTime.Now, ObjectType = response.ObjectType };
            log.UserName = userName;
            rabbitMqManagment.SendMessage(log);
        }
    }
}
