using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JustManAdmin.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class BaseHomeController:Controller
    {
        
    }
}