(function () {
    "use strict";

    var baseUrl = $('#baseUrl').val();
    // 定義 modal 可以多層次開啟；最上層關閉後，底層 modal 可以維持 scroll
    $(document).on('hidden.bs.modal', '.modal', function () {
        $('.modal:visible').length && $(document.body).addClass('modal-open');
    });

    $(document).ready(function () {
        getData(1);
        getUnincodeData(1);
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
            unicodePageList: {
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
            unicodeData: {},
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
            getUnincodeData(pageIndex) {
                getUnincodeData(pageIndex);
            },
            CreateStafferMember() {
                CreateStafferMember();
            },
            CreateStudentData() {
                CreateStudentData();
            },
            CreateAlumnusData() {
                CreateAlumnusData();
            },
            GenerateMailGroupFile() {
                GenerateMailGroupFile();
            },
            GenerateAliasesFile() {
                GenerateAliasesFile();
            },
            OpenUnicodeModal() {
                OpenUnicodeModal();
            },
            editUnicode(tunitcode, unitcode) {
                editUnicode(tunitcode, unitcode);
            },
            updateUnicodeData() {
                updateUnicodeData();
            },
        },
        filters: {
        }
    })
    function getData(pageIndex) {
        $.LoadingOverlay("show");
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
                                "language": {
                                    url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/zh-HANT.json',
                                },
                                'iDisplayLength': 15,
                                "buttons": ["excel", "print", "colvis"],
                                "destroy": true,
                            }).buttons().container().appendTo('#bulkmail_wrapper .col-md-6:eq(0)');
                        })
                        $.LoadingOverlay("hide");
                        //buildPageList();
                    }
                    else {
                        $.LoadingOverlay("hide");
                        Swal.fire({
                            icon: 'error',
                            title: '連線逾時',
                            text: '請稍後再試',
                        })
                    }
                }
                else {
                    $.LoadingOverlay("hide");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $.LoadingOverlay("hide");
            }
        });
    }

    function getUnincodeData(pageIndex) {
        $.LoadingOverlay("show");
        $.ajax({
            url: baseUrl + 'Home/UnincodeList',
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
                        vm.unicodePageList = response.pageListViewModel;
                        Vue.nextTick(function () {
                            $("#unicode").DataTable({
                                "responsive": true, "lengthChange": false, "autoWidth": false,
                                "language": {
                                    url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/zh-HANT.json',
                                },
                                'iDisplayLength': 15,
                                "buttons": ["excel", "print", "colvis"],
                                "destroy": true,
                            }).buttons().container().appendTo('#unicode_wrapper .col-md-6:eq(0)');
                        })
                        $.LoadingOverlay("hide");
                        //buildPageList();
                    }
                    else {
                        $.LoadingOverlay("hide");
                        Swal.fire({
                            icon: 'error',
                            title: '連線逾時',
                            text: '請稍後再試',
                        })
                    }
                }
                else {
                    $.LoadingOverlay("hide");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $.LoadingOverlay("hide");
            }
        });
    }

    function CreateStafferMember() {
        $.LoadingOverlay("show");
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
                $.LoadingOverlay("hide");
                if (jqXHR.status === 200) {             
                    if (response.Status == "OK") {
                        Swal.fire({
                            icon: 'success',
                            title: '成功',
                            text: '資料建立成功',
                            confirmButtonText: 'OK',
                        }).then((result) => {
                            /* Read more about isConfirmed, isDenied below */
                            if (result.isConfirmed) {
                                getData(1);
                            }
                        })
                    }
                    else {
                        $.LoadingOverlay("hide");
                        Swal.fire({
                            icon: 'error',
                            title: '連線逾時',
                            text: '請稍後再試',
                        })
                    }
                }
                else {
                    $.LoadingOverlay("hide");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $.LoadingOverlay("hide");
            }
        });
    }
    function CreateStudentData() {
        $.LoadingOverlay("show");
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
                $.LoadingOverlay("hide");
                if (jqXHR.status === 200) {
                    if (response.Status == "OK") {
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
                        $.LoadingOverlay("hide");
                        Swal.fire({
                            icon: 'error',
                            title: '連線逾時',
                            text: '請稍後再試',
                        })
                    }
                }
                else {
                    $.LoadingOverlay("hide");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $.LoadingOverlay("hide");
            }
        });
    }
    function CreateAlumnusData() {
        $.LoadingOverlay("show");
        $.ajax({
            url: baseUrl + 'Home/GetAlumnusData',
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
                $.LoadingOverlay("hide");
                if (jqXHR.status === 200) {
                    if (response.Status == "OK") {
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
                        $.LoadingOverlay("hide");
                        Swal.fire({
                            icon: 'error',
                            title: '連線逾時',
                            text: '請稍後再試',
                        })
                    }
                }
                else {
                    $.LoadingOverlay("hide");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $.LoadingOverlay("hide");
            }
        });
    }
    function GenerateMailGroupFile() {
        $.LoadingOverlay("show");
        $.ajax({
            url: baseUrl + 'Home/GenerateMailGroupFile',
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
                $.LoadingOverlay("hide");
                if (jqXHR.status === 200) {
                    if (response.Status == "OK") {
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
                        $.LoadingOverlay("hide");
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
                $.LoadingOverlay("hide");
            }
        });
    }
    function GenerateAliasesFile() {
        $.LoadingOverlay("show");
        $.ajax({
            url: baseUrl + 'Home/GenerateAliasesFile',
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
                $.LoadingOverlay("hide");
                if (jqXHR.status === 200) {
                    if (response.Status == "OK") {
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
                        $.LoadingOverlay("hide");
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
                $.LoadingOverlay("hide");
            }
        });
    }
    function editUnicode(tunitcode, unitcode) {
        $.LoadingOverlay("show");
        $.ajax({
            url: baseUrl + 'Home/GetUnicodeData',
            type: "POST",
            async: true,
            cache: false,
            contentype: "application/json",
            datatype: "json",
            data: {
                tunitcode: tunitcode,
                unitcode: unitcode
            },
            headers: {
                //'RequestVerificationToken': token
            },
            success: function (response, textStatus, jqXHR) {
                $.LoadingOverlay("hide");
                if (jqXHR.status === 200) {
                    if (response.resultMessage.Status == "OK") {
                        vm.unicodeData = response.unicodeData;
                        OpenUnicodeEditModal();
                    }
                    else {
                        $.LoadingOverlay("hide");
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
                $.LoadingOverlay("hide");
            }
        });
    }

    function updateUnicodeData() {
        $.LoadingOverlay("show");
        $.ajax({
            url: baseUrl + 'Home/UpdateUnicodeData',
            type: "POST",
            async: true,
            cache: false,
            contentype: "application/json",
            datatype: "json",
            data: {
                unitcode: vm.unicodeData,
            },
            headers: {
                //'RequestVerificationToken': token
            },
            success: function (response, textStatus, jqXHR) {
                $.LoadingOverlay("hide");
                if (jqXHR.status === 200) {
                    if (response.Status == "OK") {
                        Swal.fire({
                            icon: 'success',
                            title: '成功',
                            text: '資料建立成功',
                            confirmButtonText: 'OK',
                        }).then((result) => {
                            /* Read more about isConfirmed, isDenied below */
                            if (result.isConfirmed) {
                                CloseUnicodeEditModal();
                                getUnincodeData(1);
                            }
                        })
                    }
                    else {
                        $.LoadingOverlay("hide");
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
                $.LoadingOverlay("hide");
            }
        });
    }
    function OpenUnicodeModal() {
        $('#modal-xl').modal({
            show: true,
            backdrop: 'static'
        });
    }
    function OpenUnicodeEditModal() {
        $('#editModal').modal({
            show: true,
            backdrop: 'static'
        });
    }
    function CloseUnicodeEditModal() {
        $('#editModal').modal('hide')
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