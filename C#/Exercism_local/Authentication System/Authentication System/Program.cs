using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication_System
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var authenticator = new Authenticator(new Identity { EyeColor = "green", Email = "admin@ex.ism" });
            IDictionary<string, Identity> devs = authenticator.GetDevelopers();
            Identity tamperedDev = new Identity { EyeColor = "grey", Email = "anders@hack.ed" };
            devs["Anders"] = tamperedDev;
        }
    }

    public class Authenticator
    {
        private class EyeColor
        {
            public const string Blue = "blue";
            public const string Green = "green";
            public const string Brown = "brown";
            public const string Hazel = "hazel";
            public const string Grey = "grey";
        }

        public Authenticator(Identity admin)
        {
            this.admin = admin;
        }

        private readonly Identity admin;

        private readonly IDictionary<string, Identity> developers
            = new Dictionary<string, Identity>
            {
                ["Bertrand"] = new Identity
                {
                    Email = "bert@ex.ism",
                    EyeColor = "blue"
                },

                ["Anders"] = new Identity
                {
                    Email = "anders@ex.ism",
                    EyeColor = "brown"
                }
            };

        public Identity Admin
        {
            get 
            { 
                return new Identity 
                { 
                    Email = admin.Email,
                    EyeColor  = admin.EyeColor,
                }; 
            }
        }

        public IDictionary<string, Identity> GetDevelopers()
        {
            return new ReadOnlyDictionary<string, Identity>(developers);
        }
    }

    public struct Identity
    {
        public string Email { get; set; }

        public string EyeColor { get; set; }
    }

}
