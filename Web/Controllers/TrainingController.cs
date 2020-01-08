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
    public class TrainingController : Controller
    {
        // Обеспечивает работу страницы повторения карточек
        const string TrainingView = "Views/Training/Training.cshtml";  // html-файл страницы повторения карточек
        private ICardService _cardService;
        private ISpeachService _speachService;
        private IDeckService _deckService;

        public TrainingController(ICardService cardService, ISpeachService speachService, IDeckService deckService)
        {
            _cardService = cardService;
            _speachService = speachService;
            _deckService = deckService;
        }

        [Route("training/{id}")]
        [HttpGet]
        public ActionResult Index(int id) // Получение страницы тренировки
        {
            if (_deckService.AuthorizeUserDeck(User.Identity.Name, id)) // защита от просмотра и изменения колоды пользователем, которому она не принадлежит
                return View(TrainingView);
            else
                return RedirectToAction("Index", "Home");
        }

        [Route("api/Training/GetNextCard")]
        [HttpGet]
        public JsonResult GetNextCard(int deck_id) // Получение следующей карточки
        {
            return Json(_cardService.GetNextCard(deck_id));
        }

        [Route("api/Training/Answer")]
        [HttpPost]
        public void Answer(int card_id, int result) // Обновление индекса группы карточки
        {
            if (_cardService.AuthorizeUserCard(User.Identity.Name, card_id))  // защита от изменения карточки пользователем, которому она не принадлежит
            {
                if (result == 1) // правильный ответ
                    _cardService.KnowledgeLevelIncrement(card_id);
                else // неправильный ответ
                    _cardService.KnowledgeLevelSetNull(card_id);
            }
        }

        [Route("api/Training/DeleteCard")]
        [HttpPost]
        public void DeleteCard(int id) // Удаление карточки
        {
            if (_cardService.AuthorizeUserCard(User.Identity.Name, id))  // защита от удаления карточки пользователем, которому она не принадлежит
                _cardService.DeleteCard(id);
        }

        [Route("api/Training/UpdateCard")]
        [HttpPost]
        public bool UpdateCard(int card_id, String front, String back) // Изменение карточки
        {
            if (_cardService.AuthorizeUserCard(User.Identity.Name, card_id))  // защита от изменения карточки пользователем, которому она не принадлежит
                return _cardService.UpdateCard(card_id, front, back);
            else
                return false;
        }

        [Route("api/Training/Speech")]
        [HttpGet]
        public async Task<ActionResult> Speech(string query, string languageCode) // Получение аудио-файла озвученного слова (через api Amazon Polly)
        {
            try
            {
                byte[] audio = await _speachService.Speech(query, languageCode);
                return File(audio, "audio/mpeg", "audio.mp3");
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }
    }
}