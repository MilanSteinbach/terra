#define DEBUG_ENABLED

using Godot;
using terra.network;

namespace terra.debug;

public partial class DebugManager : Node
{
#if DEBUG_ENABLED
    public override void _Ready()
    {
        if (OS.HasFeature("client"))
        {
            NetworkManager.Instance.CreateClient("localhost");
        }
        else
        {
            NetworkManager.Instance.CreateHost();
        }

        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _Process(double delta)
    {
        if (Input.IsKeyPressed(Key.Escape))
        {
            GetTree().Quit();
        }
    }
#endif
}