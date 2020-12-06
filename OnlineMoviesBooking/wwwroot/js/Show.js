$(document).ready(function () {
    var val = $('#theater').val();
    
    $('#screen').select2({
        ajax: {
            method: 'GET',
            url: '/Admin/Shows/GetScreen/' + val,
            processResults: function (data) {
                return {
                    results: data.items
                }
            },
        }
    });
    

    $('#theater').change(function () {
        var value = $('#theater').val();
        $.ajax({
            method: 'get',
            url: '/Admin/shows/getscreen/' + value,
            success: function (data) {
                //console.log(data);
                //$('#screen').append('<option value="' + data.id + '">' + data.name + '</option>');
                $('#screen').select2({
                    ajax: {
                        method: 'GET',
                        url: '/Admin/Shows/GetScreen/' + value,
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
