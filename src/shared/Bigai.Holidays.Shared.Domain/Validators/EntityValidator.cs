using Bigai.Holidays.Shared.Domain.Enums.Entities;
using Bigai.Holidays.Shared.Domain.Models;
using Bigai.Holidays.Shared.Infra.CrossCutting.Helpers;
using FluentValidation;
using System;

namespace Bigai.Holidays.Shared.Domain.Validators
{
    /// <summary>
    /// This class provides support for validating the base entity.
    /// </summary>
    /// <typeparam name="T">Represents the type of the class that inherits from this class.</typeparam>
    public abstract class EntityValidatorError<T> : AbstractValidator<T> where T : Entity
    {
        #region Constructor

        protected EntityValidatorError() : base()
        {
            ValidateId();
            ValidateStatus();
            ValidateTypeProcess();
            ValidateRegistrationDate();
            ValidateRegisteredBy();
            ValidateModificationDate();
            ValidateModifiedBy();
        }

        #endregion

        #region Validations

        private void ValidateId()
        {
            RuleFor(entity => entity.Id)
                .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.");
        }

        private void ValidateStatus()
        {
            RuleFor(entity => entity.Status)
                .Must(BeStatus).WithMessage("{PropertyName} não é válido.");
        }

        private void ValidateTypeProcess()
        {
            RuleFor(entity => entity.Action)
                .Must(BeTypeProcess).WithMessage("{PropertyName} não é válida.");
        }

        private void ValidateRegistrationDate()
        {
            When(entity => entity.Action == TypeProcess.Register, () =>
            {
                RuleFor(entity => entity.RegistrationDate)
                    .GreaterThan(entity => DateTime.MinValue)
                    .WithMessage("Data de cadastro deve ser informada.");
            });
        }

        private void ValidateRegisteredBy()
        {
            When(entity => entity.Action == TypeProcess.Register, () =>
            {
                RuleFor(entity => entity.RegisteredBy)
                    .NotEmpty().WithMessage("Quem está cadastrando deve ser informado.");
            });
        }

        private void ValidateModificationDate()
        {
            When(entity => entity.Action != TypeProcess.Register, () =>
            {
                RuleFor(entity => entity.ModificationDate)
                    .GreaterThan(entity => DateTime.MinValue)
                    .WithMessage("Data de modificação deve ser informada.");
            });

        }

        private void ValidateModifiedBy()
        {
            When(entity => entity.Action != TypeProcess.Register, () =>
            {
                RuleFor(entity => entity.ModifiedBy)
                    .NotEmpty().WithMessage("Quem está modificando deve ser informado.");
            });
        }

        protected bool BeStatus(EntityStatus status)
        {
            return status != null && EntityStatus.GetById(status.Key) != null;
        }

        protected bool BeTypeProcess(TypeProcess action)
        {
            return action != null && TypeProcess.GetById(action.Key) != null;
        }

        protected bool BeCountry(string countryIsoCode)
        {
            return countryIsoCode.HasValue() && (countryIsoCode.Length == 2 || countryIsoCode.Length == 3);
        }

        protected bool BeState(string countryCode, string stateIsoCode)
        {
            return countryCode.HasValue() && stateIsoCode.HasValue();
        }

        protected bool BePositiveInteger(string value)
        {
            return value.IsPositiveInteger();
        }

        #endregion
    }
}
