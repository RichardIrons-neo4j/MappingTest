﻿ // <autogenerated />
using System;
using HydrationPrototype.Interfaces;
using Neo4j.Driver;
namespace MappingTest;
public partial class ActedInRelationship : IRelationshipHydratable
{
    public void HydrateFromRelationship(IRelationship relationship, IDictionary<string, object> instances)
    {
Roles = relationship.Properties["roles"].As<System.Collections.Generic.List<System.String>>();

var Actor_other_end = instances[relationship.EndNodeElementId];
Actor = instances[relationship.StartNodeElementId].As<MappingTest.Person>();
if(Actor is INodeHydratable Actor_hydratable)
{
    Actor_hydratable.AddRelationship<ActedInRelationship>(Actor_other_end);
}


var Movie_other_end = instances[relationship.StartNodeElementId];
Movie = instances[relationship.EndNodeElementId].As<MappingTest.Movie>();
if(Movie is INodeHydratable Movie_hydratable)
{
    Movie_hydratable.AddRelationship<ActedInRelationship>(Movie_other_end);
}

    }
}