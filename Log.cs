using System;
using System.IO;
using WorkerServiceTest2.Config;

namespace WorkerServiceTest2
{
    internal class Log
    {
        internal Log()
        {
           
        }
        internal void write(string message)
        {
            string encryptedMessage = encrypt(message);
            string path = Directory.GetCurrentDirectory();
            Console.WriteLine("path and file: " + Path.Combine(path, Singleton.Instance.LOGFILE));
            try
            {
                //Path.Combine() takes into consideration which OS it is combining for, so 
                File.AppendAllTextAsync(Path.Combine(path, Singleton.Instance.LOGFILE), encryptedMessage + "\n") ;
            }
            catch (IOException ioE)
            {
                //Write why it couldnt write to a file. 
                //In a file :)
            }
        }

        private string encrypt(string message)
        {
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(message);
            string encryptedMessage = Convert.ToBase64String(bytes);
            return encryptedMessage;
        }
    }
}