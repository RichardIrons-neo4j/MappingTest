﻿ // <autogenerated />
using System;
using System.Collections.Generic;
using HydrationPrototype;
using HydrationPrototype.Interfaces;
using Neo4j.Driver;
namespace MappingTest.DemoStages.Stage2;
public partial class Person : NodeHydratableBase, INodeHydratable
{
    public void HydrateFromNode(INode node)
    {
        Name = node.Properties["name"].As<System.String>();
Born = node.Properties["born"].As<System.Int32>();
    }
}