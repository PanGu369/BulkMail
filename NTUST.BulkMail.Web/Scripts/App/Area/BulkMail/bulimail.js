
// 全局注册 vue-multiselect 组件
Vue.component('vue-multiselect', window.VueMultiselect.default);
(function () {
    "use strict";
    var baseUrl = $('#baseUrl').val();
    var formData = new FormData();
    // 定義 modal 可以多層次開啟；最上層關閉後，底層 modal 可以維持 scroll
    $(document).on('hidden.bs.modal', '.modal', function () {
        $('.modal:visible').length && $(document.body).addClass('modal-open');
    });
    Dropzone.autoDiscover = false;
    $(document).ready(function () {
        getData(1);
        getUnincodeData(1);
        getStaffClassTitleCodeData(1);
        getMailGroup();
        // Summernote
        $('#summernote').summernote({
            lang: 'zh-TW',
        });
        //init();
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
            staffClassTitleCodePageList: {
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
            staffClassTitleCodeData: {},
            mailgroup: [],
            mailgroupList: {},
            lostUnitCodes: {},
            lostStaffClassTitleCodes: {},
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
            getStaffClassTitleCodeData(pageIndex) {
                getStaffClassTitleCodeData(pageIndex);
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
            OpenCreateStaffClassTitleCodeModal() {
                OpenCreateStaffClassTitleCodeModal();
            },
            OpenStaffClassTitleCodeModal() {
                OpenStaffClassTitleCodeModal()
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
            editStaffClassTitleCode(id) {
                editStaffClassTitleCode(id);
            },
            createUnicodeData() {
                createUnicodeData();
            },
            createStaffClassTitleCodeData() {
                createStaffClassTitleCodeData();
            },
            deleteUnicode(tunitcode, unitcode) {
                deleteUnicode(tunitcode, unitcode);
            },
            deleteStaffClassTitleCode(id) {
                deleteStaffClassTitleCode(id);
            },
            updateUnicodeData() {
                updateUnicodeData();
            },
            updateStaffClassTitleCodeData() {
                updateStaffClassTitleCodeData();
            },
            addEMailToGroup(item) {
                addEMailToGroup(item);
            },
            SendMail() {
                SendMail();
            },
            SendBulletinBoard() {
                SendBulletinBoard();
            },
            DownloadBulkMail() {
                DownloadBulkMail();
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
            autoProcessQueue: true, // 启用自动上传
            type: "POST",
            thumbnailWidth: 80,
            thumbnailHeight: 80,
            parallelUploads: 20,
            previewTemplate: previewTemplate,
            paramName: "file", 
            acceptedFiles: ".jpg,.gif,.png,.jpeg,.pdf,.doc,.docx",
            //autoQueue: false, // Make sure the files aren't queued until manually added
            previewsContainer: "#previews", // Define the container to display the previews
            clickable: ".fileinput-button" // Define the element that should be used as click trigger to select files.
        })

        myDropzone.on("addedfile", function (file) {
            // Hookup the start button
            file.previewElement.querySelector(".start").onclick = function () { myDropzone.enqueueFile(file) }
            formData.append(file.name, file);
        })

        myDropzone.on("sending", function (file) {
            // Show the total progress bar when upload starts
            document.querySelector("#total-progress").style.opacity = "1"
            // And disable the start button
            file.previewElement.querySelector(".start").setAttribute("disabled", "disabled")
        })

        myDropzone.on("removedfile", function (file) {
            formData.delete(file.name);
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
                        vm.lostUnitCodes = response.lostUnintCode;
                        vm.lostStaffClassTitleCodes = response.lostStaffClassTitleCode;
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
                                //"buttons": ["excel", "print", "colvis"],
                                "destroy": true,
                            });
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

    function getStaffClassTitleCodeData(pageIndex) {
        $.LoadingOverlay("show");
        $.ajax({
            url: baseUrl + 'Home/StaffClassTitleCodeList',
            type: "POST",
            async: true,
            cache: false,
            contentype: "application/json",
            datatype: "json",
            data: {
                pageIndex: pageIndex,
            },
            headers: {
                //'RequestVerificationToken': token
            },
            success: function (response, textStatus, jqXHR) {
                if (jqXHR.status === 200) {
                    if (response.resultMessage.Status == "OK") {
                        vm.staffClassTitleCodePageList = response.pageListViewModel;
                        Vue.nextTick(function () {
                            $("#staffClassTitleCode").DataTable({
                                "responsive": true, "lengthChange": false, "autoWidth": false,
                                "language": {
                                    url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/zh-HANT.json',
                                },
                                'iDisplayLength': 15,
                                //"buttons": ["excel", "print", "colvis"],
                                "destroy": true,
                            });
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
    function editStaffClassTitleCode(id) {
        $.LoadingOverlay("show");
        $.ajax({
            url: baseUrl + 'Home/GetStaffClassTitleCodeData',
            type: "POST",
            async: true,
            cache: false,
            contentype: "application/json",
            datatype: "json",
            data: {
                id: id,
            },
            headers: {
                //'RequestVerificationToken': token
            },
            success: function (response, textStatus, jqXHR) {
                $.LoadingOverlay("hide");
                if (jqXHR.status === 200) {
                    if (response.resultMessage.Status == "OK") {
                        vm.staffClassTitleCodeData = response.staffClassTitleCodeData;
                        OpenStaffClassTitleCodeEditModal();
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
    function createStaffClassTitleCodeData() {
        $.LoadingOverlay("show");
        $.ajax({
            url: baseUrl + 'Home/CreateStaffClassTitleCodeData',
            type: "POST",
            async: true,
            cache: false,
            contentype: "application/json",
            datatype: "json",
            data: {
                staffClassTitleCode: vm.staffClassTitleCodeData,
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
                                getStaffClassTitleCodeData(1);
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
    function deleteStaffClassTitleCode(id) {
        $.LoadingOverlay("show");
        $.ajax({
            url: baseUrl + 'Home/DeleteStaffClassTitleCodeData',
            type: "POST",
            async: true,
            cache: false,
            contentype: "application/json",
            datatype: "json",
            data: {
                id: id
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
                                getStaffClassTitleCodeData(1);
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
    function updateStaffClassTitleCodeData() {
        $.LoadingOverlay("show");
        $.ajax({
            url: baseUrl + 'Home/UpdateStaffClassTitleCodeData',
            type: "POST",
            async: true,
            cache: false,
            contentype: "application/json",
            datatype: "json",
            data: {
                staffClassTitleCode: vm.staffClassTitleCodeData,
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
                                CloseStaffClassTitleCodeEditModal();
                                getStaffClassTitleCodeData(1);
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
        formData.append("mail", JSON.stringify(vm.mail));
        $.LoadingOverlay("show");
        $.ajax({
            url: baseUrl + 'Home/SendEmail',
            type: "POST",
            async: true,
            cache: false,
            //dataType: "json",
            //contentType: "application/json",
            data: formData,
            processData: false,
            contentType: false,
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
                        }).then((result) => {
                            /* Read more about isConfirmed, isDenied below */
                            if (result.isConfirmed) {
                                formData = new FormData();
                                location.reload();
                            }
                        })
                    }
                    else if (response.Status == "EMPTY") {
                        $.LoadingOverlay("hide");
                        Swal.fire({
                            icon: 'error',
                            title: '連線逾時',
                            text: response.Message,
                            confirmButtonText: 'OK',
                        }).then((result) => {
                            /* Read more about isConfirmed, isDenied below */
                            if (result.isConfirmed) {
                                formData = new FormData();
                                location.reload();
                            }
                        })
                    }
                    else {
                        $.LoadingOverlay("hide");
                        Swal.fire({
                            icon: 'error',
                            title: '連線逾時',
                            text: "請稍後再試",
                            confirmButtonText: 'OK',
                        }).then((result) => {
                            /* Read more about isConfirmed, isDenied below */
                            if (result.isConfirmed) {
                                formData = new FormData();
                                location.reload();
                            }
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
    function DownloadBulkMail() {
        $.LoadingOverlay("show");
        var xhr = new XMLHttpRequest();
        xhr.open("GET", "bulkmail/Home/DownloadZip", true);
        xhr.responseType = "blob";  // 將響應類型設置為二進制數據

        xhr.onload = function () {
            if (xhr.status === 200) {
                var blob = new Blob([xhr.response], { type: "application/zip" });
                var url = window.URL.createObjectURL(blob);
                var a = document.createElement("a");
                a.href = url;
                a.download = "bulkmail.zip";
                a.style.display = "none";
                document.body.appendChild(a);
                a.click();
                window.URL.revokeObjectURL(url);
            }
        };

        xhr.send();
        $.LoadingOverlay("hide");
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
    function OpenCreateStaffClassTitleCodeModal() {
        vm.staffClassTitleCodeData = {};
        $('#createStaffClassTitleCodeModal').modal({
            show: true,
            backdrop: 'static'
        });
    }
    function OpenStaffClassTitleCodeModal() {
        $('#staffClassTitleCodeModal').modal({
            show: true,
            backdrop: 'static'
        });
    }
    function OpenStaffClassTitleCodeEditModal() {
        $('#editStaffClassTitleCodeModal').modal({
            show: true,
            backdrop: 'static'
        });
    }
    function CloseUnicodeEditModal() {
        $('#editModal').modal('hide')
    }
    function CloseStaffClassTitleCodeEditModal() {
        $('#editStaffClassTitleCodeModal').modal('hide')
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