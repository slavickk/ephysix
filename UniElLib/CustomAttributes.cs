using System;

namespace UniElLib;

public class GUIAttribute:Attribute
{
    Type settingsType;
    public GUIAttribute(Type setType)
    {
        settingsType = setType;
    }
}

public class SensitiveAttribute : Attribute
{
    public string NameSensitive;

    public SensitiveAttribute(string name)
    {
        NameSensitive = name;
    }
}

public class AnnotationAttribute : Attribute
{
    public string Description
    {
        get { return description; }
    }

    string description;

    public AnnotationAttribute(string description)
    {
        this.description = description;
    }
}