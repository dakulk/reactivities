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
    public class Details
    {
        public class Query : IRequest<Activity>{
             public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Activity>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context; 
            }

            public async Task<Activity> Handle(Query query, CancellationToken cancellationToken){
                return await _context.Activties.FindAsync(query.Id);
            }
        }

    }
}