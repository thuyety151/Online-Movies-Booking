﻿// Set new default font family and font color to mimic Bootstrap's default styling
Chart.defaults.global.defaultFontFamily = 'Nunito', '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
Chart.defaults.global.defaultFontColor = '#858796';

function number_format(number, decimals, dec_point, thousands_sep) {
    // *     example: number_format(1234.56, 2, ',', ' ');
    // *     return: '1 234,56'
    number = (number + '').replace(',', '').replace(' ', '');
    var n = !isFinite(+number) ? 0 : +number,
        prec = !isFinite(+decimals) ? 0 : Math.abs(decimals),
        sep = (typeof thousands_sep === 'undefined') ? ',' : thousands_sep,
        dec = (typeof dec_point === 'undefined') ? '.' : dec_point,
        s = '',
        toFixedFix = function (n, prec) {
            var k = Math.pow(10, prec);
            return '' + Math.round(n * k) / k;
        };
    // Fix for IE parseFloat(0.55).toFixed(0) = 0;
    s = (prec ? toFixedFix(n, prec) : '' + Math.round(n)).split('.');
    if (s[0].length > 3) {
        s[0] = s[0].replace(/\B(?=(?:\d{3})+(?!\d))/g, sep);
    }
    if ((s[1] || '').length < prec) {
        s[1] = s[1] || '';
        s[1] += new Array(prec - s[1].length + 1).join('0');
    }
    return s.join(dec);
}

// Area Chart Example
var ctx = document.getElementById("myAreaChart");
var myAreaChart = new Chart(ctx, {

    type: 'line',
    data: {
        labels: ["1","2","3","4","5","6","7","8","9","10","11","12"],
        datasets: [{
            label: "Earnings",
            lineTension: 0.3,
            backgroundColor: "rgba(78, 115, 223, 0.05)",
            borderColor: "rgba(78, 115, 223, 1)",
            pointRadius: 3,
            pointBackgroundColor: "rgba(78, 115, 223, 1)",
            pointBorderColor: "rgba(78, 115, 223, 1)",
            pointHoverRadius: 3,
            pointHoverBackgroundColor: "rgba(78, 115, 223, 1)",
            pointHoverBorderColor: "rgba(78, 115, 223, 1)",
            pointHitRadius: 10,
            pointBorderWidth: 2,
            data: [1000,20000,3000,3000,1000,2200,1200,100000,1000000,2000,11000,111100],
        }],
    },
    options: {
        maintainAspectRatio: false,
        layout: {
            padding: {
                left: 10,
                right: 25,
                top: 25,
                bottom: 0
            }
        },
        scales: {
            xAxes: [{
                time: {
                    unit: 'date'
                },
                gridLines: {
                    display: false,
                    drawBorder: false
                },
                ticks: {
                    maxTicksLimit: 7
                }
            }],
            yAxes: [{
                ticks: {
                    maxTicksLimit: 5,
                    padding: 10,
                    // Include a dollar sign in the ticks
                    callback: function (value, index, values) {
                        return '$' + number_format(value);
                    }
                },
                gridLines: {
                    color: "rgb(234, 236, 244)",
                    zeroLineColor: "rgb(234, 236, 244)",
                    drawBorder: false,
                    borderDash: [2],
                    zeroLineBorderDash: [2]
                }
            }],
        },
        legend: {
            display: false
        },
        tooltips: {
            backgroundColor: "rgb(255,255,255)",
            bodyFontColor: "#858796",
            titleMarginBottom: 10,
            titleFontColor: '#6e707e',
            titleFontSize: 14,
            borderColor: '#dddfeb',
            borderWidth: 1,
            xPadding: 15,
            yPadding: 15,
            displayColors: false,
            intersect: false,
            mode: 'index',
            caretPadding: 10,
            callbacks: {
                label: function (tooltipItem, chart) {
                    var datasetLabel = chart.datasets[tooltipItem.datasetIndex].label || '';
                    return datasetLabel + ': $' + number_format(tooltipItem.yLabel);
                }
            }
        }
    }
});

//$(document).ready(function () {   // gọi function này ở đâu //lúc khởi chạy view á
//    //var Id = $('#shopchoosen').val();   //dô đây trước nè // id shopchossen ở đâu

//    console.log(Id);
//    ajax_chart('/Admin/Home/Getdata', 0);//rồi dô đây

//})
////function ChooseChanged(obj) {
////    console.log(obj.value);
////    statistic_number('/Admin/Home/Getdatanumber/' + obj.value, 0);
////    ajax_chart('/Admin/Home/Getdata/' + obj.value, 0);//rồi dô đây
////    //statistic_table('/Admin/Home/GetdatTable/' + obj.value);
////    return obj.value;
////}
////function statistic_number(url, data) {
////    console.log(url);//đây nè
////    $.getJSON(url, data).done(function (response) {
////        console.log(response);//rồi xuống đây
////        $('#earning').val(response.earning);
////        $('#sumproduct').val(response.sumproduct);
////        $('#sumcustomer').val(response.sumcustomer);
////    })
////}
//function ajax_chart(url, data) {
//    console.log(url);//đây nè
//    $.getJSON(url, data).done(function (response) {
//        console.log(response);//rồi xuống đây

//        myAreaChart.data.labels = response.labels;
//        myAreaChart.data.datasets[0].data = response.values;
//        myAreaChart.update();
//    })
//}