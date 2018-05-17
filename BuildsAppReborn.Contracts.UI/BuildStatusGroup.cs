using System;
using System.Collections.Generic;
using System.Linq;
using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Infrastructure;

namespace BuildsAppReborn.Contracts.UI
{
    public class BuildStatusGroup : ViewModelBase
    {
        public BuildStatusGroup(IPullRequest pullRequest, IEnumerable<BuildItem> buildItems) : this(buildItems)
        {
            PullRequest = pullRequest;
        }

        public BuildStatusGroup(IBuildDefinition buildDefinition, IEnumerable<BuildItem> buildItems) : this(buildItems)
        {
            BuildDefinition = buildDefinition;
        }

        private BuildStatusGroup(IEnumerable<BuildItem> buildItems)
        {
            AllBuildItems = buildItems?.OrderBy(a => a.Build?.QueueDateTime).ToList();
        }

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

        public IPullRequest PullRequest { get; }

        private Boolean additionalInformationShown;
    }
}