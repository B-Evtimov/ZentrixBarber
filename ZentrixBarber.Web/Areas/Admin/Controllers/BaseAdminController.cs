using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static ZentrixBarber.Common.ApplicationConstants;

namespace ZentrixBarber.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = AdministratorRoleName)]
    public abstract class BaseAdminController : Controller
    {
    }
}