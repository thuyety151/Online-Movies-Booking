(function ($) {
    $(document).ready(function () {

        // tim div #dates chua cac button ngay
        const btn = document.querySelector('#dates');
        // tim tat ca the a ( tat ca button ngay)
        const btnDates = btn.querySelectorAll('.button');

       

        //const btnT = document.querySelector('#theater');
        //// tim tat ca the a ( tat ca button ngay)
        //const btntheaters = btnT.querySelectorAll('.button');

        $('#date-hidden').val(btnDates[0].text);

        $.ajax({
            type: 'GET',
            url: '/customer/movie/getshowbydate/',
            data: {
                "idMovie": $('#Id').text(),
                "date": $('#date-hidden').val(),
            },
            dataType: 'json',
            success: function (data) {
                $('#shows-date').empty();
                console.log(data);
                console.log($(data).length);
                if ($(data).lenght > 0) {
                    var div = '<li><p>Không có lịch chiếu</p></li>';
                    $('#shows-date').append(div);
                }
                else {

                    $.each(data, function (index, value) {
                        var div = '<li>' +
                            '<div class="movie-name">' +
                            ' <div class="icons">' +
                            '</div>' +
                            '<a class="name">' + value.name + '</a>' +
                            ' <div class="location-icon">' +
                            '<i class="fas fa-map-marker-alt"></i></div></div><div class="movie-schedule">';

                        $.each(value.times, function (index, time) {
                            div = div + '<div class="details-banner-content"><a class="button" data-id="' + time.id + '" href="/customer/movie/seatplan/' + time.id + '" style="color: #ffffff;padding: 0.2 1rem;">' + time.times + '</a></div>';
                        })
                        div = div + '</div ></li>';
                        $('#shows-date').append(div);
                    })
                }

            }
        });


        $.each(btnDates, function (index, value) {
            // tao event click
            value.addEventListener('click', function () {
                // xoa css 
                $.each(btnDates, function (index, value) {
                    if (value.classList.contains('clickcolor')){
                        value.classList.remove('clickcolor');
                    }
                });
                value.classList.toggle('clickcolor');
                // note date lai tren view
                $('#date-hidden').val(value.text);
                $.ajax({
                    type: 'GET',
                    url: '/customer/movie/getshowbydate/',
                    data: {
                        "idMovie": $('#Id').text(),
                        "date": $('#date-hidden').val()
                    },
                    dataType: 'json',
                    success: function (data) {
                        $('#shows-date').empty();
                        if ($(data).lenght > 0) {
                            var div = '<li><p>Không có lịch chiếu</p></li>';
                            $('#shows-date').append(div);
                        }
                        else {

                            $.each(data, function (index, value) {
                                var div = '<li>' +
                                    '<div class="movie-name">' +
                                    ' <div class="icons">' +
                                    '</div>' +
                                    '<a class="name">' + value.name + '</a>' +
                                    ' <div class="location-icon">' +
                                    '<i class="fas fa-map-marker-alt"></i></div></div><div class="movie-schedule">';

                                $.each(value.times, function (index, time) {
                                    div = div + '<div class="details-banner-content"><a class="button" data-id="'+time.id+'" href="/customer/movie/seatplan/'+time.id+'" style="color: #ffffff;padding: 0.2 1rem;">' + time.times + '</a></div>';
                                })
                                div = div + '</div ></li>';
                                $('#shows-date').append(div);


                            })
                        }

                    }
                });
            });
        });

    });
})(jQuery);

