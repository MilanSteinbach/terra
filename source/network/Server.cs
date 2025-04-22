using Godot;
using terra.player;
using terra.world;

namespace terra.network;

public partial class Server : Node
{
    [Export] private World _world;
    
    public override void _EnterTree()
    {
        SetMultiplayerAuthority(PeerId.Server);

        if (!IsMultiplayerAuthority())
        {
            return;
        }

        NetworkManager.PeerConnected += NetworkManager_PeerConnected;
        NetworkManager.PeerDisconnected += NetworkManager_PeerDisconnected;
    }

    public override void _ExitTree()
    {
        NetworkManager.PeerConnected -= NetworkManager_PeerConnected;
        NetworkManager.PeerDisconnected -= NetworkManager_PeerDisconnected;
        
        // TODO: Daten speichern
    }

    private void NetworkManager_PeerConnected(long peerId)
    {
        PackedScene playerScene = ResourceLoader.Load<PackedScene>("res://scenes/player/player.tscn");

        Player player = playerScene.Instantiate<Player>();
        player.Name = peerId.ToString();
        AddChild(player);
        
        _world.RegisterPlayer(player);
    }

    private void NetworkManager_PeerDisconnected(long peerId)
    {
        // TODO: Player löschen, daten speichern?
    }
}