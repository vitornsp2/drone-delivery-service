using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DroneDelivery.Domain.Models;
using FluentValidation;

namespace DroneDelivery.Domain.Validations.models
{
    public class DroneValidator : AbstractValidator<Drone>
    {
        public DroneValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .MaximumLength(100).WithMessage("{PropertyName} must be fewer than 100 characters");

            RuleFor(p => p.MaximumWeight)
                .GreaterThan(1).WithMessage("{PropertyName} cannot be less then 1");
        }
    }
}