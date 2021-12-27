using Domain;
using ServerRepository;
using ServerRepositoryInterface;
using ServerLogicInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ServerLogic
{
    public class FileLogic : IFileLogic
    {
        private IFileRepository fileRepository;
        private IPostRepository postRepository;
        private static readonly object locker = new object();
        private CommonLogic commonLogic;
        public FileLogic()
        {
            this.postRepository = PostRepository.GetInstance();
            this.fileRepository = FileRepository.GetInstance();
            commonLogic = new CommonLogic();
        }
        public string AddFile(File file)
        {
            Monitor.Enter(locker);
            try
            {
                File fileDb = fileRepository.GetFileByName(file.Name);
                if (fileDb == null)
                {
                    fileRepository.AddFile(file);
                    return "Archivo agregado correctamente!";
                }
                else
                {
                    return "Lo sentimos, ya existe un archivo con ese nombre.";
                }
            }
            finally
            {
                Monitor.Exit(locker);
            }
        }

        public File GetFileByName(string name)
        {
            return fileRepository.GetFileByName(name);
        }

        public List<File> GetFiles()
        {
            return fileRepository.GetFiles();
        }

        public Response AssosiateFileToPost(string fileName, string postName)
        {
            File file = GetFileByName(fileName);
            Post post = postRepository.GetPostByName(postName);
            if (file != null)
            {
                if (post != null)
                {
                    file.PostName = postName;
                    post.Files.Add(file);
                    var message = "Archivo asociado correctamente!";
                    return commonLogic.GenerateInfoResponse(message, "File");
                }
                else
                {
                    var message = "Lo sentimos, no existe un post con ese nombre.";
                    return commonLogic.GenerateWarningResponse(message, "File");
                }
            }
            else
            {
                var message = "Lo sentimos, no existe un archivo con ese nombre.";
                return commonLogic.GenerateWarningResponse(message, "File");
            }
        }

        public List<File> GetFilteredFiles(FileFilter filter)
        {
            List<File> files = fileRepository.GetFiles();
            if(filter.NameFilter == "1")
            {
                return files.OrderBy(file => file.Name).ToList();
            }
            else if(filter.SizeFilter == "1") 
            {
                return files.OrderBy(file => file.Size).ToList();
            }
            else 
            {
                return files.OrderBy(file => file.DateUploaded).ToList();
            }
        }
    }
}
