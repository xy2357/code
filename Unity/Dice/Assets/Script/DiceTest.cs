using UnityEngine;

public class DiceTest : MonoBehaviour
{

    public DiceDice dice;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            dice.RollRandom();
        }
    }
}
