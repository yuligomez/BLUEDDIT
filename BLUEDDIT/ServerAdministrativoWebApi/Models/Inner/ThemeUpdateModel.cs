using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAdministrativoWebApi.Models
{
    public class ThemeUpdateModel
    {
        public string OldName { get; set; }
        public string NewName { get; set; }
        public string NewDescription { get; set; }
        public string Username { get; set; }

        public Theme ToEntity()
        {
            var theme = new Theme() { Name = this.NewName, Description = this.NewDescription };
            return theme;
        }
    }
}
