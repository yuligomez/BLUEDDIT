using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAdministrativoWebApi.Models
{
    public class PostUpdateModel
    {
        public string NewName { get; set; }
        public string OldName { get; set; }
        public string Username { get; set; }

        public Post ToEntity()
        {
            var post = new Post()
            {
                Name = this.NewName,
            };
            return post;
        }
    }
}
