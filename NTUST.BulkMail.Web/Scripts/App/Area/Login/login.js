(function () {
    "use strict";

    var baseUrl = $('#baseUrl').val();
    // 定義 modal 可以多層次開啟；最上層關閉後，底層 modal 可以維持 scroll
    $(document).on('hidden.bs.modal', '.modal', function () {
        $('.modal:visible').length && $(document.body).addClass('modal-open');
    });

    $(document).ready(function () {
    });

    var vm = new Vue({
        el: '#app',
        data: {
            user: {
                userId: "",
                password: "",
            }
        },
        methods: {
            Login() {
                Login();
            }
        },
        filters: {
        }
    })

    function Login() {
        $.ajax({
            url: baseUrl + 'Account/Login',
            type: "POST",
            async: true,
            cache: false,
            contentype: "application/json",
            datatype: "json",
            data: JSON.stringify(vm.user),
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            success: function (rdatas, textStatus, jqXHR) {
                window.location.href = rdatas.redirectToUrl;
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR);
                console.log(errorThrown);
            }
        });
    }
})();