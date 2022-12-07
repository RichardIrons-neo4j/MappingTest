﻿using HydrationPrototype.Attributes;

namespace MappingTest;

[GenerateHydrationFromNode]
public partial class Person
{
    [HydrateFromProperty]
    public string Name { get; private set; } = string.Empty;

    [HydrateFromProperty]
    public int Born { get; private set; }
}
