(function ($) {
    $(document).ready(function () {
        console.log($('#idshow').text())
        console.log($('#idshow').val())
        console.log("ready");
        
    });
    setTimeout(function () {
        console.log('timeout');
        $('#timeout').submit();
    }, 60000);
    $('#use-discount').on('click', function () {
        console.log('click');
        if ($('#code-discount').val()=="") {
            $('#validation').text("*Không hợp lệ")
        }
        else {
            $.ajax({
                method: 'GET',
                url: "/Movie/UseDiscount",
                data: {
                    idshow: $('#idshow').val(),
                    iddiscount: $('#code-discount').val()
                },
                success: function (data) {
                    console.log(data);
                    if (data.iddiscount == null) {
                      
                    }
                    console.log(data)
                    $('#discount-name').text(data.nameDiscount)
                    console.log($('#discount-name').text())
                    if (data.maxCost == "") {
                        $('#discount-price').text(data.percentDiscount + ' %')
                        console.log('percent')
                    }
                    else {
                        $('#discount-price').text((data.no * data.maxCost).toLocaleString('it-IT', { style: 'currency', currency: 'VND' }))

                    }
                    $('#total-price').text(data.price.toLocaleString('it-IT', { style: 'currency', currency: 'VND' }));
                }

            });
        }
        
    });
})(jQuery);
