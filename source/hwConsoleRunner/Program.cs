using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon.IdentityManagement.Model;
using HydrantWiki.Library.Constants;
using HydrantWiki.Library.Managers;
using HydrantWiki.Library.Objects;
using TreeGecko.Library.Common.Enums;
using TreeGecko.Library.Net.Objects;

namespace hwConsoleRunner
{
    class Program
    {
        static void Main(string[] _args)
        {
            if (_args != null
                && _args.Length > 0)
            {
                string action = _args[0].ToLower();

                switch (action)
                {
                    case "builddb":
                        BuildDB();
                        break;
                    case "adduser":
                        if (_args.Length>=4)
                        AddUser(_args[1], _args[2], _args[3]);
                        break;
                    default:
                        Console.WriteLine("No action requested");
                        break;
                }
            }
        }

        private static void AddUser(string _username, string _email, string _password)
        {
            HydrantWikiManager hwm = new HydrantWikiManager();

            HydrantWiki.Library.Objects.User user = new HydrantWiki.Library.Objects.User
            {
                UserSource = UserSources.HydrantWiki,
                UserType = UserTypes.User,
                Username = _username,
                EmailAddress = _email,
                IsVerified = true
            };
            hwm.Persist(user);

            TGUserPassword userPassword = TGUserPassword.GetNew(user.Guid, _username, _password);
            hwm.Persist(userPassword);
        }

        private static void BuildDB()
        {
            HydrantWikiStructureManager hwsm = new HydrantWikiStructureManager();
            hwsm.BuildDB();
        }
    }
}
