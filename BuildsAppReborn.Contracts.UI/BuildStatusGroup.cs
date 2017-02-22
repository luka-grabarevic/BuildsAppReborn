using System.Collections.Generic;
using System.Linq;
using BuildsAppReborn.Contracts.Models;

namespace BuildsAppReborn.Contracts.UI
{
    public class BuildStatusGroup
    {
        #region Public Properties

        public List<BuildItem> AllBuildItems
        {
            get { return this.allBuildItems; }
            set
            {
                this.allBuildItems = value;
                this.allBuildItems = this.allBuildItems?.OrderBy(a => a.Build?.QueueDateTime).ToList();
            }
        }

        public IBuildDefinition BuildDefinition { get; set; }

        public BuildItem CurrentBuild => AllBuildItems.Last();

        public List<BuildItem> PreviousBuilds => AllBuildItems.Where(a => a != CurrentBuild).ToList();

        #endregion

        #region Private Fields

        private List<BuildItem> allBuildItems;

        #endregion
    }
}