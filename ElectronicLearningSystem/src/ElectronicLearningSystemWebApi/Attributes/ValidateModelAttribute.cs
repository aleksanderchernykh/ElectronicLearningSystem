using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicLearningSystemWebApi.Attributes
{
    /// <summary>
    /// Аттрибут для проверки входящих данных.
    /// </summary>
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Выполнение действия до основного метода.
        /// </summary>
        /// <param name="context">Контекст выполнения запроса. </param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(new
                {
                    ErrorCode = "INVALID_REQUEST",
                    ErrorMessage = "Incorrect data request",
                    Errors = context.ModelState
                });
            }
        }
    }
}
