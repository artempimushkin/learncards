
$(document).ready(function () {

    $(document).mouseup(function (e) {
        // закрывает меню аккаунта при клике на любой элемент
        if (e.target.id !== "img-account-icon" /*&& e.target.className !== "three-dots-menu-item" && e.target.className !== "three-dots-menu"*/) {
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
    $("#btn-close-analytics").click(function (event) {
        location.href = window.location.origin;
    });

    function DeckAnalyticsNode(analytics, id) {
        let element = ` <div class="deck-analytics">
                            <div class="deck-name-analytics">${analytics.deckName}</div>
                            <div class="analytics-text">Дата последнего повторения: ${analytics.labels[analytics.labels.length - 1]}</div>
                            <div class="analytics-text">Текущий уровень знания: ${analytics.data[analytics.data.length - 1]}%</div>
                            <canvas id="${id}" class="chart-canvas"></canvas>
                        </div>`;
        return element;
    }

    function ShowDecksAnalytics(analytics) {
        let NoDataDecks = "";
        for (let i = 0; i < analytics.length; i++) {
            if (analytics[i].labels.length > 1) { // если аналитика есть, то показываем график, иначе выводим сообщение
                $("#analytics-list").append(DeckAnalyticsNode(analytics[i], i));

                new Chart(document.getElementById(i), {
                    type: 'line',
                    data: {
                        labels: analytics[i].labels,
                        datasets: [{
                            data: analytics[i].data,
                            label: "Уровень знаний (%)",
                            borderColor: "#2196F3",
                            fill: 'start',
                            pointBackgroundColor: "#2196F3",
                            backgroundColor: "rgba(33, 150, 243, 0.1)"
                        }]
                    },
                    options: {
                        title: {
                            display: false
                        },
                        tooltips: {
                            enable: false
                        },
                        legend: false
                    }
                });
            }
            else {
                NoDataDecks += `<div class="deck-analytics">
                                    <div class="deck-name-analytics">${analytics[i].deckName}</div>
                                    <div class="no-analytics-message">Недостаточно данных</div>
                                </div>`;             
            }                
        }

        if (NoDataDecks.length > 0) {
            $("#analytics-list").append(NoDataDecks);
        }
    }

    OnLoad();
    function OnLoad() {
        let str = window.location.pathname;
        let Username = "";
        if (str.includes('analytics')) {
            Username = str.substring(str.lastIndexOf('/') + 1);
        }

        if (Username !== 'analytics') {
            GetDecksAnalytics(Username);
        }
        else {
            GetOwnDecksAnalytics();
        }
    }

    function GetDecksAnalytics(Username) {
        $.ajax({
            url: window.location.origin + '/api/Analytics/GetAnalytics',
            type: 'GET',
            data: { username: Username },
            success: function (analytics) {
                ShowDecksAnalytics(analytics);
            }
        });
    }

    function GetOwnDecksAnalytics() {
        $.ajax({
            url: window.location.origin + '/api/Analytics/GetOwnAnalytics',
            type: 'GET',
            success: function (analytics) {
                ShowDecksAnalytics(analytics);
            }
        });
    }
});
