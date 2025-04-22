using System;
using Godot;

namespace terra.network;

public partial class NetworkManager : Node
{
    public static NetworkManager Instance { get; private set; }

    public static event Action<long> PeerConnected;
    public static event Action<long> PeerDisconnected;

    private readonly ENetMultiplayerPeer _multiplayerPeer = new();

    private const int Port = 5000;

    public override void _EnterTree()
    {
        if (IsInstanceValid(Instance))
        {
            Free();
            GD.PrintErr("NetworkManager Instance already exists! Freeing this one.");
        }
        else
        {
            Instance = this;
        }

        SetMultiplayerAuthority(PeerId.Server);
    }

    public override void _ExitTree()
    {
        Multiplayer.PeerConnected -= Multiplayer_PeerConnected;
        Multiplayer.PeerDisconnected -= Multiplayer_PeerDisconnected;
    }

    public void CreateHost()
    {
        CreateServer();

        PeerConnected?.Invoke(PeerId.Server);
    }

    // TODO: Überprüfen, ob bereits ein server am laufen ist
    public void CreateServer()
    {
        _multiplayerPeer.CreateServer(Port);

        Multiplayer.MultiplayerPeer = _multiplayerPeer;
        Multiplayer.PeerConnected += Multiplayer_PeerConnected;
        Multiplayer.PeerDisconnected += Multiplayer_PeerDisconnected;

        PackedScene serverScene = ResourceLoader.Load<PackedScene>("res://scenes/network/server.tscn");

        Server server = serverScene.Instantiate<Server>();
        server.Name = "Server";
        AddChild(server);
    }

    // TODO: Überprüfen, ob bereits ein client am laufen ist
    public void CreateClient(string address)
    {
        _multiplayerPeer.CreateClient(address, Port);

        Multiplayer.MultiplayerPeer = _multiplayerPeer;
    }

    private void Multiplayer_PeerConnected(long id)
    {
        PeerConnected?.Invoke(id);
    }

    private void Multiplayer_PeerDisconnected(long id)
    {
        PeerDisconnected?.Invoke(id);
    }
}