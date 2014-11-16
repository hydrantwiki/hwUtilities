using System;
using HydrantWiki.Library.Constants;
using HydrantWiki.Library.Managers;
using TreeGecko.Library.Common.Enums;
using TreeGecko.Library.Net.Enums;
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
                        if (_args.Length >= 4)
                        {
                            AddUser(_args[1], _args[2], _args[3]);
                        }
                        break;
                    case "dumphydrants":
                        if (_args.Length >= 2)
                        {
                            DumpHydrants(_args[1]);
                        }
                        break;
                    case "buildcannedemail":
                        BuildEmail();
                        break;
                    default:
                        Console.WriteLine("No action requested");
                        break;
                }
            }
        }

        private static void BuildEmail()
        {
            HydrantWikiManager hwm = new HydrantWikiManager();

            CannedEmail resetPasswordEmail = new CannedEmail
            {
                Active = true,
                BodyType = EmailBodyType.HTML,
                Description = "Sent when a user needs to have their password reset",
                From = "noreply@hydrantwiki.com",
                Guid = new Guid("5a65557f-9428-4d8b-a76b-b0698bd3194f"),
                Name = "Reset Password Email",
                ReplyTo = "noreply@hydrantwiki.com",
                Subject = "HydrantWiki Password Reset",
                To = "[[EmailAddress]]",
                Body =
                    "<p>We have recieved a request to reset your password to the HydrantWiki system.</p><p>Your username is [[Username]].</p><p>Your password has been changed to [[Password]].</p><p>Thanks for helping to build an open database of hydrants.</p>"
            };
            hwm.Persist(resetPasswordEmail);

            CannedEmail emailAddressValidateEmail = new CannedEmail
            {
                Active = true,
                BodyType = EmailBodyType.HTML,
                Description = "Sent when a user needs to verify their email address",
                From = "noreply@hydrantwiki.com",
                Guid = new Guid("a12b8c41-f409-474f-bd91-2ffbb209c727"),
                Name = "Validate Email Address",
                ReplyTo = "noreply@hydrantwiki.com",
                Subject = "HydrantWiki Email Validation",
                To = "[[EmailAddress]]",
                Body =
                    "<p>Please click the following link to complete your setup as a HydrantWiki user. <a href=\"[[SystemUrl]]/rest/emailvalidation/[[ValidationText]]\">Validate my Email</a></p>"
            };
            hwm.Persist(emailAddressValidateEmail);
        }

        private static void DumpHydrants(string _filename)
        {
            
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
