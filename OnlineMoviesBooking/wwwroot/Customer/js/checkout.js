﻿$(document).ready(function(){
    console.log("ready");
    $.ajax({
        method: 'GET',
        url:'/Movie/getinfo/'
    });
});
setTimeout(function () {
    console.log('timeout');
    $('#form1').submit();
}, 30000);