using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace CLManager
{
    public class FTPConnect
    {
        public bool usePasive = false;
        public bool useBinary = false;
        public bool showMessage = true;
        public int buffer = 2048;
        public string login = "";
        public string password = "";

        /// <summary>
        /// загрузка файла на ftp
        /// </summary>        
        public void FTPUploadFile(string fromFilePATH, string toFtp)
        {
            FileInfo fileInf = new FileInfo(fromFilePATH);
            if (!fileInf.Exists)
            {
                Console.WriteLine("Файл не найден!");
                return;
            }
            if (showMessage)
                Console.WriteLine("Копирование файла...");
            FtpWebRequest reqFTP;
            // Создаем объект FtpWebRequest
            reqFTP = (FtpWebRequest)WebRequest.Create(new Uri(toFtp));
            // Учетная запись
            reqFTP.Credentials = new NetworkCredential(login, password);
            reqFTP.KeepAlive = false;
            reqFTP.UsePassive = usePasive;
            createFolderFtp(toFtp);
            // Задаем команду на закачку
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            // Тип передачи файла
            reqFTP.UseBinary = useBinary;
            // Сообщаем серверу о размере файла
            reqFTP.ContentLength = fileInf.Length;
            // Буффер в 2 кбайт
            int buffLength = buffer;
            byte[] buff = new byte[buffLength];
            int contentLen;
            // Файловый поток
            FileStream fs = fileInf.OpenRead();
            Stream strm = reqFTP.GetRequestStream();
            // Читаем из потока по 2 кбайт
            contentLen = fs.Read(buff, 0, buffLength);
            // Пока файл не кончится
            while (contentLen != 0)
            {
                strm.Write(buff, 0, contentLen);
                contentLen = fs.Read(buff, 0, buffLength);
            }
            // Закрываем потоки
            strm.Close();
            fs.Close();
            if (showMessage)
                Console.WriteLine("Скопировано!");
        }

        /// <summary>
        /// загрузка файла с ftp
        /// </summary>        
        public void FTPLoadFile(string from, string to)
        {
            if (showMessage)
                Console.WriteLine("Копирование файла...");
            FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(from);
            reqFTP.Credentials = new NetworkCredential(login, password);
            reqFTP.KeepAlive = true;
            reqFTP.UsePassive = usePasive;
            reqFTP.UseBinary = useBinary;
            reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
            FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
            Stream stream = response.GetResponseStream();
            List<byte> list = new List<byte>();
            int b;
            while ((b = stream.ReadByte()) != -1)
                list.Add((byte)b);
            File.WriteAllBytes(to, list.ToArray());
            if (showMessage)
                Console.WriteLine("Скопировано!");
        }

        public void createFolderFtp(string to)
        {
            FtpWebResponse resp = null; ;
            try
            {
                string ftpfullpath = to;
                int i = ftpfullpath.Length - 1;
                while (i > 0)
                {
                    if (ftpfullpath[i] != '/')
                        ftpfullpath = ftpfullpath.Remove(ftpfullpath.Length - 1, 1);
                    else
                        break;
                    i--;
                }

                FtpWebRequest reqFTP = (FtpWebRequest)WebRequest.Create(ftpfullpath);
                reqFTP.Credentials = new NetworkCredential(login, password);
                reqFTP.KeepAlive = false;
                reqFTP.UseBinary = useBinary;
                reqFTP.UsePassive = usePasive;
                reqFTP.Proxy = null;
                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                resp = (FtpWebResponse)reqFTP.GetResponse();
            }
            catch (Exception)
            { }
            finally
            {
                if (resp != null)
                    resp.Close();
            }
        }

        public void saveFile(string from, string to)
        {
            var сlient = new WebClient();
            сlient.DownloadFile(from, to);
            FileInfo fileInf = new FileInfo(to);
            if (showMessage)
                Console.WriteLine("Файл " + fileInf.Name + " скачан!");
        }

        /// <summary>
        /// Просмотр файлов на ftp
        /// </summary>
        /// <param name="folder">Папка на FTP</param>
        public void showFiles(string folder)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(folder);

            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Console.WriteLine("Содержимое " + folder + ":");

            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            Console.WriteLine(reader.ReadToEnd());

            reader.Close();
            responseStream.Close();
            response.Close();
            Console.Read();
        }        
    }
}
