using System;
namespace Domain
{
    public class Response
    {
        public string Level { get; set; }
        public string Message { get; set; }
        public string Client { get; set; }
        public string ObjectType { get; set; }

        public Response()
        {
        }
    }
}
