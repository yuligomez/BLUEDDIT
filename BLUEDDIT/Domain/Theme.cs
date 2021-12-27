using System;
using System.Collections.Generic;

namespace Domain
{
    public class Theme
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Post> Posts { get; set; }

        public Theme()
        {
            this.Posts = new List<Post>();
        }
        
    }
}
