﻿ // <autogenerated />
using System;
using HydrationPrototype.Interfaces;
using Neo4j.Driver;
namespace MappingTest.DemoStages.Stage4;
public partial class DirectedRelationship : IRelationshipHydratable
{
    public void HydrateFromRelationship(IRelationship relationship, IDictionary<string, object> instances)
    {


var Director_other_end = instances[relationship.EndNodeElementId];
Director = instances[relationship.StartNodeElementId].As<MappingTest.DemoStages.Stage2.Person>();
if(Director is INodeHydratable Director_hydratable)
{
    Director_hydratable.AddRelationship<DirectedRelationship>(Director_other_end);
}


var Movie_other_end = instances[relationship.StartNodeElementId];
Movie = instances[relationship.EndNodeElementId].As<MappingTest.DemoStages.Stage2.Movie>();
if(Movie is INodeHydratable Movie_hydratable)
{
    Movie_hydratable.AddRelationship<DirectedRelationship>(Movie_other_end);
}

    }
}