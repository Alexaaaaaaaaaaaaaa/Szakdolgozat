using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Threading.Tasks;
using MobileApp.Models;
using System.Runtime.ExceptionServices;

namespace MobileApp.Services
{
    public class SecurityService
    {
        #region Encryption
        public bool Encrypt(User user)
        {
            try
            {
                string appDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    dataDirectoryPath = Path.Combine(appDirectory, "Data");
                    if (!Directory.Exists(dataDirectoryPath))   
                        Directory.CreateDirectory(dataDirectoryPath);
                if (!File.Exists(Path.Combine(dataDirectoryPath, "UserData.txt")))
                    File.Create(Path.Combine(dataDirectoryPath, "UserData.txt")).Close();
                string dataFilePath = Path.Combine(dataDirectoryPath, "UserData.txt");

                using (FileStream fileStream = new FileStream(dataFilePath, FileMode.Open))
                {
                    using (Aes aes = Aes.Create())
                    {
                        byte[] key =
                        {
                            0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
                            0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16
                        };
                        aes.Key = key;
                        byte[] iv = aes.IV;
                        fileStream.Write(iv, 0, iv.Length);
                        using (CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                            {
                                streamWriter.WriteLine(user.UserId);
                                streamWriter.WriteLine(user.Email);
                                streamWriter.WriteLine(user.User_Name);
                                streamWriter.WriteLine(user.Password);
                                streamWriter.WriteLine(user.Bought);
                                streamWriter.WriteLine(user.Used);
                                streamWriter.WriteLine(user.Wasted);
                            }
                        }
                    }
                }
                if (File.ReadAllText(dataFilePath).Length != 0)
                    return true;
                else 
                    return false;
            }
            catch (Exception ex)
            {
                throw  new CryptographicException("An error occurred while performing a cryptographic operation." + ex.Message);
            }
        }
        #endregion

        #region Decryption
        public User Decrypt()
        {
            User user = new User();
            try
            {
                string appDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    dataDirectoryPath = Path.Combine(appDirectory, "Data"),
                    dataFilePath = Path.Combine(dataDirectoryPath, "UserData.txt");
                using (FileStream fileStream = new FileStream(dataFilePath, FileMode.Open))
                {
                    using (Aes aes = Aes.Create())
                    {
                        byte[] iv = new byte[aes.IV.Length];
                        int bytesToRead = aes.IV.Length;
                        int bytesRead = 0;
                        while (bytesToRead > 0)
                        {
                            int n = fileStream.Read(iv, bytesRead, bytesToRead);
                            if (n == 0)
                                break;
                            bytesRead += n;
                            bytesToRead -= n;
                        }
                        byte[] key =
                        {
                            0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
                            0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16
                        };
                        using (CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateDecryptor(key, iv), CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader(cryptoStream))
                            {
                                while (!streamReader.EndOfStream)
                                {
                                    user.UserId = Int32.Parse(streamReader.ReadLine());
                                    user.Email = streamReader.ReadLine();
                                    user.User_Name = streamReader.ReadLine();
                                    user.Password = streamReader.ReadLine();
                                    user.Bought = Int32.Parse(streamReader.ReadLine());
                                    user.Used = Int32.Parse(streamReader.ReadLine());
                                    user.Wasted = Int32.Parse(streamReader.ReadLine());
                                }
                                return user;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CryptographicException("An error occurred while performing a cryptographic operation." + ex.Message);
            }
        }
        #endregion

        #region Delete
        public bool ClearUserInfo(string path)
        {
            bool isCleared = false;
            try
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open))
                {
                    fileStream.SetLength(0);
                    if (fileStream.Length == 0)
                        isCleared = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            return isCleared;
        }
        #endregion
    }
}
