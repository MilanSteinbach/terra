using Godot;
using terra.network;
using terra.player;

namespace terra.world;

public partial class World : Node
{
    public override void _EnterTree()
    {
        SetMultiplayerAuthority(PeerId.Server);
    }

    public void RegisterPlayer(Player player)
    {
        
    }
}