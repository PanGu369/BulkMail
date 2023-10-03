﻿
// 全局注册 vue-multiselect 组件
Vue.component('vue-multiselect', window.VueMultiselect.default);
(function () {
    "use strict";
    var baseUrl = $('#baseUrl').val();
    // 定義 modal 可以多層次開啟；最上層關閉後，底層 modal 可以維持 scroll
    $(document).on('hidden.bs.modal', '.modal', function () {
        $('.modal:visible').length && $(document.body).addClass('modal-open');
    });
    Dropzone.autoDiscover = false;
    $(document).ready(function () {
        getData(1);
        getUnincodeData(1);
        getMailGroup();
        // Summernote
        $('#summernote').summernote({
            lang: 'zh-TW',
        });
        //Initialize Select2 Elements
        $('.select2').select2();

        //Initialize Select2 Elements
        $('.select2bs4').select2({
            theme: 'bootstrap4'
        });

        init();
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
            mail: {
                sender: "",
                receiver: [],
                txtreceiver: "",
                subject: "",
                email_content: "",
                dt: "",
            },
            unicodeData: {},
            mailgroup: [],
            mailgroupList: {},
            value: [],
            value1:"",
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
            getMailGroup() {
                getMailGroup();
            },
            getMailList(groupName) {
                getMailList(groupName);
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
            OpenCreateUnicodeModal() {
                OpenCreateUnicodeModal();
            },
            OpenUnicodeModal() {
                OpenUnicodeModal();
            },
            OpenMailAddressee() {
                OpenMailAddressee();
            },
            OpenEditMailAddressee() {
                OpenEditMailAddressee();
            },
            editUnicode(tunitcode, unitcode) {
                editUnicode(tunitcode, unitcode);
            },
            createUnicodeData() {
                createUnicodeData();
            },
            deleteUnicode(tunitcode, unitcode) {
                deleteUnicode(tunitcode, unitcode);
            },
            updateUnicodeData() {
                updateUnicodeData();
            },
            addEMailToGroup(item) {
                addEMailToGroup(item);
            },
            SendMail() {
                SendMail();
            },
            SendBulletinBoard() {
                SendBulletinBoard();
            }
        },
        filters: {
        }
    })

    function init() {
        // Get the template HTML and remove it from the doumenthe template HTML and remove it from the doument
        var previewNode = document.querySelector("#template")
        previewNode.id = ""
        var previewTemplate = previewNode.parentNode.innerHTML
        previewNode.parentNode.removeChild(previewNode)

        var myDropzone = new Dropzone(document.body, { // Make the whole body a dropzone
            url: baseUrl + 'Home/FileUpload', // Set the url
            type: "POST",
            thumbnailWidth: 80,
            thumbnailHeight: 80,
            parallelUploads: 20,
            previewTemplate: previewTemplate,
            acceptedFiles: ".jpg,.gif,.png,.jpeg,.pdf,.doc,.docx",
            autoQueue: false, // Make sure the files aren't queued until manually added
            previewsContainer: "#previews", // Define the container to display the previews
            clickable: ".fileinput-button" // Define the element that should be used as click trigger to select files.
        })

        myDropzone.on("addedfile", function (file) {
            // Hookup the start button
            file.previewElement.querySelector(".start").onclick = function () { myDropzone.enqueueFile(file) }
        })

        // Update the total progress bar
        myDropzone.on("totaluploadprogress", function (progress) {
            document.querySelector("#total-progress .progress-bar").style.width = progress + "%"
        })

        myDropzone.on("sending", function (file) {
            // Show the total progress bar when upload starts
            document.querySelector("#total-progress").style.opacity = "1"
            // And disable the start button
            file.previewElement.querySelector(".start").setAttribute("disabled", "disabled")
        })

        // Hide the total progress bar when nothing's uploading anymore
        myDropzone.on("queuecomplete", function (file, progress) {
            document.querySelector("#total-progress").style.opacity = "0"
        })

        myDropzone.on("success", function (file) {
            //var name = file.name;
            //var imagePathObject = JSON.parse(file.xhr.response);
            //var imagePath = imagePathObject[name];
            //var imagesQuery = {
            //    ItemSeq: "",
            //    ImageUrl: "",
            //};
            //imagesQuery.ImageUrl = imagePath;
            //vm.itemList.Images.push(imagesQuery);
        })

        // Setup the buttons for all transfers
        // The "add files" button doesn't need to be setup because the config
        // `clickable` has already been specified.
        document.querySelector("#actions .start").onclick = function () {
            myDropzone.enqueueFiles(myDropzone.getFilesWithStatus(Dropzone.ADDED))
        }
        //document.querySelector("#actions .cancel").onclick = function () {
        //    myDropzone.removeAllFiles(true)
        //}
        // DropzoneJS Demo Code End
    }
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
                        //Vue.nextTick(function () {
                        //    $("#bulkmail").DataTable({
                        //        "responsive": true, "lengthChange": false, "autoWidth": false,
                        //        "language": {
                        //            url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/zh-HANT.json',
                        //        },
                        //        'iDisplayLength': 15,
                        //        "buttons": ["excel", "print", "colvis"],
                        //        "destroy": true,
                        //    }).buttons().container().appendTo('#bulkmail_wrapper .col-md-6:eq(0)');
                        //})
                        $.LoadingOverlay("hide");
                        //buildPageList();
                    }
                    else if (response.resultMessage.Status == "EMPTY") {
                        Swal.fire({
                            icon: 'success',
                            title: '沒有資料',
                            text: '',
                        })
                        $.LoadingOverlay("hide");
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

    function getMailGroup() {
        $.LoadingOverlay("show");
        $.ajax({
            url: baseUrl + 'Home/GetMailGroup',
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
                    if (response.Status == "OK") {
                        vm.mailgroup = response.Body;
                        Vue.nextTick(function () {
                            $("#mail").DataTable({
                                "responsive": true, "lengthChange": false, "autoWidth": false,
                                'iDisplayLength': 15,
                                "language": {
                                    url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/zh-HANT.json',
                                },
                                "destroy": true,
                            }).buttons().container().appendTo('#mail_wrapper .col-md-6:eq(0)');
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
    function getMailList(groupName) {
        $.LoadingOverlay("show");
        $('#mailList').DataTable().destroy();
        $.ajax({
            url: baseUrl + 'Home/GetMailGroupList',
            type: "POST",
            async: true,
            cache: false,
            contentype: "application/json",
            datatype: "json",
            data: {
                groupName: groupName
            },
            headers: {
                //'RequestVerificationToken': token
            },
            success: function (response, textStatus, jqXHR) {
                if (jqXHR.status === 200) {
                    if (response.Status == "OK") {
                        vm.mailgroupList = response.Body;
                        Vue.nextTick(function () {
                            $("#mailList").DataTable({
                                "responsive": true, "lengthChange": false, "autoWidth": false,
                                'iDisplayLength': 15,
                                "language": {
                                    url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/zh-HANT.json',
                                },
                                "destroy": true,
                            }).buttons().container().appendTo('#mailList_wrapper .col-md-6:eq(0)');
                        })
                        $.LoadingOverlay("hide");
                        CloseEditMailAddressee();
                        OpenEditMailAddressee();
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
    function createUnicodeData() {
        $.LoadingOverlay("show");
        $.ajax({
            url: baseUrl + 'Home/CreateUnicodeData',
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
                            text: '資料更新成功',
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
    function deleteUnicode(tunitcode, unitcode) {
        $.LoadingOverlay("show");
        $.ajax({
            url: baseUrl + 'Home/DeleteUnicodeData',
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
                    if (response.Status == "OK") {
                        Swal.fire({
                            icon: 'success',
                            title: '成功',
                            text: '資料刪除成功',
                            confirmButtonText: 'OK',
                        }).then((result) => {
                            /* Read more about isConfirmed, isDenied below */
                            if (result.isConfirmed) {
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
                            text: '資料更新成功',
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

    function addEMailToGroup(item) {
        vm.mail.receiver.push(item);
        Swal.fire({
            icon: 'success',
            title: '成功',
            text: item.name + " " + "加入收件人",
        })
    }

    function SendMail() {
        var html = $('#summernote').summernote('code');
        vm.mail.email_content = html;
        $.LoadingOverlay("show");
        $.ajax({
            url: baseUrl + 'Home/SendEmail',
            type: "POST",
            async: true,
            cache: false,
            contentype: "application/json",
            datatype: "json",
            data: {
                mail: vm.mail
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
                            text: '寄信成功',
                            confirmButtonText: 'OK',
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
    function SendBulletinBoard() {
        $.LoadingOverlay("show");
        $.ajax({
            url: baseUrl + 'Home/SendBulletinBoard',
            type: "POST",
            async: true,
            cache: false,
            contentype: "application/json",
            datatype: "json",
            data: {
                mail: vm.mail
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
                            text: '寄信成功',
                            confirmButtonText: 'OK',
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
    function OpenCreateUnicodeModal() {
        vm.unicodeData = {};
        $('#createModal').modal({
            show: true,
            backdrop: 'static'
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

    function OpenMailAddressee() {
        $('#mailModal').modal({
            show: true,
            backdrop: 'static'
        });
    }
    function OpenEditMailAddressee() {
        $('#mailEditModal').modal({
            show: true,
            backdrop: 'static'
        });
    }
    function CloseEditMailAddressee() {
        $('#mailEditModal').modal('hide')
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