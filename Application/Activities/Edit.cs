
using MediatR;
using System.Collections.Generic;
using Domain;
using System.Threading.Tasks;
using System.Threading;
using Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using AutoMapper;

namespace Application.Activities
{
    public class Edit
    {
        public class Command : IRequest{
            public Activity Activity { get; set; }
        }

        public class Handler: IRequestHandler<Command>{

            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _context = context; 
                _mapper = mapper;
            }

            public async Task<Unit> Handle(Command command, CancellationToken cancellationToken){
              var activity = await _context.Activties.FindAsync(command.Activity.Id);
                _mapper.Map(command.Activity, activity);
               await _context.SaveChangesAsync();
               return Unit.Value;
            } 
        }
    }
}