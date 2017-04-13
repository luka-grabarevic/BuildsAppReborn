using System;
using System.ComponentModel.Composition;

namespace BuildsAppReborn.Contracts.Composition
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class PriorityExportAttribute : ExportAttribute, IPriorityMetadata
    {
        #region Constructors

        public PriorityExportAttribute(Type contractType, Int32 priority)
            : base(contractType)
        {
            Priority = priority;
        }

        #endregion

        #region Implementation of IPriorityMetadata

        public Int32 Priority { get; }

        #endregion
    }
}