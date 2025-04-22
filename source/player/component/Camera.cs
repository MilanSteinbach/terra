using Godot;

namespace terra.player.component;

public partial class Camera : PlayerComponent
{
    private Camera3D _mainCamera;

    private const float LookSensitivity = 0.05f;
    private const float CameraHeight = 0.75f;
    private const int Fov = 85;

    public override void Initialize(Player player)
    {
        base.Initialize(player);

        if (IsMultiplayerAuthority())
        {
            _mainCamera = new Camera3D();
            _mainCamera.Fov = Fov;
            _mainCamera.Name = $"Camera ({player.Name})";
            player.GetParent().AddChild(_mainCamera);

            // TODO: Item camera, etc.
        }
    }

    public override void _ExitTree()
    {
        if (IsMultiplayerAuthority())
        {
            _mainCamera.QueueFree();
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (!IsMultiplayerAuthority())
        {
            return;
        }

        if (@event is InputEventMouseMotion mouseMotion)
        {
            RotateCamera(mouseMotion);
            RotatePlayer(mouseMotion);
        }
    }

    private void RotateCamera(InputEventMouseMotion mouseMotion)
    {
        float rotationX = _mainCamera.RotationDegrees.X - mouseMotion.Relative.Y * LookSensitivity;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        _mainCamera.RotationDegrees = new Vector3(rotationX, Player.RotationDegrees.Y, 0);
    }

    private void RotatePlayer(InputEventMouseMotion mouseMotion)
    {
        Player.RotationDegrees = new Vector3(0, Player.RotationDegrees.Y - mouseMotion.Relative.X * LookSensitivity, 0);
    }

    public override void _Process(double delta)
    {
        if (!IsMultiplayerAuthority())
        {
            return;
        }

        UpdatePosition();
    }

    private void UpdatePosition()
    {
        _mainCamera.GlobalPosition = Player.GlobalPosition + Vector3.Up * CameraHeight;
    }
}