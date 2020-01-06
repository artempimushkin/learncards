var SheduledForDelete;
var CurrentDeckId;
var CurrentCard;
var IsCurrentCardInverted;
var DeckLanguage;
var ContextCardAnswer;

var Answer = false;
var DidAnswer; // флаг, показывающий, была ли отвечена хоть одна карточка, иначе сохранять аналитику нельзя
var CurrentURI;

$(document).ready(function () {
    $("#text-back").hide();
    $("#text-front").show();
    $("#card-buttons").hide();

    $(document).mouseup(function (e) {
        // закрывает меню аккаунта при клике на любой элемент
        if (e.target.id !== "img-account-icon") {
            $("#account-menu").hide();
        }
    });

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

    $("#flip-card").click(function(event) {
        if (this !== event.target) return;

        FlipCardClick();
    });

    function FlipCardClick() {
        $("#flip-card-text-front").parent().toggle();
        $("#flip-card-text-back").toggle();
        //$("#audio-button-flip-card").toggle();
    }

    $("#btn-forgot-card").click(function () {
        PostAnswer(0);
    });

    $("#btn-remember-card").click(function () {
        PostAnswer(1);
    });

    $("#btn-answer-card").click(function () {
        let answer = $("#input-card-answer").val();
   
        //----------
        let first = answer.split('.').join("");
        first = answer.split('  ').join(" ");
        first = answer.split(',').join("");
        first = answer.split('-').join("");
        first = answer.split(':').join("");
        first = answer.split('\'').join("");
        first = answer.split('"').join("");
        first = answer.split('!').join("");
        first = answer.split('?').join("");
        let ch = answer.charAt(answer.length-1);

        if (ch === ' ' || ch === ',' || ch === '-' || ch === '|' || ch === ':' || ch === ';' || ch === '!' || ch === '?' || ch === '"') {
            first = answer.substr(0, answer.length - 1);
        }

        let RightAnswer = CurrentCard.front;
        if (CurrentCard.type === "context-card")
            RightAnswer = ContextCardAnswer;

        let second = RightAnswer.split('.').join("");
        second = RightAnswer.split('  ').join(" ");
        second = RightAnswer.split(',').join("");
        second = RightAnswer.split('-').join("");
        second = RightAnswer.split(':').join("");
        second = RightAnswer.split('\'').join("");
        second = RightAnswer.split('"').join("");
        second = RightAnswer.split('!').join("");
        second = RightAnswer.split('?').join("");
        //----------

        if (first.toLowerCase() === second.toLowerCase()) {
            Answer = true;
            $("#answer-result-right").show();
        }
        else {
            Answer = false;
            $("#right-answer").html('Правильный ответ:  ' + RightAnswer);
            $("#answer-result-wrong").show();
        }

        $("#btn-answer-card").hide();
        $("#btn-next-card").show();

    });

    $("#btn-next-card").click(function () {     

        if (Answer) {
            PostAnswer(1);
        }
        else {
            PostAnswer(0);
        }
    });

    function PostAnswer(answer) {
        DidAnswer = true;
        $.ajax({
            url: window.location.origin + '/api/Training/Answer',
            type: 'POST',
            data: { card_id: CurrentCard.cardId, result: answer },
            success: function () {
                GetNextCard();
            }
        });
    };

    function GetNextCard() {
        $.ajax({
            url: window.location.origin + '/api/Training/GetNextCard',
            type: 'GET',
            data: {
                deck_id: CurrentDeckId,
                languageCode: DeckLanguage
            },
            success: function (card) {
                CurrentCard = card;

                if (CurrentCard !== null) { // если карточка пришла
                    switch (CurrentCard.type) {
                        case 'flip-card':
                            ShowFlipCard(CurrentCard);
                            break;
                        case 'type-card':
                            ShowTypeCard(CurrentCard);
                            break;
                        case 'audio-card':
                            ShowAudioCard(CurrentCard);
                            break;
                        case 'context-card':
                            ShowContextCard(CurrentCard);
                            break;
                    }

                }
                else {                  
                    $("#card-container").hide();
                    $("#end-of-cards-message-wrapper").show();

                    if (DidAnswer) { // если тренировка окончилась и она на самом деле происходила (т.е. пользователь ответил хотя бы на один вопрос)
                        $.ajax({
                            url: window.location.origin + '/api/Analytics/Save',
                            type: 'POST',
                            data: {
                                deck_id: CurrentDeckId
                            }
                        });
                    }
                }
            },
            error: function () { // если произошла ошибка, то пользователя выбрасывает на главную страницу
                location.href = "/";
            }
        });
    } 

    function ShowFlipCard(card) {// прячем все лишние элементы и показываем перенюю часть карточки
        $("#type-card").hide();
        $("#btn-answer-card").hide();
        $("#btn-next-card").hide();
        $("#answer-result-right").hide();
        $("#answer-result-wrong").hide();

        $("#flip-card").show();
        $("#flip-card-buttons").show();

        $("#card-question").html("Нажмите на карточку, чтобы перевернуть ее");

        document.getElementById("flip-card-text-front").textContent = CurrentCard.front;
        document.getElementById("flip-card-text-back").textContent = CurrentCard.back;

        $("#card-container").show();
        $("#flip-card").show();
        $("#flip-card-text-back").hide();
        $("#flip-card-text-front").parent().show();
        $("#flip-card-buttons").show();

        if (Math.floor(Math.random() * 10) % 2 === 0) {
            IsCurrentCardInverted = true;
            FlipCardClick();
            //document.getElementById("flip-card-text-front").textContent = CurrentCard.back;
            //document.getElementById("flip-card-text-back").textContent = CurrentCard.front;
            //$("#audio-button-flip-card").hide();
        }
        else {
            IsCurrentCardInverted = false;
            //document.getElementById("flip-card-text-front").textContent = CurrentCard.front;
            //document.getElementById("flip-card-text-back").textContent = CurrentCard.back;
            //$("#audio-button-flip-card").show();
        }
     
        //$("#card-container").show();
        //$("#flip-card").show();
        //$("#flip-card-text-back").hide();
        //$("#flip-card-text-front").parent().show();
        //$("#flip-card-buttons").show();
    }

    function ShowTypeCard(card) {
        $("#card-question").html("Введите переднюю часть карточки");
        $("#input-card-answer").val("");
        $("#btn-next-card").hide();
        $("#answer-result-right").hide();
        $("#answer-result-wrong").hide();
        
        //document.getElementById("type-card-text").textContent = CurrentCard.back;
        $("#type-card-text").html(CurrentCard.back);

        $("#card-container").show();
        $("#flip-card").hide();
        $("#flip-card-buttons").hide();

        $("#type-card").show();
        $("#btn-answer-card").show();
        $("#audio-button").hide();
    }

    function PrepareSentence(sentence, back) {
        let mas = sentence.split("'''");
        ContextCardAnswer = mas[1];
        return mas[0] + `<span class="context-word">[` + back + `]</span>` + mas[2];
    }
    function ShowContextCard(card) {
        $("#card-question").html("Введите подходящее слово");
        $("#input-card-answer").val("");
        $("#btn-next-card").hide();
        $("#answer-result-right").hide();
        $("#answer-result-wrong").hide();

        $("#type-card-text").html(PrepareSentence(card.sentence, card.back) );

        $("#card-container").show();
        $("#flip-card").hide();
        $("#flip-card-buttons").hide();

        $("#type-card").show();
        $("#btn-answer-card").show();
        $("#audio-button").hide();
    }

    function ShowAudioCard(card) {
        $("#card-question").html("Введите услышанное слово(а)");
        $("#input-card-answer").val("");
        $("#btn-next-card").hide();
        $("#answer-result-right").hide();
        $("#answer-result-wrong").hide();

        $("#type-card-text").html("Воспроизвести аудио");

        $("#card-container").show();
        $("#flip-card").hide();
        $("#flip-card-buttons").hide();

        $("#type-card").show();
        $("#btn-answer-card").show();
        $("#audio-button").show();
    }

    $("#audio-button").click(function () {
        $('audio #source').attr('src', window.location.origin + "/api/Training/Speech?query=" + CurrentCard.front + "&languageCode=" + DeckLanguage);
        $('audio').get(0).load();
        $('audio').get(0).play();
        //https://localhost:44372/api/Training/Speech?query=zero&languageCode=en-US
    });

    $("#audio-button-flip-card").click(function () {
        $('audio #source').attr('src', window.location.origin + "/api/Training/Speech?query=" + CurrentCard.front + "&languageCode=" + DeckLanguage);
        $('audio').get(0).load();
        $('audio').get(0).play();
        //https://localhost:44372/api/Training/Speech?query=zero&languageCode=en-US
    });
    
    OnTrainingLoad(); // call function when page loaded
    function OnTrainingLoad() {

        let str = window.location.pathname;
        if (str.includes('training')) {
            CurrentDeckId = str.substring(str.lastIndexOf('/') + 1);

            $.ajax({
                url: window.location.origin + '/api/GetDeckSettings',
                type: 'GET',
                data: { deck_id: CurrentDeckId },
                success: function (Deck) {
                    DeckLanguage = Deck.languageCode;
                    GetNextCard();
                }
            });
        }
    }

    $("#close-training").click(function (event) {
        location.href = "/";
    });
    $("#close-training-2").click(function (event) {
        location.href = "/";
    });

    $("#message-close-training").click(function (event) {
        location.href = "/";
    });

    $("#card-delete-button").click(function () {
        $("#DeleteCardForm").toggle();
    });

    $("#btn-cancel-delete-card").click(function () {
        $("#DeleteCardForm").toggle();
    });

    $("#btn-delete-card").click(function () {
        $.ajax({
            url: window.location.origin + '/api/Training/DeleteCard',
            type: 'POST',
            data: { id: CurrentCard.cardId },
            success: function () {
                $("#DeleteCardForm").toggle();
                GetNextCard();
            }
        });
    });

    $("#card-settings-button").click(function () {
        $("#CardSettings").toggle();
        $("#close-training-2").hide();
        $("#btn-close-card-settings-2").show();

        $("#input-card-front").val(CurrentCard.front);
        $("#input-card-back").val(CurrentCard.back);

        $("#CardSettings_SuccessMessage").empty();
        $("#CardSettings_ErrorMessage").empty();

        FixInputError(document.getElementById("input-card-front"));
        FixInputError(document.getElementById("input-card-back"));
    });

    $("#btn-close-card-settings").click(function () {
        $("#CardSettings").toggle();
        $("#btn-close-card-settings-2").hide();
        $("#close-training-2").hide();
    });

    $("#btn-close-card-settings-2").click(function () {
        $("#CardSettings").toggle();
        $("#btn-close-card-settings-2").hide();
        $("#close-training-2").show();
    });

    $("#SaveCardSettingsButton").click(function () {

        if ((CurrentCard.front === $("#input-card-front").val()) && (CurrentCard.back === $("#input-card-back").val())) return; // проверка, что карточка была изменена

        var element = document.getElementById("input-card-front");
        var flag = true;
        if ($(element).val().length < 1) {
            InputError(element);
            flag = false;
        }
        element = document.getElementById("input-card-back");
        if ($(element).val().length < 1) {
            InputError(element);
            flag = false;
        }

        if (flag) {
            FixInputError(document.getElementById("input-card-front"));
            FixInputError(document.getElementById("input-card-back"));

            $.ajax({
                url: window.location.origin + '/api/Training/UpdateCard',
                type: 'POST',
                data: {
                    card_id: CurrentCard.cardId,
                    front: $("#input-card-front").val(),
                    back: $("#input-card-back").val()
                },
                success: function (result) {
                    if (result) {
                        //$("#CardSettings").toggle();
                        $("#CardSettings_ErrorMessage").empty(); // очистка сообщения об ошибке
                        $("#CardSettings_SuccessMessage").html("Сохранено"); // добавляет зеленую надпись "success" над кнопкой, а потом через 2с исчезает
                        setTimeout(function () { $("#CardSettings_SuccessMessage").empty(); }, 2000);

                        CurrentCard.front = $("#input-card-front").val();
                        CurrentCard.back = $("#input-card-back").val();

                        switch (CurrentCard.type) {
                            case 'flip-card':
                                if (IsCurrentCardInverted) {
                                    document.getElementById("flip-card-text-front").textContent = CurrentCard.back;
                                    document.getElementById("flip-card-text-back").textContent = CurrentCard.front;
                                }
                                else {
                                    document.getElementById("flip-card-text-front").textContent = CurrentCard.front;
                                    document.getElementById("flip-card-text-back").textContent = CurrentCard.back;
                                }

                                $("#flip-card-text-front").show();
                                $("#flip-card-text-back").hide();
                                break;
                            case 'type-card':
                                document.getElementById("type-card-text").textContent = CurrentCard.back;
                                break;
                        }

                    }
                    else {
                        $("#CardSettings_ErrorMessage").html("Ошибка сервера"); // добавляет красную надпись "error" над кнопкой
                    }
                }
            });
        }
    });
});
