namespace Attack_of_the_Trolls;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }
    
    // TODO: define the 'AccountType' enum
    [Flags]
    public enum AccountType
    {
        Guest = 0,
        User = 1,
        Moderator = 2,
    }

    // TODO: define the 'Permission' enum
    [Flags]
    public enum Permission:byte
    {
        None = 0b000,
        All = 0b001,
        Read = 0b010,
        Write = 0b100,
        Delete = 0b111,
    }
    static class Permissions
    {
        public static Permission Default(AccountType accountType)
        {
            return accountType switch
            {
                AccountType.Guest => Permission.Read,
                AccountType.User => Permission.Read | Permission.Write,
                AccountType.Moderator => Permission.Read | Permission.Write | Permission.Delete,
                _ => Permission.None
            };
        }

        public static Permission Grant(Permission current, Permission grant)
        {
            return current | grant;
        }

        public static Permission Revoke(Permission current, Permission revoke)
        {
            return current & ~revoke;
        }

        public static bool Check(Permission current, Permission check)
        {
            return (current & check) == check;
        }
    }

}