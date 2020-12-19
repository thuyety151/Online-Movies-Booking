(function ($) {
    $(document).ready(function () {
        console.log($('#idshow').text())
        console.log($('#idshow').val())
        console.log("ready");
        $.ajax({
            method: 'GET',
            url: "/Movie/UseDiscount",
            data: {
                idshow: $('#idshow').val(),
                iddiscount: '09d166895d'
            },
            success: function (data) {
                console.log(data)
                $('#discount-name').text(data.nameDiscount)
                console.log($('#discount-name').text())
                if (data.maxCost != null) {
                    $('#discount-price').text(data.maxCost.toLocaleString('it-IT', { style: 'currency', currency: 'VND' }))
                }
                else {
                    $('#discount-price').text(data.percentDiscount +' %')
                }
                
                
            }

        });
    });
    setTimeout(function () {
        console.log('timeout');
        $('#timeout').submit();
    }, 60000);

})(jQuery);
