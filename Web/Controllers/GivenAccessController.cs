using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize]
    public class GivenAccessController : Controller
    {
        //Обеспечивает работу страницы списка пользователей, имеющих доступ к аналитике данного пользователя
        const string TeachersView = "Views/Access/GivenAccess.cshtml";
        private IAccessService _accessService;
        public GivenAccessController(IAccessService accessService) //конструктор с инициализацией контекста entity framework
        {
            _accessService = accessService;
        }

        [Route("GivenAccess")]
        [HttpGet]
        public IActionResult Index() // Получение страницы пользователей, имеющих доступ к аналитике данного пользователя
        {
            return View(TeachersView);
        }

        [Route("api/GivenAccess/GetList")]
        [HttpGet]
        public JsonResult GetList() // Получение списка пользователей, имеющих доступ к аналитике данного пользователя
        {
            return Json(_accessService.GivenAccessRightsList(User.Identity.Name));
        }

        [Route("api/GivenAccess/DeleteAccess")]
        [HttpPost]
        public void DeleteAccess(string username)
        {
            _accessService.DeleteAccessRights(User.Identity.Name, username);
        }
    }
}