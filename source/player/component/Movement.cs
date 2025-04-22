using Godot;

namespace terra.player.component;

public partial class Movement : PlayerComponent
{
    public override void _Process(double delta)
    {
        if (!IsMultiplayerAuthority())
        {
            return;
        }
        
        FreeFlight();
    }
    
    private void FreeFlight()
    {
        Vector2 moveInput = Input.GetVector("player_move_left", "player_move_right", "player_move_forward", "player_move_backward");
        Vector3 direction = (Player.Transform.Basis * new Vector3(moveInput.X, 0, moveInput.Y)).Normalized();
        
        Player.Velocity = direction * 10;
    }
}