using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // Обеспечивает работу главной страницы приложения
        const string HomeView = "Views/Home/Index.cshtml"; // html-файл главной страницы

        private IUserService _userService;
        private IDeckService _deckService;
        private ICardService _cardService;
        private ILanguageService _languageService;

        public HomeController(IUserService userService, IDeckService deckService, ICardService cardService, ILanguageService languageService)
        {
            _userService = userService;
            _deckService = deckService;
            _cardService = cardService;
            _languageService = languageService;
        }

        [Route("")]
        [HttpGet]
        public ActionResult Index() // Получение главной страницы
        {
            if (_userService.Exist(User.Identity.Name)) // если юзера, указанного в куках нет в базе (он мог сменить юзернейм в другом браузере), то выполняется выход
                return View(HomeView);
            else
                return Redirect("Account/Logout");
        }

        [Route("api/GetLanguageList")] // Получение списка языков
        [HttpGet]
        public JsonResult GetLanguageList()
        {
            return Json(_languageService.GetLanguageList());
        }

        [Route("api/GetDeckList")]
        [HttpGet]
        public JsonResult GetDeckList() // Получение списка колод пользователя
        {
            using (StreamWriter writer = new StreamWriter("D:/sukatest.txt"))
            {
                writer.Write(User.Identity.Name);
            }
            return Json(_deckService.GetDeckElementList(User.Identity.Name));
        }

        [Route("api/CreateNewDeck")]
        [HttpPost]
        public bool CreateNewDeck(string DeckName, string LanguageCode, bool IsPublic) // Создание новой колоды
        {
            return _deckService.CreateDeck(User.Identity.Name, DeckName, LanguageCode, IsPublic); // возвращается значение true - если колода успешно создана, false - если нет
        }

        [Route("api/DeleteDeck")]
        [HttpPost]
        public void DeleteDeck(int id) // Удаление колоды
        {
            if (_deckService.AuthorizeUserDeck(User.Identity.Name, id)) // проверка принадлежит ли колода пользователю
                _deckService.DeleteDeck(id);
        }

        [Route("api/GetDeckSettings")]
        [HttpGet]
        public JsonResult GetDeckSettings(int deck_id) // Получение параметров колоды
        {
            if (_deckService.AuthorizeUserDeck(User.Identity.Name, deck_id)) // защита от чтения данных колоды пользователем, которому она не принадлежит
                return Json(_deckService.GetDeckSettings(deck_id));
            else
                return Json(null);
        }

        [Route("api/UpdateDeckSettings")]
        [HttpPost]
        public bool UpdateDeckSettings(int DeckId, string DeckName, string LanguageCode, bool IsPublic) // Обновление параметров колоды
        {
            if (_deckService.AuthorizeUserDeck(User.Identity.Name, DeckId)) // защита от изменения колоды пользователем, которому она не принадлежит
                return _deckService.UpdateDeck(DeckId, DeckName, LanguageCode, IsPublic); // возвращает true в случае удачного обновления данных колоды
            else
                return false;
        }

        [Route("api/AddCard")]
        [HttpPost]
        public bool AddCard(String front, String back, int deck_id) // Добавление новой карточки в колоду
        {
            if (_deckService.AuthorizeUserDeck(User.Identity.Name, deck_id)) // защита от изменения колоды пользователем, которому она не принадлежит
                return _cardService.AddCard(deck_id, front, back); // возвращается значение true - если карточка успешно добавлена, false - если нет
            else
                return false;
        }
    }
}
