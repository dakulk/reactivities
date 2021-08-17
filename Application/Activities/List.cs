using MediatR;
using System.Collections.Generic;
using Domain;
using System.Threading.Tasks;
using System.Threading;
using Persistence;
using Microsoft.EntityFrameworkCore;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Application.Activities
{
    public class List
    {
        public class Query : IRequest<Result<List<ActivityDto>>>{
             
        }

        public class Handler : IRequestHandler<Query, Result<List<ActivityDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _context = context; 
                _mapper = mapper;
            }

            public async Task<Result<List<ActivityDto>>> Handle(Query query, CancellationToken cancellationToken){
                var activities = await _context.Activties
                .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
                return Result<List<ActivityDto>>.Success(activities);
            }
        }
    }
}