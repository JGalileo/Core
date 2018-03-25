using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Linq;

namespace Core.Conventions
{
    public class RoutingConvention : IApplicationModelConvention
    {
        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                controller.ControllerName = new string(controller.ControllerName.Select((v, index) => index == 0 ? char.ToLowerInvariant(v) : v).ToArray());
                foreach (var action in controller.Actions)
                {
                    action.ActionName = new string(action.ActionName.SelectMany((v, index) => char.IsUpper(v) && index > 0 ? new[] { '-', char.ToLowerInvariant(v), } : new[] { char.ToLowerInvariant(v), }).ToArray());
                }
            }

            // You can continue to put attribute route templates for the controller actions depending on the way you want them to behave
        }
    }
}
