using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Expense.Controllers;

[ApiController]
[Route("[controller]")]
public class BaseApiController : ControllerBase
{
}
