
$(document).ready(function () {

    OnLoad();
    function OnLoad() {
        if ($("#decklist").length > 0) { // если элемента decklist нет, значит пользователь не найден и запрос делать не надо
            LoadDeckList();
        }   

        //--------------------------- Кнопки аккаунта и логотипа -----------------------------------
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

        $("#become-student-button").click(function () {
            var str = window.location.pathname;
            let Username = "";
            if (str.includes('profile')) {
                Username = str.substring(str.lastIndexOf('/') + 1);
            }

            GrantAccessRights(Username);
        });

        $("#stop-being-a-student-button").click(function () {
            var str = window.location.pathname;
            let Username = "";
            if (str.includes('profile')) {
                Username = str.substring(str.lastIndexOf('/') + 1);
            }

            DeleteAccessRights(Username);
        });
    }

    function LoadDeckList() {
        var str = window.location.pathname;
        let Username = "";
        if (str.includes('profile')) {
            Username = str.substring(str.lastIndexOf('/') + 1);
        }

        $.ajax({
            url: '/api/Profile/GetDeckList',
            type: 'GET',
            data: {
                username: Username
            },
            success: function (DeckList) {

                if (DeckList !== null) {
                    if (DeckList.length == 2) { $(".gridbox").css("grid-template-columns", "1fr 1fr"); }
                    else if (DeckList.length == 1) { $(".gridbox").css("grid-template-columns", "1fr"); }

                    DeckList.forEach(function (element) {
                        $("#decks-table").append(DeckNode(element));
                    });

                    $(".copy-deck-button").click(CopyDeck);
                    $(".wrapper").show();
                }
                else {
                    //$("#profile-not-found-wrapper").show();
                    $(".no-decks-message").show();
                    $("#decks-table").hide();
                }
            }
        });
    }

    function DeckNode(Deck) {
        let str = `<div id="${Deck.deckId}" class="deck">
                        <div class="deck-icon" style = "background-color: #${Deck.colorCode}">${Deck.deckName[0]}</div>
                        <div class="deck-info">
                            <div class="deck-name">${Deck.deckName}</div>
                            <div class="deck-cardsnumber"><span class="cards-number">${Deck.cardNumber}</span> карточек</div>
                            <div class="copy-deck-button">Скопировать</div>
                        </div>
                    </div>`;
        return str;
    }

    function CopyDeck(event) {

        $.ajax({
            url: '/api/Profile/CopyDeck',
            type: 'Post',
            data: {
                deck_id: event.target.parentNode.parentNode.id
            },
            success: function (result) {
                if (result) {
                    $(event.target).html("Скопировано");
                    $(event.target).css("background-color", "#dcfae2");
                    $(event.target).css("color", "#02b32a");
                }
                else {
                    $(event.target).html("Server error");
                    $(event.target).css("background-color", "#fac1c1");
                    $(event.target).css("color", "#c33");
                }
            }
        });

        
    }

    function GrantAccessRights(Username) {
        $.ajax({
            url: '/api/Profile/GrantAccessRights',
            type: 'POST',
            data: {
                username: Username
            },
            success: function (result) {
                if (result) {
                    $("#become-student-button").toggle();
                    $("#stop-being-a-student-button").toggle();
                }
            }
        });
    }

    function DeleteAccessRights(Username) {
        $.ajax({
            url: '/api/Profile/DeleteAccessRights',
            type: 'POST',
            data: {
                username: Username
            },
            success: function (result) {
                if (result) {
                    $("#become-student-button").toggle();
                    $("#stop-being-a-student-button").toggle();
                }
            }
        });
    }
});

