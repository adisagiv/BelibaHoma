using System.Web.Mvc;

namespace BelibaHoma.Areas.Trainee
{
    public class TraineeAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Trainee";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Trainee_default",
                "Trainee/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "BelibaHoma.Areas.Trainee.Controllers" }
            );
        }
    }
}