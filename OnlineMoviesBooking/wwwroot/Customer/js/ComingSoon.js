$(document).ready(function () {
    $('#movie').load(function () {
        $.ajax
        ({
            method: 'GET',
            url: '/Movie/Coming' ,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $('#movie').html(data);
            }
        });
    })
});