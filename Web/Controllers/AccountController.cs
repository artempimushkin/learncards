using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Services;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        // Обеспечивает работу страниц регистрации, авторизации и настройки аккаунта пользователя
        const string LoginView = "Views/Account/Login.cshtml";
        const string RegisterView = "Views/Account/Register.cshtml";
        const string AccountView = "Views/Account/Account.cshtml";

        private IUserService _userService;

        public AccountController(IUserService userService) //конструктор с инициализацией контекста entity framework
        {
            _userService = userService;
        }

        [Authorize]
        [Route("Account")]
        public ActionResult Index() // Получение страницы настроек аккаунта
        {
            return View(AccountView, new RegisterDto { Username = User.Identity.Name, Password = "", ConfirmPassword = "" });
        }

        [Authorize]
        [Route("Account/Update")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(RegisterDto model) // Обновление настроек аккаунта
        {
            if (ModelState.IsValid)
            {
                // если пользователь не менял свой юзернейм или если новый юзернейм никто не использует
                if ((model.Username == User.Identity.Name) || !_userService.Exist(model.Username))
                {
                    // создаем нового пользователя
                    // проверяем действительно ли пользователь удачно обновлен в бд
                    if (_userService.Update(User.Identity.Name, model))
                    {
                        new AuthService(HttpContext).Authenticate(model.Username).Wait(); // аутентификация с целью обновления кук
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Такое имя пользователя занято");
                }
            }

            return View(AccountView, model);
        }

        [Route("Account/Login")]
        public ActionResult Login() // Получение страницы входа в аккаунт
        {
            return View(LoginView, new UserDto());
        }

        [Route("Account/Login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(UserDto model) // Авторизация пользователя
        {
            if (ModelState.IsValid)
            {
                if (_userService.Exist(model)) // если нашли, то записали в куки
                {
                    new AuthService(HttpContext).Authenticate(model.Username).Wait(); // аутентификация
                    return RedirectToAction("Index", "Home");
                }
                else // если не нашли
                {
                    ModelState.AddModelError("", "Неверное имя пользователя/пароль");
                }
            }

            return View(LoginView, model);
        }

        [Route("Account/Register")]
        public ActionResult Register() // Получение страницы регистрации
        {
            return View(RegisterView, new RegisterDto());
        }

        [Route("Account/Register")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterDto model) // Регистрация пользователя
        {
            if (ModelState.IsValid)
            {
                if (!_userService.Exist(model.Username)) // если не нашли, то регистрируем
                {
                    // создаем нового пользователя
                    // проверяем действительно ли пользователь удачно добавлен в бд
                    if (_userService.Create(model))
                    {
                        new AuthService(HttpContext).Authenticate(model.Username).Wait(); // аутентификация
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с таким именем уже существует");
                }
            }

            return View(RegisterView, model);
        }

        [Route("Account/Logout")]
        public async Task<IActionResult> Logout() // Выход пользователя из аккаунта
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

    }
}