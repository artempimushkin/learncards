var SheduledForDelete;

$(document).ready(function () {

    OnLoad();
    function OnLoad() {

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


        $("#btn-cancel-delete-deck").click(CloseFormDelete);
        $("#btn-delete-deck").click(DeleteAccess);

        LoadList();
    }

    function LoadList() {
        $.ajax({
            url: '/api/TakenAccess/GetList',
            type: 'GET',
            success: function (students) {
                if (students.length > 0) {
                    $("#studentslist").append(`<div class="student-list-title">Аналитика других пользователей</div>`);
                    students.forEach(function (element) {
                        $("#studentslist").append(Node(element));
                    });

                    $(".student-row").click(ClickRow);
                    $(".delete-student-button").click(ShowFormDelete);
                }
                else {
                    $("#studentslist").append(`<div class="no-students-message">Вам не предоставил доступ к аналитике ни один пользователь</div>`);
                }
            }
        });
    }

    function Node(student) {
        let str = `<div id="${student}" class="student-row">
                        ${student}
                        <img class="delete-student-button" src="/images/close.svg" />
                    </div>`;
        return str;
    }

    function ClickRow(event) {
        if (event.target.className !== "delete-student-button") {
            location.href = "/analytics/" + this.id;
        }
    }

    function ShowFormDelete() { // показывает диалог с подтверждением удаления студента
        // сначала функция (после клика по "delete") записывает предполагаемый элемент в переменную SheduledForDelete
        // потом функция (после подтверждения удаления в окне) удаляет этот элемент и отправляет ajax запрос об удалении его из БД
        SheduledForDelete = this.parentNode.id;
        $("#DeleteStudentForm").toggle();
    }
    function CloseFormDelete() { // закрывает диалог с подтверждением удаления студента
        $("#DeleteStudentForm").toggle();
    }

    function DeleteAccess() {
        $("#DeleteStudentForm").toggle();
        $.ajax({
            url: '/api/TakenAccess/DeleteAccess',
            type: 'POST',
            data: {
                username: SheduledForDelete
            },
            success: function () {
                $("#studentslist").html("");
                LoadList();
            }
        });
        //$("#" + SheduledForDelete).remove();
    }
});

