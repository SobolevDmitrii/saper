using System.Collections.Generic;

namespace Saper.GameUnits;

public class Unit
{
    public bool _bomb { get; set; }
    public List<int> group { get; set; } = new ();
    public int countBombs { get; set; } = 0;

    public Unit(bool bomb)
    {
        _bomb = bomb;
    }
}