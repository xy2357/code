using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Face_ID_2._0
{
    internal class Program
    {
        static void Main(string[] args)
        {
        }
    }

    public class FacialFeatures
    {
        public string EyeColor { get; }
        public decimal PhiltrumWidth { get; }

        public FacialFeatures(string eyeColor, decimal philtrumWidth)
        {
            EyeColor = eyeColor;
            PhiltrumWidth = philtrumWidth;
        }
        // TODO: implement equality and GetHashCode() methods
    }

    public class Identity
    {
        public string Email { get; }
        public FacialFeatures FacialFeatures { get; }

        public Identity(string email, FacialFeatures facialFeatures)
        {
            Email = email;
            FacialFeatures = facialFeatures;
        }
        // TODO: implement equality and GetHashCode() methods

        public bool Equailty(string email, FacialFeatures facialFeatures)
        {
            if (this.Email == email && this.FacialFeatures == facialFeatures)
            {
                return true;
            }
            return false;
        }
    }

    public class Authenticator
    {

        Dictionary<int, Identity> identities = new Dictionary<int, Identity>();

        public static bool AreSameFace(FacialFeatures faceA, FacialFeatures faceB)
        {
            if (faceA.EyeColor == faceB.EyeColor && faceA.PhiltrumWidth == faceB.PhiltrumWidth)
            {
                return true;
            }
            return false;
        }

        public bool IsAdmin(Identity identity)
        {
            if (identity.Email == "admin@exerc.ism" 
                && identity.FacialFeatures.EyeColor == "green" 
                && identity.FacialFeatures.PhiltrumWidth == 0.9m)
            {
                return true;
            }
            return false;
        }

        public bool Register(Identity identity)
        {
            if (!IsRegistered(identity))
            {
                identities[identities.GetHashCode()] = identity;
                return true;
            }
            return false;
        }

        public bool IsRegistered(Identity identity)
        {
            if (identities.ContainsKey(identity.GetHashCode()))
            {
                return true;
            }
            return false;
        }

        public static bool AreSameObject(Identity identityA, Identity identityB)
        {
            if (identityA.GetHashCode() == identityB.GetHashCode())
            {
                return true;
            }
            return false;
        }
    }

}
