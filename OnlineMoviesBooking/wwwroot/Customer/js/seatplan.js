(function ($) {
    $(document).ready(function () {
        
        // get row
        var rows = document.querySelectorAll('.seat-line');
        $.each(rows, function (index, row) {
            var name = row.querySelector('span');
            var seats = row.querySelectorAll('.single-seat');
            var span;
            if (name.textContent === 'G' || name.textContent === 'H') {
                $.each(seats, function (index, seat) {
                    seat.name = name.textContent + (index*2 + 1).toString();
                    seat.name = seat.name + '' + name.textContent + (index * 2 + 2).toString();
                    span = seat.querySelector('.sit-num');
                    if (span != null) {
                        span.textContent = seat.name;
                    }
                   
                });
            }
            else {
                $.each(seats, function (index, seat) {
                    // dat ten ghe cho day A,B,C,D,E,F

                    seat.name = name.textContent + (index + 1).toString();
                    span = seat.querySelector('.sit-num');
                    if (span != null) {
                        span.textContent = seat.name;
                    }
                });
            }
            

        });

        // thanh toasn
        var checkout = document.getElementById("submit");
        checkout.addEventListener("click", function () {
            console.log($('#lstSeat').val());
            if ($('#lstSeat').val() == "") {
                alert("Vui lòng chọn vị trí ngồi");
            }
            $.ajax({
                type: 'GET',
                url: '/Movie/getinfo/',
                
                data: {
                    "idshow": $('#idshow').val(),
                    "lstSeat": $('#lstSeat').val(),
                },
                dataType: "json",
                success: function (data) {
                    console.log(data);
                    if (data.success == false) {
                        alert("Hello! I am an alert box!");
                    }
                    else {
                        $("#Checkout").modal();
                        var vnd = data.totalprice.toLocaleString('it-IT', { style: 'currency', currency: 'VND' });
                        
                        $("#Checkout").find('#modal-movie').text(data.movieName);
                        $("#Checkout").find('#modal-languages').text(data.languages);
                        $("#Checkout").find('#modal-time').text(data.datestart + ' ' + data.timestart);
                        $("#Checkout").find('#modal-total').text(vnd);
                        $("#Checkout").find('#modal-count').text(data.seats.length);
                        $("#Checkout").find('#modal-theater').text(data.theatername);
                    }
                }
            });
        });

        
        var cost = 0;
        $('#total-price').text(cost.toLocaleString('it-IT', { style: 'currency', currency: 'VND' }));
        //get price
        $.ajax({
            type: 'GET',
            url: '/Movie/getprice',
            success: function (data) {
                $('.type-1').val(data[0].cost);
                $('.type-2').val(data[1].cost);
                $('.type-3').val(data[2].cost);
                console.log($('.type-3').val());
                
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
