// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

showInPopup = (url, title) => {
    $.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            $('#frm-modal .modal-body').html(res);
            $('#frm-modal .modal-title').html(title);
            $('#frm-modal').modal('show');
        }
    })
}

