using System;

[AttributeUsage(AttributeTargets.Method)]
public class ButtonAttribute : Attribute
{
    public string ButtonName { get; }

    public ButtonAttribute(string buttonName = null)
    {
        ButtonName = buttonName;
    }
}
