namespace Wizards_and_Warriors;

class Program
{
    static void Main(string[] args)
    {
        Warrior warrior = new Warrior();
        Console.WriteLine(warrior.ToString());
        Console.WriteLine(warrior.Vulnerable());
    }
    
    abstract class Character
    {
        private readonly string _characterType;
        protected Character(string characterType)
        {
            // throw new NotImplementedException("Please implement the Character() constructor");
            this._characterType = characterType;
        }

        public abstract int DamagePoints(Character target);

        public virtual bool Vulnerable()
        {
            //throw new NotImplementedException("Please implement the Character.Vulnerable() method");
            return false;
        }

        public override string ToString()
        {
            //throw new NotImplementedException("Please implement the Character.ToString() method");
            return $"Character is a {this._characterType}";
        }
    }

    class Warrior : Character
    {
        public Warrior() : base("Warrior")
        {
        }

        public override int DamagePoints(Character target)
        {
            return target.Vulnerable() ? 10 : 6;
        }
    }

    class Wizard : Character
    {
        private bool _spellPrepared = false;
        public Wizard() : base("Wizard")
        {
        }

        public override bool Vulnerable()
        {
            return !_spellPrepared;
        }
        public override int DamagePoints(Character target)
        {
            return this._spellPrepared ? 12 : 3;
        }

        public void PrepareSpell()
        {
            _spellPrepared = true;
        }
    }
}