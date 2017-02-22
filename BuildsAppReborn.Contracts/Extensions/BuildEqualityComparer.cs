﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;
using BuildsAppReborn.Contracts.Models;

namespace BuildsAppReborn.Contracts.Extensions
{
    [Export(typeof(IEqualityComparer<IBuild>))]
    public class BuildEqualityComparer : IEqualityComparer<IBuild>
    {
        #region Constructors

        [ImportingConstructor]
        public BuildEqualityComparer(IEqualityComparer<IBuildDefinition> buildDefinitionEqualityComparer)
        {
            this.buildDefinitionEqualityComparer = buildDefinitionEqualityComparer;
        }

        #endregion

        #region Implementation of IEqualityComparer<IBuild>

        public Boolean Equals(IBuild x, IBuild y)
        {
            return x.Id == y.Id && this.buildDefinitionEqualityComparer.Equals(x.Definition, y.Definition);
        }

        public Int32 GetHashCode(IBuild obj)
        {
            if (obj == null)
            {
                return RuntimeHelpers.GetHashCode(null);
            }
            return obj.Id.GetHashCode() + this.buildDefinitionEqualityComparer.GetHashCode(obj.Definition);
        }

        #endregion

        #region Private Fields

        private readonly IEqualityComparer<IBuildDefinition> buildDefinitionEqualityComparer;

        #endregion
    }
}