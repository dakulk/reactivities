using Persistence;
using Domain;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Policy ="IsActivityHost")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditActivity(Guid id,Activity activity){
            activity.Id = id;
            
            return HandleResult(await Mediator.Send(new Application.Activities.Edit.Command{Activity = activity     }));
        }

        [Authorize(Policy ="IsActivityHost")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(Guid id){
            return HandleResult(await Mediator.Send(new Application.Activities.Delete.Command{ Id = id}));
        }
        
        [HttpPost("{id}/attend")]
        public async Task<IActionResult> Attend(Guid id){
            return HandleResult(await Mediator.Send(new Application.Activities.UpdateAttendence.Command{Id = id     }));
        }

    }
}