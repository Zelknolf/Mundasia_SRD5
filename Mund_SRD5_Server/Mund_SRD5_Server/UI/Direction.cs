using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mundasia.Objects
{
    /// <summary>
    /// Used to indicate a direction relative to the four sides and four points on a typical
    /// tile, used to refer to slopes, angles, and adjacency.
    /// </summary>
    public enum Direction
    {
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest,
        DirectionLess,
    }
}
