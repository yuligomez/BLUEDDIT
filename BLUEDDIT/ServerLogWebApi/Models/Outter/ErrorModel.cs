using System;
using System.Collections.Generic;
using System.Text;

namespace ServerLogWebApi.Models
{
    public class ErrorModel
    {
        public string ErrorDescription { get; set; }
        public ErrorModel (string error)
        {
            this.ErrorDescription = error;
        }
    }
}
