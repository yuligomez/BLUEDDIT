using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerLogicInterface
{
    public interface IFileLogic
    {
        List<File> GetFiles();
        File GetFileByName(string name);
        string AddFile(File file);
        Response AssosiateFileToPost(string fileName, string postName);
        List<File> GetFilteredFiles(FileFilter filter);
    }
}
