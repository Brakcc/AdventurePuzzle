﻿using System;

namespace Plugins.SerializedCollections.Editor.Scripts.KeyListGenerators
{
    [AttributeUsage(AttributeTargets.Class)]
    public class KeyListGeneratorAttribute : Attribute
    {
        public readonly string Name;
        public readonly Type TargetType;
        public readonly bool NeedsWindow;

        public KeyListGeneratorAttribute(string name, Type targetType, bool needsWindow = true)
        {
            Name = name;
            TargetType = targetType;
            NeedsWindow = needsWindow;
        }
    }
}