using MobileApp.ViewModels;
using Newtonsoft.Json.Serialization;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml;

namespace MobileApp.Services
{
    public class UserService
    {
        private int uId;
        public void SaveUserId(int userId)
        {
            uId = userId;
            
        }
        public int GetUserId()
        {
            return uId;
        }
    }
}
