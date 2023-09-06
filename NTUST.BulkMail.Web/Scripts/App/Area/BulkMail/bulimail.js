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
            getData() {
                getData();
            }
        },
        methods: {
        },
        filters: {
        }
    })

    function getData() {
        $.ajax({
            url: baseUrl + 'Home/Test',
            type: "POST",
            async: true,
            cache: false,
            contentype: "application/json",
            datatype: "json",
            data: {
            },
            headers: {
                //'RequestVerificationToken': token
            },
            success: function (response, textStatus, jqXHR) {
                if (jqXHR.status === 200) {
                }
                else {
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    }
})();