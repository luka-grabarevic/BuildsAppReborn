using System;
using System.ComponentModel.Composition;

namespace BuildsAppReborn.Contracts.Composition
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class PriorityExportAttribute : ExportAttribute, IPriorityMetadata
    {
        public PriorityExportAttribute(Type contractType, Int32 priority)
            : base(contractType)
        {
            Priority = priority;
        }

        public Int32 Priority { get; }
    }
}