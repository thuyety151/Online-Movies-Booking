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
                    "theaterName": $('#Id').text(),
                    "date": $(this).text()
                },
                success: function (data) {
                    console.log(data);
                }
            });
        });
    });
});