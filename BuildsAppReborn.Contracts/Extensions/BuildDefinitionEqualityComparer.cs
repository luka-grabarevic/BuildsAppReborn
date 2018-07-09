using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;
using BuildsAppReborn.Contracts.Models;

namespace BuildsAppReborn.Contracts.Extensions
{
    [Export(typeof(IEqualityComparer<IBuildDefinition>))]
    public class BuildDefinitionEqualityComparer : IEqualityComparer<IBuildDefinition>
    {
        public Boolean Equals(IBuildDefinition x, IBuildDefinition y)
        {
            return x.Id == y.Id && x.Project.Id == y.Project.Id;
        }

        public Int32 GetHashCode(IBuildDefinition obj)
        {
            if (obj == null)
            {
                return RuntimeHelpers.GetHashCode(null);
            }

            return obj.Id.GetHashCode() + obj.Project.Id.GetHashCode();
        }
    }
}