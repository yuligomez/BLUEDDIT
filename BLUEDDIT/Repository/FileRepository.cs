using Domain;
using ServerRepositoryInterface;
using System.Collections.Generic;

namespace ServerRepository
{
    public class FileRepository : IFileRepository
    {
        private static FileRepository Instance = null;
        private static readonly object ObjectLock = new object();
        private List<File> Files;

        private FileRepository()
        {
            this.Files = new List<File>();
        }

        public static FileRepository GetInstance()
        {
            if (FileRepository.Instance == null)
            {
                lock (FileRepository.ObjectLock)
                {
                    if (FileRepository.Instance == null)
                    {
                        FileRepository.Instance = new FileRepository();
                    }
                }
            }
            return FileRepository.Instance;
        }
        public File GetFileByName(string name)
        {
            File file = null;
            foreach(File fileBd in Files)
            {
                if(fileBd.Name == name)
                {
                    return fileBd;
                }
            }
            return file;
        }

        public List<File> GetFiles()
        {
            return Files;
        }

        public void AddFile(File file)
        {
            Files.Add(file);
        }

        public void DeleteFile(File file)
        {
            Files.Remove(file);
        }
    }
}