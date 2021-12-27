using System;

namespace Domain
{
    public class File
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public DateTime DateUploaded { get; set; }
        public string PostName { get; set; }
        public File()
        {
        }
    }
}
