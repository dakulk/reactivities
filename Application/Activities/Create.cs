using MediatR;
using System.Collections.Generic;
using Domain;
using System.Threading.Tasks;
using System.Threading;
using Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using FluentValidation;
using Application.Core;


namespace Application.Activities
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>{
            public Activity Activity { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>{
            public CommandValidator(){
                RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
            }

        }
    

        public class Handler: IRequestHandler<Command, Result<Unit>>{

            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context; 
            }

            public async Task<Result<Unit>> Handle(Command command, CancellationToken cancellationToken){
               _context.Activties.Add(command.Activity);
               var result = await _context.SaveChangesAsync() > 0;
               if (!result) return Result<Unit>.Failure("Failed to create activity!");
               return Result<Unit>.Success(Unit.Value);
            } 
        }
    }
}