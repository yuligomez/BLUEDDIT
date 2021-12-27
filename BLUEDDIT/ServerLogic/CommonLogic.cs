using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerLogic
{
    public class CommonLogic
    {
        public Response GenerateInfoResponse(string message, string objectType)
        {
            return new Response() { Level = "[info]", Message = message, ObjectType = objectType };
        }

        public Response GenerateWarningResponse(string message, string objectType)
        {
            return new Response() { Level = "[warning]", Message = message, ObjectType = objectType };
        }
    }
}
