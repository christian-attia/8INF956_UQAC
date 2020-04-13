using PluginBase;
using System;

namespace HelloPlugin
{
    public class HelloCommand : IUser
    {
        public string FirstName { get => "Miche"; }
        public string LastName { get => "Mich"; }
        public string Email { get => "test@email.com"; }


        public string toString()
        {
            string userInfo = $"{FirstName} -- {LastName} -- {Email}";
            Console.WriteLine(userInfo);
            return userInfo;
        }
    }
}