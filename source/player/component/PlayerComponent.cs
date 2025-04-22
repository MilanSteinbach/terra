using Godot;

namespace terra.player.component;

public abstract partial class PlayerComponent : Node
{
    protected Player Player { get; private set; }

    public virtual void Initialize(Player player)
    {
        SetMultiplayerAuthority(int.Parse(player.Name));
        
        Player = player;
    }
}