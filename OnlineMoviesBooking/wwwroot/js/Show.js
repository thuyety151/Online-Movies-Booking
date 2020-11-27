$(document).ready(function () {
    var val = $('#theater').val();
    console.log(val);

    $('#screen').select2({
        ajax: {
            method: 'GET',
            url: '/Shows/GetScreen/' + val,
            processResults: function (data) {
                return {
                    results: data.items
                }
            },
        }
    });
    $('#theater').select2({
        width: 'resolve'     
    });

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