var MasterAdmin = MasterAdmin || {};
//Erro message
function MessageErro(message) {
    $("#messageInfo").html(
        '<div class="alert alert-danger alert-dismissable">' +
            '<button type="button" data-dismiss="alert" aria-hidden="true" class="close"></button>' +
            '<span>' + message + '</span>' +
         '</div>');
}
//---Success mesage
function MessageSuccess(messageSuccsess) {
    $("#messageInfo").html(
        '<div class="alert alert-success alert-dismissable">' +
            '<button type="button" data-dismiss="alert" aria-hidden="true" class="close"></button>' +
            '<span>' + messageSuccsess + '</span>' +
         '</div>');
}
//--validate file
function MessageErroFile(message) {
    $("#messageInfoUpload").html(
        '<div class="alert alert-danger alert-dismissable">' +
            '<button type="button" data-dismiss="alert" aria-hidden="true" class="close"></button>' +
            '<span>' + message + '</span>' +
         '</div>');
}
function MessageSucessFile(message) {
    $("#messageInfoUpload").html(
        '<div class="alert alert-success alert-dismissable">' +
            '<button type="button" data-dismiss="alert" aria-hidden="true" class="close"></button>' +
            '<span>' + message + '</span>' +
         '</div>');
}
//----Reload grid----
function reloadGrid(url) {
    $.ajax({
        type: "GET",
        url: url,
        data: "{}",
        cache: false,
        dataType: "html",
        success: function (data) {
            $("#gridShow").html(data);
        }
    });
}
//----------CK Editer
function createEditor(languageCode, id) {
    var editor = CKEDITOR.replace(id, {
        language: languageCode
    });
}
//-----------------------Upload file-----------
function FileUploadInAjax() {
    window.addEventListener("submit", function (e) {
        var form = e.target;
        if (form.getAttribute("enctype") === "multipart/form-data") {
            if (form.dataset.ajax) {
                e.preventDefault();
                e.stopImmediatePropagation();
                var xhr = new XMLHttpRequest();
                xhr.open(form.method, form.action);
                xhr.onreadystatechange = function () {
                    if (xhr.readyState == 4 && xhr.status == 200) {
                        if (form.dataset.ajaxUpdate) {
                            var updateTarget = document.querySelector(form.dataset.ajaxUpdate);
                            if (updateTarget) {
                                updateTarget.innerHTML = xhr.responseText;
                            }
                        }
                    }
                };
                xhr.send(new FormData(form));
            }
        }
    }, true);

}
//--tim kiem chi tiet camera
////-----refresh img
//function fnSetTimer(image, src, counter, limit) {
//    image.onload = null;

//    if (counter < limit) {
//        counter++;
//        setTimeout(function () { fnSetTimer(image, src, counter, limit); }, 500);
//        image.src = src + '?\\' + new Date().getMilliseconds();
//        //alert(counter); // show frame number
//    }
//}

MasterAdmin.ShowUpload = {
    // Show modal upload file
    detail: function (type, productidentifier) {

        if (productidentifier!= 0) {
            var url = '';
            switch (type) {
            case 'uploadImageProduct':
                url = window.location.protocol + '//' + window.location.host + '/FileBase/UploadImageProduct/' + productidentifier;
                break;
            case 'UploadFile':
                url = window.location.protocol + '//' + window.location.host + '/FileBase/UploadFile/' + productidentifier;
                break;
            }
            if (navigator.appName === "Microsoft Internet Explorer") {
                url = url + '&uniq_param=' + (new Date()).getTime();
            }
            $.when($.get(url, function (data) {
                $('#myModalUploadFile').remove();
                $('body').append(data);
            })).then(function () {
                $('#myModalUploadFile').modal('show');
            });
        }
    }
}