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

        var checkout = document.getElementById("submit");
        checkout.addEventListener("click", function () {
            console.log($('#lstSeat').val());
            if ($('#lstSeat').val() == "") {
                alert("Vui lòng chọn vị trí ngồi");
            }
            else {
                $.ajax({
                    type: 'GET',
                    url: '/Movie/getinfo/',

                    data: {
                        "idshow": $('#idshow').val(),
                        "lstSeat": $('#lstSeat').val(),
                    },
                    dataType: "json",
                    success: function (data) {
                        if (data == "error") {
                            alert("Đã xảy ra lỗi! Vui lòng load lại trang");
                        }
                        else if (data == "seat") {
                            alert("Vui lòng chọn chỗ ngồi");
                        }
                        else if (data == "Ghế đã được chọn") {
                            alert("Ghế đã được chọn, vui lòng chọn lại!");
                        }
                        else {
                            console.log(data)
                            window.location.href = "/Movie/CheckOut?idshow=" + $('#idshow').val() + "&lstSeat=" + data;
                            console.log("redirect")
                        }
                    }
                })
            }

        });
})(jQuery);
