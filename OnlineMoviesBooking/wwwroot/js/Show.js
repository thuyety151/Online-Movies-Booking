$(document).ready(function () {
    
    $('#theater').change(function () {
        console.log('change');
        var value = $('#theater').val();
        console.log(value);
        $.ajax({
            method: 'GET',
            url: '/Admin/Shows/GetScreen/' + value,
            success: function (data) {
                console.log(data.items);
                $('#screen').empty();
                $.each(data.items, function (index, value) {

                    console.log('<option value="' + value.id + '">' + value.text + '</option>');
                    $('#screen').append('<option value="' + value.id + '">' + value.text + '</option>');
                });
            }
           
        });
       
    });
});
