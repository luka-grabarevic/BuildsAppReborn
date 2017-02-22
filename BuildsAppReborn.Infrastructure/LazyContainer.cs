using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace BuildsAppReborn.Infrastructure
{
    /// <summary>
    /// container for MEF lazy resolving
    /// </summary>
    /// <typeparam name="TInterface">The type of the interface.</typeparam>
    /// <typeparam name="TMetaData">The type of the meta data.</typeparam>
    [Export]
    public sealed class LazyContainer<TInterface, TMetaData> : IEnumerable<TInterface>, IEnumerator<TInterface>, IReadOnlyDictionary<TMetaData, TInterface>
        where TInterface : class where TMetaData : class
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyContainer{TInterface, TMetaData}"/> class.
        /// </summary>
        /// <param name="lazies">The lazies.</param>
        [ImportingConstructor]
        public LazyContainer([ImportMany] IEnumerable<Lazy<TInterface, TMetaData>> lazies)
        {
            if (lazies == null)
            {
                throw new ArgumentNullException(nameof(lazies));
            }
            this.lazyArray = lazies;
        }

        #endregion

        #region Implementation of IEnumerable<TInterface>

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<TInterface> GetEnumerator()
        {
            return Values.GetEnumerator();
        }

        #endregion

        #region Implementation of IEnumerator<TInterface>

        public void Dispose()
        {
        }

        public Boolean MoveNext()
        {
            return GetEnumerator().MoveNext();
        }

        public void Reset()
        {
            GetEnumerator().Reset();
        }

        Object IEnumerator.Current => GetEnumerator().Current;

        public TInterface Current => GetEnumerator().Current;

        #endregion

        #region Implementation of IReadOnlyDictionary<TMetaData,TInterface>

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <value>
        /// The values.
        /// </value>
        public IEnumerable<TInterface> Values => this.lazyArray.Select(a => a.Value);

        IEnumerator<KeyValuePair<TMetaData, TInterface>> IEnumerable<KeyValuePair<TMetaData, TInterface>>.GetEnumerator()
        {
            return new LazyContainerDictionaryEnumerator(this.lazyArray.GetEnumerator());
        }

        public Boolean ContainsKey(TMetaData key)
        {
            return this.lazyArray.Any(a => a.Metadata == key);
        }

        public Boolean TryGetValue(TMetaData key, out TInterface value)
        {
            if (ContainsKey(key))
            {
                value = GetSingleOrDefault(data => data == key);
                return true;
            }

            value = null;
            return false;
        }

        TInterface IReadOnlyDictionary<TMetaData, TInterface>.this[TMetaData key]
        {
            get
            {
                return GetSingleOrDefault(data => data == key);
            }
        }

        public IEnumerable<TMetaData> Keys => this.lazyArray.Select(a => a.Metadata);

        public Int32 Count => this.lazyArray.Count();

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the many.
        /// </summary>
        /// <param name="searchExpression">The search expression.</param>
        /// <returns></returns>
        public IEnumerable<TInterface> GetMany(Func<TMetaData, Boolean> searchExpression)
        {
            return this.lazyArray.Where(a => a.Metadata != null).Where(lazy => searchExpression.Invoke(lazy.Metadata)).Where(a => a.Value != null).Select(a => a.Value);
        }

        /// <summary>
        /// Gets the specified search expression.
        /// </summary>
        /// <param name="searchExpression">The search expression.</param>
        /// <returns>the implementation</returns>
        public TInterface GetSingleOrDefault(Func<TMetaData, Boolean> searchExpression)
        {
            return GetMany(searchExpression).SingleOrDefault();
        }

        #endregion

        #region Private Fields

        private readonly IEnumerable<Lazy<TInterface, TMetaData>> lazyArray;

        #endregion

        private class LazyContainerDictionaryEnumerator : IEnumerator<KeyValuePair<TMetaData, TInterface>>
        {
            #region Constructors

            public LazyContainerDictionaryEnumerator(IEnumerator<Lazy<TInterface, TMetaData>> innerEnumerator)
            {
                this.innerEnumerator = innerEnumerator;
            }

            #endregion

            #region Implementation of IEnumerator<KeyValuePair<TMetaData,TInterface>>

            public void Dispose()
            {
            }

            public Boolean MoveNext()
            {
                return this.innerEnumerator.MoveNext();
            }

            public void Reset()
            {
                this.innerEnumerator.Reset();
            }

            public KeyValuePair<TMetaData, TInterface> Current => new KeyValuePair<TMetaData, TInterface>(this.innerEnumerator.Current.Metadata, this.innerEnumerator.Current.Value);

            Object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            #endregion

            #region Private Fields

            private readonly IEnumerator<Lazy<TInterface, TMetaData>> innerEnumerator;

            #endregion
        }
    }
}