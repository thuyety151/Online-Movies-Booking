$(document).ready(function () {
    var value = $('#theater').val();
    //$('.js-example-basic-single').select2();
    //$('#screen').select2({
    //    ajax: {
    //        method: 'GET',
    //        url: '/Shows/GetScreen/' + value,
    //        processResults: function (data) {
    //            return {
    //                results: data.items
    //            }
    //        },
    //    }
    //});
    $('#theater').change(function () {
        var value = $('#theater').val();
        $.ajax({
            method: 'get',
            url: '/shows/getscreen/' + value,
            success: function (data) {
                //console.log(data);
                //$('#screen').append('<option value="' + data.id + '">' + data.name + '</option>');
                $('#screen').select2({
                    ajax: {
                        method: 'GET',
                        url: '/Shows/GetScreen/' + value,
                        processResults: function (data) {
                            return {
                                results: data.items
                            }
                        },
                    }
                })
            }
        });
       
    });
});