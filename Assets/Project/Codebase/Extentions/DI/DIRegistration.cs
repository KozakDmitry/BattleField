using System;

internal class DIRegistration
{
    public RegisterMode mode;

    public object Instance { get; set; }
    public DIRegistration(object instance,RegisterMode mode )
    {
        this.mode = mode;
        Instance = instance;
    }
}
