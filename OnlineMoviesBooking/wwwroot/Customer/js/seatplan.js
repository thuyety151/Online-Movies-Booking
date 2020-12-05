$(function () {
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
