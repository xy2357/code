class Lasagna
{
    // TODO: define the 'ExpectedMinutesInOven()' method
    public int ExpectedMinutesInOven()
    {
        int expectMinutesInoven = 40;
        return expectMinutesInoven;
    }

    // TODO: define the 'RemainingMinutesInOven()' method
    public int RemainingMinutesInOven(int hasBeenInOven)
    {
        int expectMinutesInoven = 40;
        return expectMinutesInoven - hasBeenInOven;
    }

    // TODO: define the 'PreparationTimeInMinutes()' method
    public int PreparationTimeInMinutes(int layers)
    {
        return layers * 2;
    }

    // TODO: define the 'ElapsedTimeInMinutes()' method
    public int ElapsedTimeInMinutes(int layers,int hasBeenInOven)
    {
        return layers * 2 + hasBeenInOven;
    }
}
