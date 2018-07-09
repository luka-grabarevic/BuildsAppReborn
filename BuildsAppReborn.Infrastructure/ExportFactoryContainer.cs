using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace BuildsAppReborn.Infrastructure
{
    /// <summary>
    /// container for multiple export factories
    /// </summary>
    /// <typeparam name="TInterface">The type of the interface.</typeparam>
    /// <typeparam name="TMetaData">The type of the meta data.</typeparam>
    [Export]
    public class ExportFactoryContainer<TInterface, TMetaData>
        where TInterface : class
        where TMetaData : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExportFactoryContainer{TInterface,TMetaData}" /> class.
        /// </summary>
        /// <param name="factories">The factories.</param>
        /// <exception cref="System.ArgumentNullException">lazies</exception>
        [ImportingConstructor]
        public ExportFactoryContainer([ImportMany] ExportFactory<TInterface, TMetaData>[] factories)
        {
            if (factories == null)
            {
                throw new ArgumentNullException(nameof(factories));
            }

            this.factories = factories;
        }

        /// <summary>
        /// Gets the meta data.
        /// </summary>
        /// <value>
        /// The meta data.
        /// </value>
        public IEnumerable<TMetaData> MetaData
        {
            get
            {
                if (this.metadata == null)
                {
                    var list = new List<TMetaData>();
                    foreach (var it in this.factories)
                    {
                        list.Add(it.Metadata);
                    }

                    this.metadata = list.AsReadOnly();
                }

                return this.metadata;
            }
        }

        /// <summary>
        /// Gets the many.
        /// </summary>
        /// <param name="searchExpression">The search expression.</param>
        /// <returns></returns>
        public IEnumerable<ExportFactory<TInterface>> GetMany(Func<TMetaData, Boolean> searchExpression)
        {
            return this.factories.Where(a => a.Metadata != null).Where(lazy => searchExpression.Invoke(lazy.Metadata));
        }

        /// <summary>
        /// Gets the specified search expression.
        /// </summary>
        /// <param name="searchExpression">The search expression.</param>
        /// <returns>the implementation</returns>
        public ExportFactory<TInterface> GetSingleOrDefault(Func<TMetaData, Boolean> searchExpression)
        {
            return GetMany(searchExpression).SingleOrDefault();
        }

        private readonly ExportFactory<TInterface, TMetaData>[] factories;
        private IReadOnlyList<TMetaData> metadata;
    }
}