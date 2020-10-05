using Bigai.Holidays.Shared.Domain.Enums.Entities;
using System;
using System.Reflection;

namespace Bigai.Holidays.Shared.Domain.Models
{
    /// <summary>
    /// <see cref="Entity"/> represents a basic entity of the domain model.
    /// </summary>
    public abstract class Entity
    {
        #region Public Properties

        /// <summary>
        /// Record identifier.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Current status of the entity.
        /// </summary>
        public EntityStatus Status { get; private set; }

        /// <summary>
        /// Action to take with entity.
        /// </summary>
        public TypeProcess Action { get; private set; }

        /// <summary>
        /// Who registered the record.
        /// </summary>
        public Guid? RegisteredBy { get; private set; }

        /// <summary>
        /// Registration date.
        /// </summary>
        public DateTime RegistrationDate { get; private set; }

        /// <summary>
        /// Last user who modified the registry.
        /// </summary>
        public Guid? ModifiedBy { get; private set; }

        /// <summary>
        /// Date the record was last modified.
        /// </summary>
        public DateTime? ModificationDate { get; private set; }

        #endregion

        #region Constructor

        protected Entity() { }

        /// <summary>
        /// Return a instance of <see cref="Entity"/>.
        /// </summary>
        /// <param name="id">Record identifier. Optional if action equal <c>Register</c>. Required for other actions.</param>
        /// <param name="status">Current status of the entity. Required.</param>
        /// <param name="action">Action to take with entity. Required.</param>
        /// <param name="userId">Who is taking this action. Optional.</param>
        protected Entity(Guid? id, EntityStatus status, TypeProcess action, Guid? userId)
        {
            AssignId(id);

            Action = action;
            Status = status;

            AssignRegistrationDate();
            AssignLastModificationDate();
            AssignWhoDidAction(userId);
        }

        #endregion

        #region Private Methods

        private void AssignId(Guid? id)
        {
            if (!id.HasValue || id == null || id.Equals(Guid.Empty))
            {
                Id = Guid.NewGuid();
            }
            else
            {
                Id = id.Value;
            }
        }

        private void AssignRegistrationDate()
        {
            if (Action == TypeProcess.Register || Action == TypeProcess.Seed)
            {
                RegistrationDate = DateTime.UtcNow;
            }
        }

        private void AssignLastModificationDate()
        {
            if (Action != TypeProcess.Register)
            {
                ModificationDate = DateTime.UtcNow;
            }
        }

        private void AssignWhoDidAction(Guid? userId)
        {
            if ((Action == TypeProcess.Register || Action == TypeProcess.Register) && userId.HasValue)
            {
                RegisteredBy = userId.Value;
            }
            else if (Action != TypeProcess.Register && userId.HasValue)
            {
                ModifiedBy = userId;
            }
        }

        #endregion

        #region Comparison Methods

        /// <summary>
        /// Determines whether the specified object instances are considered equal.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns><c>true</c> if the objects are considered equal, otherwise, <c>false</c>. If both objects are <c>null</c>,
        /// returns <c>true</c>.</returns>
        public override bool Equals(object obj)
        {
            Entity entity = obj as Entity;

            if (ReferenceEquals(null, entity))
            {
                return false;
            }

            if (ReferenceEquals(this, entity))
            {
                return true;
            }

            if (entity.GetType() != GetType())
            {
                return false;
            }

            return EqualsCore(entity);
        }

        /// <summary>
        /// Determines whether the instances of the specified object are considered equal, according to the business rule of 
        /// the object that it inherited.
        /// </summary>
        /// <param name="other">The object to compare.</param>
        /// <returns><c>true</c> if the current object is equal to the other parameter; otherwise, <c>false</c>.</returns>
        protected abstract bool EqualsCore(object other);

        #endregion

        #region Hashcode

        /// <summary>
        /// Generate hash code for the current object.
        /// </summary>
        /// <returns>A hash code for the current object</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 7;
                Type type = GetType();
                PropertyInfo[] properties = type.GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (!property.PropertyType.ToString().Contains("ValidationResult") &&
                        !property.PropertyType.ToString().Contains("TipoProcesso") &&
                        !type.GetProperty(property.Name).GetGetMethod().IsVirtual)
                    {
                        hash = HashValue(hash, property.GetValue(this, null));
                    }
                }

                return hash;
            }
        }

        /// <summary>
        /// Generate hash code for the current object value.
        /// </summary>
        /// <param name="seed">Initial value.</param>
        /// <param name="value">Value to be generate the hash value.</param>
        /// <returns>A hash code for the object value.</returns>
        private int HashValue(int seed, object value)
        {
            unchecked
            {
                int hash = value != null ? value.GetHashCode() : 0;

                return seed * 7 + hash;
            }
        }

        #endregion

        #region Operators Overload

        /// <summary>
        /// Determines whether the specified object instances are considered equal.
        /// </summary>
        /// <param name="objA">The first object to compare.</param>
        /// <param name="objB">The second object to compare.</param>
        /// <returns><c>true</c> if the objects are considered equal, otherwise, <c>false</c>. If both objA and objB 
        /// are null, the returns <c>true</c>.</returns>
        public static bool operator ==(Entity objA, Entity objB)
        {
            if (ReferenceEquals(objA, null) && ReferenceEquals(objB, null))
            {
                return true;
            }

            if (ReferenceEquals(objA, null) || ReferenceEquals(objB, null))
            {
                return false;
            }

            return objA.Equals(objB);
        }

        /// <summary>
        /// Determines whether the specified object instances are considered diferent.
        /// </summary>
        /// <param name="objA">The first object to compare.</param>
        /// <param name="objB">The second object to compare.</param>
        /// <returns><c>true</c> if the objects are considered diferent, otherwise, <c>false</c>.</returns>
        public static bool operator !=(Entity objA, Entity objB)
        {
            return !(objA == objB);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the registration date of the root entity in the aggregated entity.
        /// </summary>
        /// <param name="root">Root entity.</param>
        public virtual void SynchronizeRegistrationDate(Entity root)
        {
            if (root.Action == TypeProcess.Register)
            {
                RegistrationDate = root.RegistrationDate;
            }
        }

        #endregion
    }
}
