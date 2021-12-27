using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class ErrorModel
    {
        public string ErrorDescription { get; set; }
        public ErrorModel (String error)
        {
            this.ErrorDescription = error;
        }
    }
}
