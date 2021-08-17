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
using Application.Interfaces;


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
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _context = context; 
                _userAccessor = userAccessor;
            }

            public async Task<Result<Unit>> Handle(Command command, CancellationToken cancellationToken){
                var user  = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());
                var attendee = new ActivityAttendee{
                    AppUser = user,
                    Activity = command.Activity,
                    IsHost = true
                };

                command.Activity.Attendees.Add(attendee);
               _context.Activties.Add(command.Activity);
               var result = await _context.SaveChangesAsync() > 0;
               if (!result) return Result<Unit>.Failure("Failed to create activity!");
               return Result<Unit>.Success(Unit.Value);
            } 
        }
    }
}