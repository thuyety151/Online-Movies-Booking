(function ($) {
    $(document).ready(function () {
        console.log($('#idshow').text())
        console.log($('#idshow').val())
        console.log("ready");
        
    });
    setTimeout(function () {
        console.log('timeout');
        $('#timeout').submit();
    }, 300000); // 5 minute

    $('#use-discount').on('click', function () {
        
        if ($('#code-discount').val()=="") {
            $('#validation').text("*Không hợp lệ")
        }
        else {
            $.ajax({
                method: 'GET',
                url: "/Movie/UseDiscount",
                data: {
                    code: $('#code-discount').val()
                },
                success: function (data) {

                    console.log($('#code-discount').val())
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
                    $('#price-hidden').val(data.price)
                    $('#total-price').text(data.price.toLocaleString('it-IT', { style: 'currency', currency: 'VND' }));
                    $('#point').text(data.point)
                }

            });
        }
        
    });

    $('#use-point').on('click', function () {
        if ($('#code-point').val() == "") {
            $('#validation-point').text("*Không hợp lệ")
        }
        else {
            $.ajax({
                method: 'GET',
                url: "/Movie/UsePoint",
                data: {
                    point: $('#code-point').val()
                },
                success: function (data) {
                    console.log(data)
                    if (data === "Không thỏa") {
                        $('#validation-point').text("*Không hợp lệ")
                    }
                    else {
                        
                        console.log(parseInt($('#price-hidden').text().replace(/\20AB(\d)(?=(\d{3})+\.)/g, '$1,')))
                        const price = parseInt($('#price-hidden').val()) - parseInt($('#code-point').val()) * 1000;
                        console.log(price)
                        if (price < 0) {
                            $('#validation-point').text("*Không hợp lệ")
                            $('#point-hidden').val(0)
                        }
                        else {
                            $('#validation-point').text("")
                            // thảy đổi giá tiền hiển thị
                           
                            
                            $('#total-price').text(price.toLocaleString('it-IT', { style: 'currency', currency: 'VND' }));
                            // thay đổi số điểm
                            var point = (parseInt($('#total-price').text().replace(/\20AB(\d)(?=(\d{3})+\.)/g, '$1,')) * 5 / 100);
                            $('#point').text(parseInt(point))
                            $('#point-hidden').val(parseInt(point))
                            console.log('point ' + $('#point-hidden').val())
                        }
                        console.log(parseInt(point))
                    }
                }

            });
        }
    });
})(jQuery);
