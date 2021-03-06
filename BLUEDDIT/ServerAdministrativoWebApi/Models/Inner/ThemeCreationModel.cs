using System;
using System.Collections.Generic;
using System.Text;

namespace ServerAdministrativoWebApi.Models
{
    public class ThemeCreationModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Username { get; set; }

        public Theme ToEntity()
        {
            var theme = new Theme() {Name = this.Name, Description = this.Description };
            return theme;
        }
    }
}
