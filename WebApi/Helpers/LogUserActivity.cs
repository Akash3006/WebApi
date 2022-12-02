using Microsoft.AspNetCore.Mvc.Filters;
using WebApi.Extensions;
using WebApi.Interfaces;

namespace WebApi.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {       

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            
            var resultContext = await next();
            if(resultContext.HttpContext.User.Identity.IsAuthenticated) return;
            string username = context.HttpContext.User.GetUserName();
            var repository = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
            var user = await repository.GetMappedUserAsync(username);
            user.LastActive = DateTime.Now;
            await repository.SaveAllAsync();
        }
    }
}