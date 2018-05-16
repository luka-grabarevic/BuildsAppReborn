using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;
using BuildsAppReborn.Contracts.Models;

namespace BuildsAppReborn.Contracts.Extensions
{
    [Export(typeof(IEqualityComparer<IPullRequest>))]
    public class PullRequestEqualityComparer : IEqualityComparer<IPullRequest>
    {
        #region Implementation of IEqualityComparer<IPullRequest>

        public Boolean Equals(IPullRequest x, IPullRequest y)
        {
            return x.Id == y.Id;
        }

        public Int32 GetHashCode(IPullRequest obj)
        {
            if (obj == null)
            {
                return RuntimeHelpers.GetHashCode(null);
            }

            return obj.Id.GetHashCode();
        }

        #endregion
    }
}