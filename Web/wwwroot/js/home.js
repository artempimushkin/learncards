var SheduledForDelete; // html-элемент колоды, которая планируется к удалению
var CurrentDeckId;
var CurrentDeck;

var Languages; // список языков
var DeckList;

$(document).ready(function () {

    $(document).mouseup(function (e) {
        // закрывает меню колоды при клике на другой элемент
        if (e.target.className !== "three-dots-image") {
            $(".three-dots-menu").hide();
        }

        // закрывает меню аккаунта при клике на любой элемент
        if (e.target.id !== "img-account-icon") {
            $("#account-menu").hide();
        }
    });

    OnLoad();
    function OnLoad() {
        // получение списка языков (необходимо в добавлении новой колоды, изменения параметров существующих колод)
        $.ajax({
            url: '/api/GetLanguageList',
            type: 'GET',
            success: function (languages) {
                Languages = languages;

                // добавление списка в форму создания и редактирования колоды
                $.each(Languages, function (i, item) {
                    $('#CreateNewDeck_LanguageCode').append($('<option>', {
                        value: item.languageCode,
                        text: item.languageName
                    }));
                });

                $.each(Languages, function (i, item) {
                    $('#DeckSettings_LanguageCode').append($('<option>', {
                        value: item.languageCode,
                        text: item.languageName
                    }));
                });
            }
        });

        LoadDeckList();

        //--------------------------- Кнопки аккаунта и логотипа -----------------------------------

        $("#img-account-icon").click(ShowAccountMenu);
        //$("#label-logo").click(function (event) {
        //    location.href = window.location.origin;
        //});

        $("#account-button").click(function () {
            location.href = "/Account";
        });
        $("#analytics-button").click(function () {
            location.href = "/analytics";
        });
        $("#teachers-button").click(function () {
            location.href = "/GivenAccess";
        });
        $("#students-button").click(function () {
            location.href = "/TakenAccess";
        });

        //---------- Создание новой карточки -------------
        $("#btn-close-add-card").click(CloseFormAddCard);
        $("#btn-close-add-card-2").click(CloseFormAddCard);
        $("#AddCardButton").click(AddCard);
        //================================================
        //---------- Изменение настроек ------------------
        $("#btn-close-deck-settings").click(CloseFormDeckSettings);
        $("#btn-close-deck-settings-2").click(CloseFormDeckSettings);
        $("#SaveDeckSettingsButton").click(SaveDeckSettings);
        //================================================
        //---------- Удаление колоды ---------------------
        $("#btn-cancel-delete-deck").click(CloseFormDeleteDeck);
        $("#btn-delete-deck").click(DeleteDeck);
        //================================================
    }

    function ShowAccountMenu() {
        element = $(this.parentNode).find("#account-menu");
        if ($(element).is(":visible")) {
            $(element).hide();
        }
        else {
            $("#account-menu").hide();
            $(element).show();
        }
    }

    function LoadDeckList() {
        $.ajax({
            url: '/api/GetDeckList',
            type: 'GET',
            success: function (DeckList) {
                $(".deck").remove(); // удаление всех старых элементов
                $(".no-decks-message").remove();
                if (DeckList.length > 0) {
                    DeckList.forEach(function (element) {
                        $("#decklist").append(DeckNode(element));
                    });

                    InitializeDeckButtonsHandlers();
                }
                else {
                    $("#decklist").append(`<div class="no-decks-message">У вас нет ни одной колоды</div>`);
                }
            }
        });
    }

    function DeckNode(Deck) { // создает строку html-кодом колоды
        let CardsToRepeat = "";
        let MobileCardsToRepeat = "";
        if (Deck.cardsToRepeat > 0) {
            CardsToRepeat = `<div class="cards-to-repeat">${Deck.cardsToRepeat}  карточек к повторению</div>`;
            MobileCardsToRepeat = `<div class="cards-to-repeat-mobile">${Deck.cardsToRepeat}  карточек к повторению</div>`;
        }

        let str = `<div id="${Deck.deckId}" class="deck">
                        <div class="deck-icon" style = "background-color: #${Deck.colorCode}">${Deck.deckName[0]}</div>
                        <div class="deck-info">
                            <div class="deck-name">${Deck.deckName}</div>
                            <div class="deck-cardsnumber"><span class="cards-number">${Deck.cardNumber}</span> карточек</div>
                            ${MobileCardsToRepeat}
                            <div class="deck-buttons">
                                <span class="deck-buttons-button menu-button-add">добавить</span>
                                <span class="deck-buttons-button menu-button-delete">удалить</span>
                                <span class="deck-buttons-button menu-button-settings">настройки</span>
                            </div>
                        </div>
                        ${CardsToRepeat}
                        <div class="three-dots">
                            <img class="three-dots-image" src="/images/dots.svg" />
                            <div class="three-dots-menu" style="display:none">
                                <div class="three-dots-menu-item menu-button-add">добавить карточку</div>
                                <div class="three-dots-menu-item menu-button-delete">удалить колоду</div>
                                <div class="three-dots-menu-item menu-button-settings">настройки</div>
                            </div>
                        </div>
                    </div>`;
        return str;
    }
    //--------------------------- Создание новой карточки -----------------------------------
    function ShowFormAddCard() { // открывает форму добавления новой карточки в колоду
        $("#AddCardForm_ErrorMessage").empty(); // очистка сообщений
        $("#AddCardForm_SuccessMessage").empty();

        $("#AddCardForm").toggle();
        $("#btn-close-add-card-2").show();
        CurrentDeckId = this.parentNode.parentNode.parentNode.id; // запоминаем id этой колоды. Это нужно, чтобы знать в какую колоду сохранять
    }
    function CloseFormAddCard() { // закрывает форму добавления новой карточки
        $("#AddCardForm").toggle();
        $("#btn-close-add-card-2").hide();
    }
    function AddCard() { // отправляет запрос на добавление новой карточки в колоду
        var element = document.getElementById("input-card-front"); // получаем элемент, содержащий передний текст карточки, чтобы проверить на ошибки
        var NoError = true; // это флаг ошибки
        if ($(element).val().length < 1) {
            InputError(element); // выделаем поле ввода красным цветом
            NoError = false;
        }
        element = document.getElementById("input-card-back"); // получаем элемент, содержащий задний текст карточки, чтобы проверить на ошибки
        if ($(element).val().length < 1) {
            InputError(element); // выделаем поле ввода красным цветом
            NoError = false;
        }

        if (NoError) { // если ошибок нет

            FixInputError(document.getElementById("input-card-front")); // надо убрать выделение красным
            FixInputError(document.getElementById("input-card-back"));

            $.ajax({
                url: '/api/AddCard',
                type: 'POST',
                data: {
                    front: $("#input-card-front").val(),
                    back: $("#input-card-back").val(),
                    deck_id: CurrentDeckId
                },
                success: function (result) {// result - это булева переменная, обозначающая записалась ли карточка в БД
                    if (result) {
                        $("#AddCardForm_ErrorMessage").empty(); // очистка сообщения об ошибке
                        $("#AddCardForm_SuccessMessage").html("Добавлено"); // добавляет зеленую надпись "success" над кнопкой, а потом через 2с исчезает
                        setTimeout(function () { $("#AddCardForm_SuccessMessage").empty(); }, 2000);

                        $("#input-card-front").val("");
                        $("#input-card-back").val("");

                        //// код ниже увеличивает у колоды цифру количества карт. 
                        //// Это нужно, чтобы показать, что карта добавлена и чтобы работала функция пускающая в тренировку, только если у колоды прописано число больше нуля
                        //var numbers = document.getElementsByClassName('cards-number');
                        //var notnull = false;
                        //for (var i = 0; i < numbers.length; i++) {
                        //    if (numbers[i].parentNode.parentNode.parentNode.id === CurrentDeckId) {
                        //        var num = (numbers[i].innerHTML * 1) + 1;
                        //        numbers[i].innerHTML = num;
                        //        break;
                        //    }
                        //}
                        LoadDeckList();
                    }
                    else {
                        $("#AddCardForm_ErrorMessage").html("Ошибка сервера"); // добавляет красную надпись "error" над кнопкой
                    }
                }
            });
        }
    }
    //=================================================================================
    //--------------------------- Изменение настроек -----------------------------------
    function ShowFormDeckSettings() { // загружает параметры колоды и показывает их на форме, в которой их можно редактировать
        $("#DeckSettings_ErrorMessage").empty(); // очистка сообщений
        $("#DeckSettings_SuccessMessage").empty();

        // очистка всех полей
        $("#DeckSettings_DeckName").val("");
        $("#DeckSettings_IsPublic").prop('checked', false);

        $("#DeckSettings").toggle();
        $("#btn-close-deck-settings-2").show();
        CurrentDeckId = this.parentNode.parentNode.parentNode.id;

        $.ajax({
            url: '/api/GetDeckSettings',
            type: 'GET',
            data: { deck_id: CurrentDeckId },
            success: function (Deck) {
                CurrentDeck = Deck;
                $("#DeckSettings_DeckName").val(Deck.deckName);
                $("#DeckSettings_LanguageCode").val(Deck.languageCode);
                $("#DeckSettings_IsPublic").prop('checked', Deck.isPublic);
            }
        });
    }
    function CloseFormDeckSettings() { // закрывает форму изменения настроек
        $("#DeckSettings").toggle();
        $("#btn-close-deck-settings-2").hide();
    }
    function SaveDeckSettings() { // сохранение настроек из формы после нажатия кнопки "Save"

        var NewDeck = {}
        NewDeck.deckName = $("#DeckSettings_DeckName").val();
        NewDeck.languageCode = $("#DeckSettings_LanguageCode").val();
        NewDeck.isPublic = $("#DeckSettings_IsPublic").prop("checked");

        if ((CurrentDeck.deckName === NewDeck.deckName) // проверка того, что настройки колоды изменялись
            && (CurrentDeck.languageCode === NewDeck.languageCode)
            && (CurrentDeck.isPublic === NewDeck.isPublic))
            return;

        let element = document.getElementById("DeckSettings_DeckName"); // это поле будет подчеркиваться красным, если оно пусто

        let NoError = true; // это флаг ошибки
        if (NewDeck.deckName.length < 1) {
            InputError(element); // выделаем поле ввода красным цветом
            NoError = false;
        }

        if (NoError) {
            FixInputError(element); // убирает выделение красным

            $.ajax({
                url: '/api/UpdateDeckSettings',
                type: 'Post',
                data: {
                    DeckId: CurrentDeckId,
                    DeckName: NewDeck.deckName,
                    LanguageCode: NewDeck.languageCode,
                    IsPublic: NewDeck.isPublic
                },
                success: function (result) {
                    if (result) {
                        //$("#DeckSettings").toggle();
                        $("#DeckSettings_ErrorMessage").empty(); // очистка сообщения об ошибке
                        $("#DeckSettings_SuccessMessage").html("Сохранено"); // добавляет зеленую надпись "success" над кнопкой, а потом через 2с исчезает
                        setTimeout(function () { $("#DeckSettings_SuccessMessage").empty(); }, 2000);

                        document.getElementById(CurrentDeckId).childNodes[3].childNodes[1].innerHTML = NewDeck.deckName;

                        CurrentDeck = NewDeck;
                    }
                    else {
                        $("#DeckSettings_ErrorMessage").html("Ошибка сервера"); // добавляет красную надпись "error" над кнопкой
                    }
                }
            });
        }
    }
    //=================================================================================
    //--------------------------- Удаление колоды -----------------------------------
    function ShowFormDeleteDeck() { // показывает диалог с подтверждением удаления колоды
        // сначала функция (после клика по "delete") записывает предполагаемый элемент в переменную SheduledForDelete
        // потом функция (после подтверждения удаления в окне) удаляет этот элемент и отправляет ajax запрос об удалении его из БД
        SheduledForDelete = document.getElementById(this.parentNode.parentNode.parentNode.id);
        $("#DeleteDeckForm").toggle();
    }
    function CloseFormDeleteDeck() { // закрывает диалог с подтверждением удаления колоды
        $("#DeleteDeckForm").toggle();
    }
    function DeleteDeck() { // удаляет колоды из БД и со страницы
        $("#DeleteDeckForm").toggle();
        SheduledForDelete.parentElement.removeChild(SheduledForDelete);

        $.ajax({
            url: '/api/DeleteDeck',
            type: 'POST',
            data: { id: SheduledForDelete.id },
            success: function () {
                //$("#" + SheduledForDelete.id).remove(); // удаление колоды со страницы
                LoadDeckList();
            }
        });
    }
    //=================================================================================

    function DeckClick(event) { // открывает страницу с тренировкой, если в колоде есть хотя бы 1 карточка
        //alert(event.target.className);
        if (this !== event.target)
            if (event.target.className !== "deck" && event.target.className !== "deck-cardsnumber" && event.target.className !== "cards-number"
                && event.target.className !== "deck-name" && event.target.className !== "deck-icon" && event.target.className !== "deck-info"
                && event.target.className !== "cards-to-repeat-mobile" && event.target.className !== "cards-to-repeat")
                return;

        // следующий код сделан для того, чтобы нельзя было открыть пустую колоду
        let numbers = document.getElementsByClassName('cards-number');
        let notnull = false;
        for (let i = 0; i < numbers.length; i++) {
            if (numbers[i].parentNode.parentNode.parentNode.id === this.id) {
                if (numbers[i].innerHTML > 0)
                    notnull = true;
                break;
            }
        }

        if (notnull) {
            window.location = "/training/" + this.id;
        }
    }

    function ShowDeckMenu() {
        //alert(this.parentNode.childNodes);

        element = $(this.parentNode).find(".three-dots-menu");
        if ($(element).is(":visible")) {
            $(element).hide();
        }
        else {
            $(".three-dots-menu").hide();
            $(element).show();
        }

    }

    function InitializeDeckButtonsHandlers() { // назначение обрабаотчиков событий для колоды, после того как все колоды прогрузились        
        $(".menu-button-add").click(ShowFormAddCard);
        $(".menu-button-settings").click(ShowFormDeckSettings);
        $(".menu-button-delete").click(ShowFormDeleteDeck);
        $(".three-dots-image").click(ShowDeckMenu);
        $(".deck").click(DeckClick);
    }

    //-------------------------- Создание новой колоды ------------------------------------
    $("#plus-button").click(function () { // показывает форму создания новой колоды
        $("#NewDeckForm_ErrorMessage").empty(); // очистка сообщений
        $("#NewDeckForm_SuccessMessage").empty();

        $("#CreateNewDeck_DeckName").val(""); // очистка всех полей
        $("#CreateNewDeck_IsPublic").prop('checked', false);

        $("#NewDeckForm").toggle();
        $("#btn-close-new-deck-2").toggle();
    });
    $("#btn-close-new-deck").click(function () { // заклывает форму создания новой колоды
        $("#NewDeckForm").toggle();
        $("#btn-close-new-deck-2").hide();
    });
    $("#btn-close-new-deck-2").click(function () { // заклывает форму создания новой колоды
        $("#NewDeckForm").toggle();
        $("#btn-close-new-deck-2").hide();
    });
    $("#CreateDeckButton").click(function () {
        let element = document.getElementById("CreateNewDeck_DeckName"); // получаем элемент, содержащий название колоды, чтобы проверить на ошибки
        let NoError = true; // это флаг ошибки
        if ($(element).val().length < 1) {
            InputError(element); // выделаем поле ввода красным цветом
            NoError = false;
        }

        if (NoError) { // если ошибок нет

            FixInputError(element); // убирает выделение красным

            $.ajax({
                url: '/api/CreateNewDeck',
                type: 'POST',
                data: {
                    DeckName: $("#CreateNewDeck_DeckName").val(),
                    LanguageCode: $("#CreateNewDeck_LanguageCode").val(),
                    IsPublic: $("#CreateNewDeck_IsPublic").prop("checked"),
                },
                success: function (result) {
                    //$("#NewDeckForm").toggle();
                    if (result) {
                        $("#NewDeckForm_ErrorMessage").empty(); // очистка сообщения об ошибке
                        $("#NewDeckForm_SuccessMessage").html("Создано"); // добавляет зеленую надпись "success" над кнопкой, а потом через 2с исчезает
                        setTimeout(function () { $("#NewDeckForm_SuccessMessage").empty(); }, 2000);

                        $("#CreateNewDeck_DeckName").val(""); // очистка всех полей
                        $("#CreateNewDeck_IsPublic").prop('checked', false);

                        LoadDeckList();
                    }
                    else {
                        $("#NewDeckForm_SuccessMessage").html("Ошибка сервера"); // добавляет красную надпись "error" над кнопкой
                    }
                }
            });
        }
    });
    //=================================================================================

    function InputError(element) {
        element.style.borderBottom = "2px";
        element.style.borderStyle = "solid";
        element.style.borderColor = "#c33";
    }

    function FixInputError(element) {
        element.style.borderBottom = "0.5px";
        element.style.borderStyle = "solid";
        element.style.borderColor = "#ccc";
    }
});

