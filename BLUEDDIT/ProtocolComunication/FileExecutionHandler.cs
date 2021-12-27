using System;
using System.Net.Sockets;
using System.Text;
using ProtocolComunication.Interface;
using ServerLogic;
using Domain;
using ServerLogicInterface;
using System.Threading.Tasks;

namespace ProtocolComunication
{
    public class FileExecutionHandler : IFileExcutionHandler
    {
       
        private readonly IFileHandler fileHandler;
        private readonly INetworkLogic networkLogic;
        private readonly IFileLogic fileLogic;
        private readonly IFileStremHandler fileStreamHandler;
        private readonly IPostLogic postLogic;
        private readonly IHeaderHandler headerHandler;


        public FileExecutionHandler()
        {
            fileHandler = new FileHandler();
            networkLogic = new NetworkLogic();
            fileStreamHandler = new FileStremHandler();
            fileLogic = new FileLogic();
            headerHandler = new HeaderHandler();
            postLogic = new PostLogic();
        }

        public async Task<Response> RecivedFileForPostAsync(Tuple<short, int> header, TcpClient client)
        {
            var dataLength = header.Item2;
            var data = new byte[dataLength];
            Array.Copy(await networkLogic.ReceiveDataAsync(dataLength, client), 0, data, 0, dataLength);
            string dataString = System.Text.Encoding.UTF8.GetString(data);
            string[] dataStringArray = dataString.Split("/*/");
            string fileName = dataStringArray[0];
            string fileSizeString = dataStringArray[1];
            string postName = dataStringArray[2];
            string username = dataStringArray[3];
            Post post = postLogic.GetPostByName(postName);
            int fileSize = Int32.Parse(fileSizeString);
            var parts = fileHandler.GetFileParts(fileSize);
            var offset = 0;
            var currentPart = 1;
            if (post != null)
            {
                if (fileSize <= 104857600)
                {
                    await networkLogic.CompleteSendAsync("OK", client, CommandConstants.AddFileToPost);
                    while (fileSize > offset)
                    {
                        byte[] dataSegment;
                        if (currentPart == parts)
                        {
                            var lastPartSize = fileSize - offset;
                            //recibo el segmento de archivo
                            dataSegment = await networkLogic.ReceiveDataAsync(lastPartSize, client);
                            offset += lastPartSize;
                        }
                        else
                        {
                            //recibo el segmento de archivo 
                            dataSegment = await networkLogic.ReceiveDataAsync(ProtocolConstants.MaxPacketSize, client);
                            offset += ProtocolConstants.MaxPacketSize;
                        }
                        await fileStreamHandler.WriteSegmentFileAsync(fileName, dataSegment);
                        currentPart++;
                    }
                    File file = new File() { DateUploaded = DateTime.Now, Name = fileName, Size = fileSize };
                    fileLogic.AddFile(file);
                    var response = await SendDataAsync(client, fileName, postName);
                    response.Client = username;
                    return response;
                }
                else
                {
                    var message = "Lo sentimos, el archivo no puede pesar más de 100 megabytes";
                    await networkLogic.CompleteSendAsync(message, client, CommandConstants.AddFileToPost);
                    var response = new Response { Client = username, Level = "warning.*", Message = message, ObjectType = "File" };
                    return response;
                }
            }
            else
            {
                var message = "Lo sentimos, no existe un post con ese nombre.";
                await networkLogic.CompleteSendAsync(message, client, CommandConstants.AddFileToPost);
                var response = new Response { Client = username, Level = "warning.*", Message = message, ObjectType = "File" };
                return response;
            }            
        }

        public async Task<Response> SendDataAsync(TcpClient client, string fileName, string postName)
        {
            var response = fileLogic.AssosiateFileToPost(fileName, postName);
            var specialCaracters = networkLogic.CountSpecialCharacter(response.Message);
            var headerBytes = headerHandler.EncodeHeader(CommandConstants.AddFileToPost, response.Message.Length + specialCaracters);
            await networkLogic.SendAsync(headerBytes, client);
            var dataByte = Encoding.UTF8.GetBytes(response.Message);
            await networkLogic.SendAsync(dataByte, client);
            return response;
        }

        public async Task SendFileAsync(string postName, string username, string path, TcpClient tcpClient)
        {
            try
            {

                var fileSize = fileHandler.GetFileSize(path);
                var fileName = fileHandler.GetFileName(path);
                var stringFileSize = fileSize.ToString();
                var data = fileName + "/*/" + fileSize + "/*/" + postName + "/*/" + username;
                var specialCaracters = networkLogic.CountSpecialCharacter(fileSize + fileName + postName + username);
                var headerBytes = headerHandler.EncodeHeader(CommandConstants.AddFileToPost, stringFileSize.Length + 9 + fileName.Length + postName.Length + username.Length + specialCaracters);
                await networkLogic.SendAsync(headerBytes, tcpClient);
                var dataByte = Encoding.UTF8.GetBytes(data);
                await networkLogic.SendAsync(dataByte, tcpClient);

                var parts = fileHandler.GetFileParts(fileSize); //se cuentan cuantas partes se envian
                long offset = 0;
                long currentPart = 1;
                string response = await networkLogic.CompleteRecivedAsync(tcpClient);
                if (response == "OK")
                {
                    while (fileSize > offset)
                    {
                        byte[] dataToSend;
                        if (currentPart == parts)
                        {
                            var lastPartSize = fileSize - offset;
                            var intLastPartSize = (int)lastPartSize;
                            dataToSend = await fileStreamHandler.ReadSegmentFileAsync(path, offset, intLastPartSize);
                            offset += lastPartSize;
                        }
                        else
                        {
                            
                            dataToSend = await  fileStreamHandler.ReadSegmentFileAsync(path, offset, ProtocolConstants.MaxPacketSize);
                            offset += ProtocolConstants.MaxPacketSize;
                            
                        }
                        await networkLogic.SendAsync(dataToSend, tcpClient);
                        currentPart++;
                    }
                    Console.WriteLine();
                    var dataString = await networkLogic.CompleteRecivedAsync(tcpClient);
                    Console.WriteLine(dataString);
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine(response);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }
    }
}
