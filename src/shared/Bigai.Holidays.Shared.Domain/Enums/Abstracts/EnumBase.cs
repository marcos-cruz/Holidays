using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bigai.Holidays.Shared.Domain.Enums.Abstracts
{
    /// <summary>
    /// <see cref="EnumBase{TEnum, TKey}"/> represents a base class for creating smart enums.
    /// </summary>
    /// <typeparam name="TEnum">Represents the type of enum class you are inheriting from this class.</typeparam>
    /// <typeparam name="TKey">The type of value that will be used as the enum key, <c>string</c>, or <c>int</c>.</typeparam>
    public abstract class EnumBase<TEnum, TKey> :
        IEquatable<EnumBase<TEnum, TKey>>,
        IComparable<EnumBase<TEnum, TKey>>
        where TEnum : EnumBase<TEnum, TKey>
        where TKey : IEquatable<TKey>, IComparable<TKey>
    {
        #region Private Variables

        private readonly string _name;

        private readonly TKey _key;

        private static readonly List<TEnum> _list = new List<TEnum>();

        // Despite analysis tool warnings, we want this static bool to be on this generic type (so that each TEnum has its own bool).
        private static bool _invoked;

        #endregion

        #region Public Properties

        /// <summary>
        /// Returns a list of all the enums that are defined.
        /// </summary>
        public static IReadOnlyCollection<TEnum> ListOfEnums
        {
            get
            {
                if (!_invoked)
                {
                    _invoked = true;

                    //
                    // Force invocaiton/initialization by calling one of the derived members.
                    //
                    typeof(TEnum).GetProperties(BindingFlags.Public | BindingFlags.Static).FirstOrDefault(p => p.PropertyType == typeof(TEnum))?.GetValue(null, null);
                }

                return _list.ToArray();
            }
        }

        /// <summary>
        /// Returns the name or description of the enum.
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// Returns the enum key.
        /// </summary>
        public TKey Key => _key;

        #endregion

        #region Constructor

        /// <summary>
        /// Return a instance of <see cref="EnumBase{TEnum, TKey}"/>
        /// </summary>
        protected EnumBase() { }

        /// <summary>
        /// Return a instance of <see cref="EnumBase{TEnum, TKey}"/>
        /// </summary>
        /// <param name="key">Key to identify enum.</param>
        /// <param name="name">Name of enum.</param>
        protected EnumBase(TKey key, string name)
        {
            _name = name;
            _key = key;

            TEnum item = this as TEnum;
            _list.Add(item);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Determines whether an enum exists based on the name.
        /// </summary>
        /// <param name="name">Name of the enum to test.</param>
        /// <returns><see cref="TEnum"/> instance corresponding to the name, or <c>null</c> if it does not exist.</returns>
        public static TEnum GetByName(string name)
        {
            return ListOfEnums.SingleOrDefault(item => string.Equals(item.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Determines whether an enum exists based on the key code.
        /// </summary>
        /// <param name="key">Key code of the enum to test.</param>
        /// <returns><see cref="TEnum"/> instance corresponding to the key code, or <c>null</c> if it does not exist.</returns>
        public static TEnum GetById(TKey key)
        {
            //
            // Can't use == to compare generics unless we constrain TValue to "class", which we don't want because then we couldn't use int.
            //
            return _list.SingleOrDefault(item => EqualityComparer<TKey>.Default.Equals(item.Key, key));
        }

        /// <summary>
        /// Return a string that represents the current object.
        /// </summary>
        /// <returns>String that represents the current object.</returns>
        public override string ToString()
        {
            return _name;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">The object to compare.</param>
        /// <returns><c>true</c> if the current object is equal to the other parameter; otherwise, <c>false</c>.</returns>
        public virtual bool Equals(EnumBase<TEnum, TKey> other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (other.GetType() != GetType())
            {
                return false;
            }

            return _key.Equals(other._key);
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates
        /// whether the current instance precedes, follows, or occurs in the same position in the sort order as the 
        /// other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has 
        /// these meanings: Value Meaning Less than zero This instance precedes other in the sort order. Zero This 
        /// instance occurs in the same position in the sort order as other. Greater than zero This instance follows 
        /// other in the sort order.
        /// </returns>
        public int CompareTo(EnumBase<TEnum, TKey> other)
        {
            return _key.CompareTo(other._key);
        }

        #endregion
    }

    /// <summary>
    /// Represents a <see cref = "TEnum" /> that uses an <c>int</c> as a value for the key code.
    /// </summary>
    /// <typeparam name="TEnum">Represents the type of enum class you are inheriting from this class.</typeparam>
    public abstract class EnumBase<TEnum> : EnumBase<TEnum, int> where TEnum : EnumBase<TEnum, int>
    {
        /// <summary>
        /// Return a instance of <see cref="EnumBase{TEnum, TKey}"/>
        /// </summary>
        protected EnumBase() { }

        /// <summary>
        /// Return a instance of <see cref="EnumBase{TEnum, TKey}"/>
        /// </summary>
        /// <param name="key">Key to identify enum.</param>
        /// <param name="name">Name of enum.</param>
        protected EnumBase(int key, string name) : base(key, name) { }
    }
}
