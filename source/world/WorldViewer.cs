using System.Collections.Generic;
using Godot;

namespace terra.world;

public partial class WorldViewer : Node3D
{
    private HashSet<Vector3I> _receivedCoords;
}