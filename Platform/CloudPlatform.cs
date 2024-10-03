using Godot;
using System;

public partial class CloudPlatform : Platform
{
    public void Response()
    {
        EmitSignal("DeleteObject", this);
    }
}
