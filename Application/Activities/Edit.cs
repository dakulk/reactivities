
using MediatR;
using System.Collections.Generic;
using Domain;
using System.Threading.Tasks;
using System.Threading;
using Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using AutoMapper;
using FluentValidation;
using Application.Core;

namespace Application.Activities
{
    public class Edit
    {
        public class Command : IRequest <Result<Unit>>{
            public Activity Activity { get; set; }
        }

        public class Handler: IRequestHandler<Command, Result<Unit>>{

            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _context = context; 
                _mapper = mapper;
            }
        public class CommandValidator : AbstractValidator<Command>{
            public CommandValidator(){
                RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
            }

        }
    
            public async Task<Result<Unit>> Handle(Command command, CancellationToken cancellationToken){
              var activity = await _context.Activties.FindAsync(command.Activity.Id);
              if (activity == null) return null;
                _mapper.Map(command.Activity, activity);
              var result =  await _context.SaveChangesAsync() > 0;
              if (!result) return Result<Unit>.Failure("Failed to update activity!");
              return Result<Unit>.Success(Unit.Value);
            } 
        }
    }
}