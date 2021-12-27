using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerRepositoryInterface
{
    public interface IFileRepository
    {
        List<File> GetFiles();
        File GetFileByName(string name);
        void AddFile(File file);
        void DeleteFile(File file);
    }
}
