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
using Application.Interfaces;
using System.Linq;

namespace Application.Activities
{
    public class UpdateAttendence
    {
        
        public class Command : IRequest <Result<Unit>>{
            public Guid Id { get; set; }
        }
        public class Handler : IRequestHandler<Command, Result<Unit>>{
            
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor, IMapper mapper)
            {
                _context = context; 
                _mapper = mapper;
                _userAccessor = userAccessor;
            }

            
            public async Task<Result<Unit>> Handle(Command command, CancellationToken cancellationToken){
                var activity = await _context.Activties
                .Include(x => x.Attendees).ThenInclude(x => x.AppUser).SingleOrDefaultAsync(x => x.Id == command.Id);
                if (activity == null){
                    return null;
                }

                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());
                if (user == null) return null;
                var hostUserName = activity.Attendees.FirstOrDefault(x => x.IsHost)?.AppUser?.UserName;
                var attendance = activity.Attendees.FirstOrDefault(x => x.AppUser.UserName == user.UserName);
                if (attendance != null && hostUserName == user.UserName){
                    activity.IsCancelled = !activity.IsCancelled;
                }
                if (attendance != null && hostUserName != user.UserName){
                    activity.Attendees.Remove(attendance);
                }
                if (attendance == null){
                    attendance = new ActivityAttendee{
                        AppUser = user,
                        Activity = activity,
                        IsHost = false
                    };
                    activity.Attendees.Add(attendance);
                }

                var result = await _context.SaveChangesAsync()>0;
                return result? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Problem updating attendance");
            } 
     
   
        }
    }
}