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
    public class ProfileController : Controller
    {
        // Обеспечивает работу страницы общедоступного профиля пользователя
        const string ProfileView = "Views/Profile/Profile.cshtml";
        private IUserService _userService;
        private IAccessService _accessService;
        private IDeckService _deckService;

        public ProfileController(IUserService userService, IAccessService accessService, IDeckService deckService) // конструктор с инициализацией контекста entity framework
        {
            _userService = userService;
            _accessService = accessService;
            _deckService = deckService;
        }

        [Route("Profile/{username}")]
        [HttpGet]
        public IActionResult Index(string username) // Получение страницы общедоступного профиля пользователя
        {
            ViewBag.CanBecomeStudent = username != User.Identity.Name; // если открыл свой же профиль

            if (!ViewBag.CanBecomeStudent || _userService.Exist(username))
            {
                ViewBag.Exist = true;
                ViewBag.Username = username;
            }
            else
            {
                ViewBag.Exist = false;
                ViewBag.Username = "not found";
            }

            if (ViewBag.CanBecomeStudent) // проверка, предоставил ли уже текущий пользователь права доступа этому пользователю
                ViewBag.AlreadyAStudent = _accessService.AccessExists(User.Identity.Name, username);

            return View(ProfileView);
        }

        [Route("api/Profile/GetDeckList")]
        [HttpGet]
        public JsonResult GetDeckList(string username) // Получение списка публичных колод пользователя
        {
            return Json(_deckService.GetPublicDecksWithCards(username));
        }

        [Route("api/Profile/GrantAccessRights")]
        [HttpPost]
        public bool GrantAccessRights(string username) // Предоставляет другому пользователю доступ к аналитике данного пользователя
        {
            return _accessService.GiveAccessRights(User.Identity.Name, username);
        }

        [Route("api/Profile/DeleteAccessRights")]
        [HttpPost]
        public bool DeleteAccessRights(string username) // Отменяет права доступ одного пользователя к аналитике другого
        {
            return _accessService.DeleteAccessRights(User.Identity.Name, username);
        }

        [Route("api/Profile/CopyDeck")]
        [HttpPost]
        public bool CopyDeck(int deck_id) // Копирование публичной колоды из общедоступного профиля пользователя
        {
            return _deckService.CopyDeck(User.Identity.Name, deck_id);
        }
    }
}