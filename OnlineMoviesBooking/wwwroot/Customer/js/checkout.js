$(document).ready(function(){
    console.log("ready");
    $.ajax({
        method: 'GET',
        url:'/Customer/Movie/getinfo/'
    });
});