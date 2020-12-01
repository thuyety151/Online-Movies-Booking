(function ($) {
    $(document).ready(function () {
        const date = document.getElementById
        
        
        //$('#date-a').click(function () {
        //    console.log($(this).text());
        //});
    });

    $('#theater-a').click(function () {
        console.log($(this).text());
        //$.ajax({
        //    type: 'GET',
        //    url: '/Customer/Movie/GetShowByDate/' + $(this).text().
        //})
    });
   


    

})(jQuery);

$(function ($) {
    
    const dates = document.querySelectorAll("#dates");

    dates.forEach(function (date) {
        //var movie = document.getElementById('nameMovie');
        const btn = date.querySelector("#date-a");
        btn.addEventListener("click", function () {
            console.log($(this).text());
            $.ajax({
                type: 'GET',
                url: '/customer/movie/getshowbydate/',
                data: {
                    "idMovie": $('#Id').text(),
                    "date": $(this).text()
                },
                dataType: 'json',
                success: function (data) {
                    $.each(data, function (index, value) {
                        console.log(value);
                        $('#shows-date').html(function () {
                            return `<div class="icons">
                                <i class="far fa-heart"></i>
                                <i class="fas fa-heart"></i>
                            </div>
                            <a href="http://pixner.net/boleto/demo/movie-ticket-plan.html#0" class="name">${value.idMovie}</a >
                            <div class="location-icon">
                                <i class="fas fa-map-marker-alt"></i>
                            </div>` 
                        });
                    });
                }

                
            });
        });
    });
});