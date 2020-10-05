﻿using Bigai.Holidays.Core.Domain.Interfaces.Repositories.Countries;
using Bigai.Holidays.Core.Domain.Interfaces.Repositories.States;
using Bigai.Holidays.Core.Domain.Models.Countries;
using Bigai.Holidays.Core.Domain.Models.States;
using Bigai.Holidays.Shared.Domain.Enums.Entities;
using Bigai.Holidays.Shared.Domain.Validators;
using Bigai.Holidays.Shared.Infra.CrossCutting.Helpers;
using FluentValidation;
using System;
using System.Linq;

namespace Bigai.Holidays.Core.Domain.Validators.States
{
    /// <summary>
    /// This class provides support for validating <see cref="State"/>.
    /// </summary>
    public class StateValidator : EntityValidatorError<State>
    {
        #region Constructor

        public StateValidator() : base()
        {
            CommonValidations();
        }

        #endregion

        #region Validations

        protected void CommonValidations()
        {
            ValidateCountryId();
            ValidateCountryIsoCode();
            ValidateStateIsoCode();
            ValidateName();
        }

        private void ValidateCountryId()
        {
            RuleFor(state => state.CountryId)
                .NotEmpty().WithMessage("País deve ser informado.")
                .NotEqual(Guid.Empty).WithMessage("País não é válido.");
        }

        private void ValidateCountryIsoCode()
        {
            RuleFor(state => state.CountryIsoCode)
                .NotEmpty().WithMessage("Código ISO do país deve ser informado.")
                .Must(BeCountry).WithMessage("Código ISO do país não é válido.");
        }

        private void ValidateStateIsoCode()
        {
            RuleFor(state => state.StateIsoCode)
                .NotEmpty().WithMessage("Código ISO do estado deve ser informado.")
                .Must(BeState).WithMessage("Código ISO do estado não é válido.");
        }

        private void ValidateName()
        {
            When(state => !state.Name.HasValue(), () =>
            {
                RuleFor(state => state.Name)
                    .NotEmpty().WithMessage("Nome oficial do estado deve ser informado.");
            });

            When(state => state.Name.HasValue(), () =>
            {
                RuleFor(state => state.Name)
                    .MaximumLength(100).WithMessage("Nome oficial do estado deve ter no máximo 100 caracteres.");
            });
        }

        private bool BeState(State state, string stateIsoCode)
        {
            return BeState(state.CountryIsoCode, stateIsoCode);
        }

        protected bool CountryMustExist(State state, ICountryRepository countryRepository)
        {
            Country record = countryRepository.GetById(state.CountryId);

            if (record != null && state.CountryIsoCode != record.CountryIsoCode3)
            {
                record = null;
            }

            return record == null;
        }

        protected bool StateIsoCodeMustBeUnique(State state, IStateRepository stateRepository)
        {
            State record = stateRepository.Find(c => c.CountryIsoCode == state.CountryIsoCode && c.StateIsoCode == state.StateIsoCode).FirstOrDefault();

            if (state.Action != TypeProcess.Register && record.Id == state.Id)
            {
                record = null;
            }

            return record == null;
        }

        #endregion
    }
}
