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
    public class AnalyticsController : Controller
    {
        // Обеспечивает работу страницы аналитики обучения
        const string AnalyticsView = "Views/Analytics/Analytics.cshtml";
        private IAnalyticsService _analyticsService;
        private IDeckService _deckService;

        public AnalyticsController(IAnalyticsService analyticsService, IDeckService deckService) // Получение страницы аналитики обучения пользователя
        {
            _analyticsService = analyticsService;
            _deckService = deckService;
        }

        [Route("analytics")]
        [HttpGet]
        public IActionResult Analytics()  // просмотр собственной аналитики
        {
            ViewBag.Username = "Your";
            return View(AnalyticsView);
        }

        [Route("analytics/{username}")] // username используется для проверки права на просмотр аналитики этого аккаунта
        [HttpGet]
        public IActionResult Analytics(string username) // Получение страницы аналитики другого пользователя
        {
            if (_analyticsService.AuthorizeUserAnalytics(username, User.Identity.Name)) // защита от просмотра чужой аналитики, на просмотр которой у пользователя нет прав
            {
                ViewBag.Username = username;
                return View(AnalyticsView);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        [Route("api/Analytics/GetOwnAnalytics")]
        [HttpGet]
        public JsonResult GetOwnAnalytics() // Получение собственной аналитики обучения
        {
            return GetAnalytics(User.Identity.Name);
        }

        [Route("api/Analytics/GetAnalytics")] // username нужен для проверки права на просмотр аналитики этого аккаунта
        [HttpGet]
        public JsonResult GetAnalytics(string username) // Получение аналитики обучения другого пользователя
        {
            if (_analyticsService.AuthorizeUserAnalytics(username, User.Identity.Name)) // защита от просмотра чужой аналитики, на просмотр которой у пользователя нет прав
                return Json(_analyticsService.GetAnalytics(username));
            else
                return Json(null);
        }

        [Route("api/Analytics/Save")]
        [HttpPost]
        public void SaveAnalytics(int deck_id) // Сохранение аналитики после повторения карточек колоды
        {
            if (_deckService.AuthorizeUserDeck(User.Identity.Name, deck_id)) // защита от изменения данных аналитики колоды пользователем, которому данная колода не принадежит
                _analyticsService.SaveAnalytics(deck_id);
        }
    }
}