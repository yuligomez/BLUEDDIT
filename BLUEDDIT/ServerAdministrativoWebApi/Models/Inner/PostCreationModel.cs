using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAdministrativoWebApi.Models
{
    public class PostCreationModel
    {
        public string Name { get; set; }
        public string ThemeName { get; set; }
        public string Username { get; set; }

        public Post ToEntity()
        {
            var post = new Post { Name = this.Name };
            return post;
        }
    }
}
