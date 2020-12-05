(function ($) {
    $(document).ready(function () {

        // get row
        var rows = document.querySelectorAll('.seat-line');
        $.each(rows, function (index, row) {
            var name = row.querySelector('span');
            var seats = row.querySelectorAll('.single-seat');
            $.each(seats, function (index, seat) {
                seat.name = name.textContent + (index+1).toString();
                
                // tao ten cho ghe ngoi
                
                //console.log(seat);
                //console.log(seat.name);
            });

        });
        var cost = 0;
        $('#total-price').text(cost.toLocaleString('it-IT', { style: 'currency', currency: 'VND' }));
        //get price
        $.ajax({
            type: 'GET',
            url: '/Customer/Movie/getprice',
            success: function (data) {
                $('.type-1').val(data[0].cost);
                $('.type-2').val(data[1].cost);
                $('.type-3').val(data[2].cost);
            }
        });

        // goi cac ghe

        var seats = document.querySelectorAll('.single-seat');
        $.each(seats, function (index, seat) {
            seat.addEventListener('click', function () {
                //console.log(seat.name);
                //$('#choosed-seat').text(seat.name);
            });
        });
        

    });
})(jQuery);
