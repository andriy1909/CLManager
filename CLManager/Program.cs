using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CLManager
{
    class Program
    {
        static void showHelp()
        {
            Console.WriteLine();
            Console.WriteLine("CLManager [-v] [-p] [-bin] [-buff 2048] [-u login:password] [-PutFtp <FromPATH> <ToPATH>]");
            Console.WriteLine();
            Console.WriteLine("\t-help\t\t" + "Помощь.");
            Console.WriteLine("\t-user, -u\t\t" + "Логин и пароль (-u Login:Password).");
            Console.WriteLine("\t-putftp\t\t" + "Копировать на сервер ftp (-PutFtp <FromPATH> <ToPATH>).");
            Console.WriteLine("\t-getftp\t\t" + "Копировать с сервера ftp (-GetFtp <FromPATH> <ToPATH>).");
            Console.WriteLine("\t-buff\t\t" + "Изменить размер буфера передачи (стандартно 2048 байт)");
            Console.WriteLine("\t-p\t\t" + "Использовать пасивный метод передачи файлов.");
            Console.WriteLine("\t-bin\t\t" + "Установка режима передачи файлов в двоичном формате.");
            Console.WriteLine("\t-v\t\t" + "Скривать сообщения при передедаче файлов.");
            Console.WriteLine("\t-save\t\t" + "Сохранить файл по ссилке (-Save <URL> <TargetPATH>).");
            Console.WriteLine("\t-dir\t\t" + "Посмотреть файлы на ftp.");
            Console.WriteLine("\t-run\t\t" + "Запустить файл с командами (-run <PATH_file>).");
            Console.WriteLine("\t-V\t\t" + "Версия.");
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
                args = Console.ReadLine().Split(' ');
            List<string> commands = new List<string>();
            foreach (string item in args)
            {
                commands.Add(item);
            }

            bool isOpen = false;
            for (int i = commands.Count - 1; i >= 0; i--)
            {
                if (commands[i].Last() == '"' || isOpen)
                {
                    isOpen = true;
                    commands[i]= commands[i].Remove(commands[i].Count());
                    if(commands[i].First()=='"')
                    {
                        isOpen = false;
                        commands[i] = commands[i].Remove(0,1);
                    }
                    if(i>0)
                    {
                        commands[i + 1] = commands[i + 1] + commands[i];
                        //isOpen-falsel=mai
                            }
                    commands[i].Remove(commands[i].Count() - 1);
                }
            }

            for (int i = 0; i < commands.Count; i++)
            {
                if (commands[i].First()=='"')
                {
                    isOpen = true;
                    commands[i].Remove(0, 1);
                }
                if (isOpen)
                {
                    if (commands[i].Last() == '"')
                    {
                        isOpen = false;
                        commands[i].Remove(commands[i].Count() - 1);
                    }
                }
            }

            if (commands.Contains("-sendEmail") || commands.Contains("-sendemail"))
            {
                try
                {
                    SendEmail mail = new SendEmail();
                    for (int i = 0; i < commands.Count; i++)
                    {

                        switch (commands[i])
                        {
                            case "-to":
                                mail.to = commands[i + 1];
                                break;
                            case "-from":
                                mail.from = commands[i + 1];
                                break;
                            case "-pw":
                                mail.password = commands[i + 1];
                                break;
                            case "-server":
                                mail.server = commands[i + 1];
                                break;
                            case "-subject":
                                mail.subject = commands[i + 1];
                                break;
                            case "-body":
                                mail.body = commands[i + 1];
                                break;
                            case "-attach":
                                mail.attach = commands[i + 1];
                                break;
                            case "-fromName":
                                mail.fromName = commands[i + 1];
                                break;
                            case "-toName":
                                mail.toName = commands[i + 1];
                                break;
                            case "-port":
                                mail.port = int.Parse(commands[i + 1]);
                                break;
                            case "-login":
                                mail.login = commands[i + 1];
                                break;
                            case "-timeout":
                                mail.timeout = int.Parse(commands[i + 1]);
                                break;
                        }
                    }
                    mail.Send();
                }
                catch (Exception error)
                {
                    Console.WriteLine("Возникла ошибка, " + error.Message);
                    Console.WriteLine("Используйте команду -help для помощи.");
                }

            }
            else
            {
                FTPConnect ftp = new FTPConnect();
                for (int i = 0; i < commands.Count; i++)
                {
                    try
                    {
                        switch (commands[i])
                        {
                            case "-help":
                                if (i == 0)
                                {
                                    showHelp();
                                    Console.WriteLine(commands[i]);
                                    return;
                                }
                                break;
                            case "-V":
                                if (i == 0)
                                {
                                    Console.WriteLine("v 1.0.1");
                                    return;
                                }
                                break;
                            case "-u":
                            case "-user":
                                string[] str = commands[i + 1].Split(':');
                                ftp.login = str[0];
                                ftp.password = str[1];
                                i++;
                                break;
                            case "-putftp":
                                ftp.FTPUploadFile(commands[i + 1], commands[i + 2]);
                                i += 2;
                                break;
                            case "-getftp":
                                ftp.FTPLoadFile(commands[i + 1], commands[i + 2]);
                                i += 2;
                                break;
                            case "-run":
                                string file = commands[i + 1];
                                file.Insert(2, "\\");
                                if (!File.Exists(file))
                                {
                                    Console.WriteLine("Файл " + file + " не найден");
                                }
                                string[] fileContents = File.ReadAllLines(file);
                                commands.Clear();
                                for (int j = 0; j < fileContents.Length; j++)
                                {
                                    string[] fileCommands = fileContents[j].Split(' ');
                                    foreach (string item in fileCommands)
                                    {
                                        commands.Add(item);
                                    }
                                }
                                i = -1;
                                break;
                            case "-save":
                                ftp.saveFile(commands[i + 1], commands[i + 2]);
                                i += 2;
                                break;
                            case "-p":
                                ftp.usePasive = true;
                                break;
                            case "-bin":
                                ftp.useBinary = true;
                                break;
                            case "-buff":
                                ftp.buffer = int.Parse(commands[i + 1]);
                                if (ftp.buffer <= 0)
                                    ftp.buffer = 2048;
                                break;
                            case "-v":
                                ftp.showMessage = false;
                                break;
                            case "-dir":
                                ftp.showFiles(commands[i + 1]);
                                break;
                            default:
                                Console.WriteLine("Команда '" + commands[i] + "' не найдена");
                                break;
                        }
                    }
                    catch (Exception error)
                    {
                        Console.WriteLine("Возникла ошибка, " + error.Message);
                        Console.WriteLine("Используйте команду -help для помощи.");
                    }
                }                
            }

        }
    }
}