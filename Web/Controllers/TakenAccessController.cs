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
    public class TakenAccessController : Controller
    {
        // Обеспечивает работу страницы списка пользователей, предоставивших доступ к аналитике
        const string StudentsView = "Views/Access/TakenAccess.cshtml";
        private IAccessService _accessService;

        public TakenAccessController(IAccessService accessService) //конструктор с инициализацией контекста entity framework
        {
            _accessService = accessService;
        }

        [Route("TakenAccess")]
        [HttpGet]
        public IActionResult Index() // Получение страницы списка пользователей, предоставивших доступ к аналитике пользователя
        {
            return View(StudentsView);
        }

        [Route("api/TakenAccess/GetList")]
        [HttpGet]
        public JsonResult GetList() // Получение списка пользователей, предоставивших доступ к своей аналитике
        {
            return Json(_accessService.TakenAccessRightsList(User.Identity.Name));
        }

        [Route("api/TakenAccess/DeleteAccess")]
        [HttpPost]
        public void DeleteAccess(string username) // Удаление пользователя, предоставившего доступ к своей аналитике
        {
            _accessService.DeleteAccessRights(username, User.Identity.Name);
        }
    }
}