using System;
using System.Collections.Generic;
using System.Linq;
using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Infrastructure;

namespace BuildsAppReborn.Contracts.UI
{
    public class BuildStatusGroup : ViewModelBase
    {
        #region Constructors

        public BuildStatusGroup(IBuildDefinition buildDefinition, IEnumerable<BuildItem> buildItems)
        {
            BuildDefinition = buildDefinition;

            AllBuildItems = buildItems?.OrderBy(a => a.Build?.QueueDateTime).ToList();
        }

        #endregion

        #region Public Properties

        public Boolean AdditionalInformationAvailable => AllBuildItems.Any(a => a.CurrentTestRun != null);

        public Boolean AdditionalInformationShown
        {
            get { return this.additionalInformationShown; }
            set
            {
                this.additionalInformationShown = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<BuildItem> AllBuildItems { get; }

        public IBuildDefinition BuildDefinition { get; }

        public BuildItem CurrentBuild => AllBuildItems.Last();

        public List<BuildItem> PreviousBuilds => AllBuildItems.Where(a => a != CurrentBuild).ToList();

        #endregion

        #region Private Fields

        private Boolean additionalInformationShown;

        #endregion
    }
}