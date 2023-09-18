(function () {
    "use strict";

    var baseUrl = $('#baseUrl').val();
    // 定義 modal 可以多層次開啟；最上層關閉後，底層 modal 可以維持 scroll
    $(document).on('hidden.bs.modal', '.modal', function () {
        $('.modal:visible').length && $(document.body).addClass('modal-open');
    });

    $(document).ready(function () {
        getData(1);
    });

    var vm = new Vue({
        el: '#app',
        data: {
            id: "eel6212",
            semester: "1121",
            pnowcondition: "01",
            pageList: {
                firstItemOnPage: 1,
                hasNextPage: true,
                hasPreviousPage: false,
                isFirstPage: true,
                isLastPage: false,
                lastItemOnPage: 20,
                pageCount: 0,
                pageNumber: 1,
                pageSize: 20,
                totalItemCount: 0,
                pageStart: 1,
                pageEnd: 1,
                itemList: []
            },
        },
        methods: {
            getNumbers: function (start, stop) {
                // For Page List 頁數計算
                var pageArray = [];
                for (var i = start; i <= stop; i++) {
                    pageArray.push(i);
                }
                return pageArray;
            },
            getData(pageIndex) {
                getData(pageIndex);
            },
            CreateStafferMember() {
                CreateStafferMember();
            },
            CreateStudentData() {
                CreateStudentData();
            }
        },
        filters: {
        }
    })

    function getData(pageIndex) {
        $.ajax({
            url: baseUrl + 'Home/List',
            type: "POST",
            async: true,
            cache: false,
            contentype: "application/json",
            datatype: "json",
            data: {
                pageIndex: pageIndex,
                id: vm.id,
                semester: vm.semester,
            },
            headers: {
                //'RequestVerificationToken': token
            },
            success: function (response, textStatus, jqXHR) {
                if (jqXHR.status === 200) {
                    if (response.resultMessage.Status == "OK") {
                        vm.pageList = response.pageListViewModel;
                        Vue.nextTick(function () {
                            $("#bulkmail").DataTable({
                                "responsive": true, "lengthChange": false, "autoWidth": false,
                                language: {
                                    url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/zh-HANT.json',
                                },
                                'iDisplayLength': 15,
                                "buttons": ["excel", "print", "colvis"],
                            }).buttons().container().appendTo('#bulkmail_wrapper .col-md-6:eq(0)');
                        })
                        //buildPageList();
                    }
                    else {
                        Swal.fire({
                            icon: 'error',
                            title: '連線逾時',
                            text: '請稍後再試',
                        })
                    }
                }
                else {
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    }

    function CreateStafferMember() {
        $.ajax({
            url: baseUrl + 'Home/GetStaffmember',
            type: "POST",
            async: true,
            cache: false,
            contentype: "application/json",
            datatype: "json",
            data: {
                id: vm.id,
                semester: vm.semester,
            },
            headers: {
                //'RequestVerificationToken': token
            },
            success: function (response, textStatus, jqXHR) {
                if (jqXHR.status === 200) {
                    if (response.Status == "OK") {
                        Swal.fire({
                            icon: 'success',
                            title: '成功',
                            text: '資料建立成功',
                        })

                        Swal.fire({
                            icon: 'success',
                            title: '成功',
                            text: '資料建立成功',
                            confirmButtonText: 'OK',
                        }).then((result) => {
                            /* Read more about isConfirmed, isDenied below */
                            //if (result.isConfirmed) {
                            //    getData(1);
                            //}
                        })
                    }
                    else {
                        Swal.fire({
                            icon: 'error',
                            title: '連線逾時',
                            text: '請稍後再試',
                        })
                    }
                }
                else {
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    }
    function CreateStudentData() {
        $.ajax({
            url: baseUrl + 'Home/GetStudentData',
            type: "POST",
            async: true,
            cache: false,
            contentype: "application/json",
            datatype: "json",
            data: {
                code: vm.pnowcondition,
            },
            headers: {
                //'RequestVerificationToken': token
            },
            success: function (response, textStatus, jqXHR) {
                if (jqXHR.status === 200) {
                    if (response.Status == "OK") {
                        Swal.fire({
                            icon: 'success',
                            title: '成功',
                            text: '資料建立成功',
                        })

                        Swal.fire({
                            icon: 'success',
                            title: '成功',
                            text: '資料建立成功',
                            confirmButtonText: 'OK',
                        }).then((result) => {
                            /* Read more about isConfirmed, isDenied below */
                            //if (result.isConfirmed) {
                            //    getData(1);
                            //}
                        })
                    }
                    else {
                        Swal.fire({
                            icon: 'error',
                            title: '連線逾時',
                            text: '請稍後再試',
                        })
                    }
                }
                else {
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    }



    // 建立 Page list 的 page 選單
    function buildPageList() {
        if (vm.pageList.PageNumber < 5) {
            vm.pageList.PageStart = 1;
            if (vm.pageList.PageCount > 10) {
                vm.pageList.PageEnd = 10;
            } else {
                vm.pageList.PageEnd = vm.pageList.PageCount;
            }
        } else {
            vm.pageList.PageStart = vm.pageList.PageNumber - 4;
            if (vm.pageList.PageNumber + 4 > vm.pageList.PageCount) {
                vm.pageList.PageEnd = vm.pageList.PageCount;
            } else {
                vm.pageList.PageEnd = vm.pageList.PageNumber + 4;
            }
        }
    }
})();