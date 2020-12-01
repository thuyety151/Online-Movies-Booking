(function ($) {
    $(document).ready(function () {
        const date = document.getElementById
        
        
        //$('#date-a').click(function () {
        //    console.log($(this).text());
        //});
    });

    

})(jQuery);

$(function ($) {

    const dates = document.querySelectorAll("#dates");

    dates.forEach(function (date) {
        //var movie = document.getElementById('nameMovie');
        const btn = date.querySelector("#date-a");
        btn.addEventListener("click", function () {
            var date = $(this).text()
            $.ajax({
                type: 'GET',
                url: '/customer/movie/getshowbydate/',
                data: {
                    "idMovie": $('#Id').text(),
                    "date": date,
                    "theaterName":null
                },
                dataType: 'json',
                success: function (data) {
                    $('#shows-date').empty();
                    $.each(data, function (index, value) {
                        var div = '<li>' +
                            '<div class="movie-name">' +
                            ' <div class="icons">' +
                            '</div>'+
                            '<a class="name">' + value.name + '</a>' +
                            ' <div class="location-icon">' +
                            '<i class="fas fa-map-marker-alt"></i></div></div><div class="movie-schedule">';

                        $.each(value.times, function (index, time) {
                            div = div + '<div class="" style="color: #ffffff;padding: 5px;">' + time.times + '</div>';
                        })
                        div = div + '</div ></li>';
                        $('#shows-date').append(div);

                    })

                }
            });
        });
    });

    const theaters = document.querySelectorAll("#theater-option");
    theaters.forEach(function (theater) {
        theater.addEventListener("click", function () {
            var idTheater = $(this).data("value");
            console.log(theater);
            console.log(idTheater);
        })
    })
})