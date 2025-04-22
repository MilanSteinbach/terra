using Godot;
using terra.player.component;

namespace terra.player;

public partial class Player : CharacterBody3D
{
    [Export] private Node _componentsParent;
    
    public override void _EnterTree()
    {
        SetMultiplayerAuthority(int.Parse(Name));
    }

    public override void _Ready()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        foreach (Node node in _componentsParent.GetChildren())
        {
            if (node is PlayerComponent component)
            {
                component.Initialize(this);
            }
        }
    }

    public override void _Process(double delta)
    {
        if (!IsMultiplayerAuthority())
        {
            return;
        }

        MoveAndSlide();
    }
}