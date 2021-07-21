using MediatR;
using System.Collections.Generic;
using Domain;
using System.Threading.Tasks;
using System.Threading;
using Persistence;
using Microsoft.EntityFrameworkCore;
using System;


namespace Application.Activities
{
    public class Create
    {
        public class Command : IRequest{
            public Activity Activity { get; set; }
        }

        public class Handler: IRequestHandler<Command>{

            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context; 
            }

            public async Task<Unit> Handle(Command command, CancellationToken cancellationToken){
               _context.Activties.Add(command.Activity);
               await _context.SaveChangesAsync();
               return Unit.Value;
            } 
        }
    }
}