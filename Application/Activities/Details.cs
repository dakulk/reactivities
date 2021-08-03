using MediatR;
using System.Collections.Generic;
using Domain;
using System.Threading.Tasks;
using System.Threading;
using Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using Application.Core;

namespace Application.Activities
{
    public class Details
    {
        public class Query : IRequest<Result<Activity>>{
             public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<Activity>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context; 
            }

            public async Task<Result<Activity>> Handle(Query query, CancellationToken cancellationToken){
                var activity =  await _context.Activties.FindAsync(query.Id);
                return Result<Activity>.Success(activity);
            }
        }

    }
}