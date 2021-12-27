using System;
using System.Collections.Generic;

namespace Domain
{
    public class Post
    {
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public List<Theme> Themes { get; set; }
        public List<File> Files { get; set; }
        public Post()
        {
            this.Themes = new List<Theme>();
            this.Files = new List<File>();
        }
    }
}
