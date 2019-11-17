using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaxkoOrderAPI.Features.Commands;


namespace DaxkoOrderAPI.Models.Validations
{
    public class OrderCommandValidator : AbstractValidator<OrderCommand>
    {
        public OrderCommandValidator()
        {
            RuleFor(x => x.ItemID)
                .NotNull()
                    .WithMessage("Can not be null")
                .Equal(0)
                    .WithMessage("Can not be zero (0)")
                .NotEmpty()
                    .WithMessage("Can not be empty")
                .LessThan(0)
                    .WithMessage(x => $"{x.ItemID} - is less than zero (0)");

            RuleFor(x => x.Quantity)
                .NotNull()
                    .WithMessage("Can not be null")
                .Equal(0)
                    .WithMessage("Can not be zero (0)")
                .NotEmpty()
                    .WithMessage("Can not be empty")
                .LessThan(0)
                    .WithMessage(x => $"{x.Quantity} - is less than zero (0)");
        }
    }
}
