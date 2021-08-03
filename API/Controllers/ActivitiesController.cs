using Persistence;
using Domain;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using MediatR;
namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetActivities(){
            return HandleResult(await  Mediator.Send(new Application.Activities.List.Query()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> GetActivity(Guid id){
            return HandleResult(await Mediator.Send(new Application.Activities.Details.Query{ Id = id}));

        }

        [HttpPost]
        public async Task<IActionResult> CreateActivity(Activity activity){
            return HandleResult(await Mediator.Send(new Application.Activities.Create.Command{Activity = activity     }));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditActivity(Guid id,Activity activity){
            activity.Id = id;
            
            return HandleResult(await Mediator.Send(new Application.Activities.Edit.Command{Activity = activity     }));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(Guid id){
            return HandleResult(await Mediator.Send(new Application.Activities.Delete.Command{ Id = id}));
        }
    }
}