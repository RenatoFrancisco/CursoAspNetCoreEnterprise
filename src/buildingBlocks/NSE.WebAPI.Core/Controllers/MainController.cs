namespace NSE.WebAPI.Core.Controllers;

[ApiController]
public abstract class MainController : Controller
{
    protected ICollection<string> Errors = new List<string>();
    protected bool IsValidOperation => !Errors.Any();

    protected ActionResult CustomResponse(object result = null)
    {
        if (IsValidOperation) return Ok(result);

        return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
        {
            { "Messages", Errors.ToArray() }
        }));
    }

    protected ActionResult CustomResponse(ModelStateDictionary modelState)
    {
        var errors = modelState.Values.SelectMany(e => e.Errors).ToList();
        errors.ForEach(e => AddError(e.ErrorMessage));

        return CustomResponse();
    }

    protected ActionResult CustomResponse(ValidationResult validationResult)
    {
        validationResult.Errors
            .ForEach(e => AddError(e.ErrorMessage));

        return CustomResponse();
    }

    protected ActionResult CustomResponse(ResponseResult response)
    {
        ResponseHasErrors(response);
        return CustomResponse();
    }

    protected bool ResponseHasErrors(ResponseResult response)
    {
        if (response is null || !response.Errors.Messages.Any()) return false;

        response.Errors.Messages.ForEach(AddError);

        return true;
    }

    protected void AddError(string error) => Errors.Add(error);

    protected void ClearErrors() => Errors.Clear();
}